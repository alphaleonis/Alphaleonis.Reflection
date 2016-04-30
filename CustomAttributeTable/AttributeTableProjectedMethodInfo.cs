using System;
using System.Linq;
using System.Reflection;
using CustomAttributeTable;
using System.Collections.Generic;

namespace CustomAttributeTableTests
{
   public partial class AttributeTableReflectionContext
   {
      public class AttributeTableProjectedMethodInfo : DelegatingMethodInfo, IAttributeTableProjector
      {
         public AttributeTableProjectedMethodInfo(MethodInfo method, AttributeTableReflectionContext reflectionContext)
            : base(method)
         {
            ReflectionContext = reflectionContext;
         }

         public AttributeTableReflectionContext ReflectionContext { get; }

         public override object[] GetCustomAttributes(bool inherit)
         {
            return GetCustomAttributes(typeof(Attribute), inherit);
         }

         public override object[] GetCustomAttributes(Type attributeType, bool inherit)
         {
            List<object> result = new List<object>();

            // Add the reflection context identifier attribute.
            result.Add(ReflectionContext.ContextIdentifierAttribute);

            // Then add any attributes defined in the reflection context table.
            result.AddRange(ReflectionContext.Table.GetCustomAttributes(this).Where(attr => attributeType.IsAssignableFrom(attr.GetType())));

            // Then check this method, without inheritance. Add only attributes if Multiple = true OR attribute not already exists.
            foreach (var ca in base.GetCustomAttributes(attributeType, false))
            {
               if (GetAttributeUsage(ca.GetType()).AllowMultiple || !result.Any(attr => attr.GetType().Equals(ca.GetType())))
                  result.Add(ca);
            }

            // If we want also inherited attributes, we must check the base definition.
            if (inherit)
            {
               // ...if it is different from the declaring type of this method, we get all attributes from there and add them as well, depending
               // on their attribute usage settings.
               MethodInfo baseMethod = GetBaseDefinition();
               
               // Then get base attributes, add only if Inherit = true AND (Multiple = true OR attribute not already exists).
               if (baseMethod != null && !baseMethod.DeclaringType.Equals(DeclaringType))
               {
                  foreach (var ca in baseMethod.GetCustomAttributes(attributeType, true))
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

         public override ParameterInfo[] GetParameters()
         {
            return base.GetParameters().Select(parameter => ReflectionContext.MapParameter(parameter)).ToArray();
         }
      }
   }
}
