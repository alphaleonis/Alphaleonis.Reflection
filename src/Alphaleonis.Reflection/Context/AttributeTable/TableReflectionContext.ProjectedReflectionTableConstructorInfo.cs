using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Alphaleonis.Reflection.Context
{

   public partial class TableReflectionContext
   {
      protected class ProjectedReflectionTableConstructorInfo : ProjectedConstructorInfo<TableReflectionContext>
      {
         public ProjectedReflectionTableConstructorInfo(ConstructorInfo constructor, TableReflectionContext reflectionContext) 
            : base(constructor, reflectionContext)
         {
             m_parentDefinition = new Lazy<MemberInfo>(() => this.GetParentDefinition());
         }
         
         public override object[] GetCustomAttributes(Type attributeType, bool inherit)
         {
            List<object> result = new List<object>();

            // Then add any attributes defined in the reflection context table.
            result.AddRange(ReflectionContext.Table.GetCustomAttributes(this).Where(attr => attributeType.IsAssignableFrom(attr.GetType())));

            // Then check this method, without inheritance. Add only attributes if Multiple = true OR attribute not already exists.
            foreach (var ca in base.GetCustomAttributes(attributeType, false))
            {
               if (AttributeUtil.GetAttributeUsage(ca.GetType()).AllowMultiple || !result.Any(attr => attr.GetType().Equals(ca.GetType())))
                  result.Add(ca);
            }

            object[] arrResult = (object[])Array.CreateInstance(attributeType, result.Count);
            for (int i = 0; i < result.Count; i++)
            {
               arrResult[i] = result[i];
            }

            return arrResult;
         }

         private Lazy<MemberInfo> m_parentDefinition;

         public override bool IsDefined(Type attributeType, bool inherit)
         {
            List<object> result = new List<object>();

            // Then add any attributes defined in the reflection context table.
            if (ReflectionContext.Table.GetCustomAttributes(this).Any(attr => attributeType.IsAssignableFrom(attr.GetType())))
               return true;

            // Then check this method, without inheritance. 
            if (base.IsDefined(attributeType, false))
               return true;

            if (inherit)
            {
               return m_parentDefinition.Value?.IsDefined(attributeType, true) == true;
            }

            return false;
         }
      }
   }




}
