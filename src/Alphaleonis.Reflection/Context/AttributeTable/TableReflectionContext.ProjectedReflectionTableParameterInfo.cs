using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Alphaleonis.Reflection.Context
{
   public partial class TableReflectionContext
   {
      protected class ProjectedReflectionTableParameterInfo : ProjectedParameterInfo<TableReflectionContext>
      {
         public ProjectedReflectionTableParameterInfo(ParameterInfo parameter, TableReflectionContext reflectionContext) 
            : base(parameter, reflectionContext)
         {
         }

         public override object[] GetCustomAttributes(Type attributeType, bool inherit)
         {
            List<object> result = new List<object>();

            // Add the context identifier attribute.
            result.Add(ReflectionContext.ContextIdentifierAttribute);

            // Then add all the attributes from the attribute table.
            result.AddRange(ReflectionContext.Table.GetCustomAttributes(this).Where(attr => attributeType.IsAssignableFrom(attr.GetType())));

            // Then check this type, without inheritance. (Parameter attributes are not inherited). Add only attributes if Multiple = true OR attribute not already exists.
            foreach (var ca in base.GetCustomAttributes(attributeType, false))
            {
               if (GetAttributeUsage(ca.GetType()).AllowMultiple || !result.Any(attr => attr.GetType().Equals(ca.GetType())))
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
            // Then add any attributes defined in the reflection context table.
            if (ReflectionContext.Table.GetCustomAttributes(this).Any(attr => attributeType.IsAssignableFrom(attr.GetType())))
               return true;

            return base.IsDefined(attributeType, false);
         }
      }
   }




}
