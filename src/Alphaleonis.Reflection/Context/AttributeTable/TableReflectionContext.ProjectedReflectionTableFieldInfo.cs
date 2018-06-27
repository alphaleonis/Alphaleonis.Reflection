using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Alphaleonis.Reflection.Context;

namespace Alphaleonis.Reflection.Context
{
   public partial class TableReflectionContext
   {
      protected class ProjectedReflectionTableFieldInfo : ProjectedFieldInfo<TableReflectionContext>
      {
         public ProjectedReflectionTableFieldInfo(FieldInfo field, TableReflectionContext reflectionContext) 
            : base(field, reflectionContext)
         {
         }

         public override object[] GetCustomAttributes(Type attributeType, bool inherit)
         {
            List<object> result = new List<object>();

            // Then add all the attributes from the attribute table.
            result.AddRange(ReflectionContext.Table.GetCustomAttributes(this).Where(attr => attributeType.IsAssignableFrom(attr.GetType())));

            // Then check this type, without inheritance. (Field attributes are not inherited). Add only attributes if Multiple = true OR attribute not already exists.
            foreach (var ca in base.GetCustomAttributes(attributeType, false))
            {
               if (AttributeUtil.GetAttributeUsage(ca.GetType()).AllowMultiple || !result.Any(attr => attr.GetType().Equals(ca.GetType())))
                  result.Add(ca);
            }

            // Finally create a resulting array of the correct type.
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
