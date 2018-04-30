using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Alphaleonis.Reflection.Context;

namespace Alphaleonis.Reflection.Context
{
   public partial class TableReflectionContext
   {
      protected class ProjectedReflectionTableEventInfo : ProjectedEventInfo<TableReflectionContext>
      {
         public ProjectedReflectionTableEventInfo(EventInfo eventInfo, TableReflectionContext reflectionContext) 
            : base(eventInfo, reflectionContext)
         {
         }

         public override object[] GetCustomAttributes(Type attributeType, bool inherit)
         {
            List<object> result = new List<object>();

            // Then add any attributes defined in the reflection context table.
            result.AddRange(ReflectionContext.Table.GetCustomAttributes(this).Where(attr => attributeType.IsAssignableFrom(attr.GetType())));

            // Then check this method, without inheritance. Add only attributes if Multiple = true OR attribute not already exists.
            foreach (var ca in base.GetCustomAttributes(attributeType, false))
            {
               if (GetAttributeUsage(ca.GetType()).AllowMultiple || !result.Any(attr => attr.GetType().Equals(ca.GetType())))
                  result.Add(ca);
            }

            // If we want also inherited attributes, we must check the base definition.
            if (inherit && (ReflectionContext.Options & TableReflectionContextOptions.HonorEventAttributeInheritance) != 0)
            {
               // ...if it is different from the declaring type of this method, we get all attributes from there and add them as well, depending
               // on their attribute usage settings.
               MemberInfo baseEvent = this.GetParentDefinition();

               // Then get base attributes, add only if Inherit = true AND (Multiple = true OR attribute not already exists).
               if (baseEvent != null && !baseEvent.DeclaringType.Equals(DeclaringType))
               {
                  foreach (var ca in baseEvent.GetCustomAttributes(attributeType, true))
                  {
                     AttributeUsageAttribute attributeUsage = GetAttributeUsage(ca.GetType());
                     if (attributeUsage.Inherited && (attributeUsage.AllowMultiple || !result.Any(attr => attr.GetType().Equals(ca.GetType()))))
                        result.Add(ca);
                  }
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
            // Then add any attributes defined in the reflection context table.
            if (ReflectionContext.Table.GetCustomAttributes(this).Any(attr => attributeType.IsAssignableFrom(attr.GetType())))
               return true;

            if (base.IsDefined(attributeType, false))
               return true;

            if (inherit && (ReflectionContext.Options & TableReflectionContextOptions.HonorEventAttributeInheritance) != 0)
            {
               return this.GetParentDefinition()?.IsDefined(attributeType, true) == true;
            }

            return false;
         }
      }
   }




}
