using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphaleonis.Reflection.Context
{
   public partial class TableReflectionContext
   {
      protected class ProjectedReflectionTableType : ProjectedType<TableReflectionContext>
      {
         public ProjectedReflectionTableType(Type delegatingType, TableReflectionContext context) 
            : base(delegatingType, context)
         {
         }

         public override object[] GetCustomAttributes(bool inherit)
         {
            return GetCustomAttributes(typeof(Attribute), inherit);
         }

         public override object[] GetCustomAttributes(Type attributeType, bool inherit)
         {
            bool stop = this.IsGenericType;
            // Start with the table attributes.
            List<object> result = new List<object>();

            result.Add(ReflectionContext.ContextIdentifierAttribute);

            result.AddRange(ReflectionContext.Table.GetCustomAttributes(this).Where(attr => attributeType.IsAssignableFrom(attr.GetType())));

            // Then check this type, without inheritance. Add only attributes if Multiple = true OR attribute not already exists.
            foreach (var ca in base.GetCustomAttributes(attributeType, false))
            {
               if (GetAttributeUsage(ca.GetType()).AllowMultiple || !result.Any(attr => attr.GetType().Equals(ca.GetType())))
                  result.Add(ca);
            }

            // Then get base attributes, add only if Inherit = true AND (Multiple = true OR attribute not already exists).
            if (inherit && BaseType != null && !BaseType.Equals(typeof(object)))
            {
               foreach (var ca in BaseType.GetCustomAttributes(attributeType, inherit))
               {
                  AttributeUsageAttribute attributeUsage = GetAttributeUsage(ca.GetType());
                  if (attributeUsage.Inherited && (attributeUsage.AllowMultiple || !result.Any(attr => attr.GetType().Equals(ca.GetType()))))
                     result.Add(ca);
               }
            }

            object[] arrResult = (object[])Array.CreateInstance(attributeType, result.Count);
            for (int i = 0; i < result.Count; i++)
            {
               arrResult[i] = result[i];
            }

            return arrResult;
         }

         public override bool IsDefined(Type attributeType, bool inherit)
         {
            return GetCustomAttributes(attributeType, inherit).Length > 0;
         }
      }
   }




}
