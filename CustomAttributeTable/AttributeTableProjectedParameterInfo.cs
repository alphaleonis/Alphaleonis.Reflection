using System;
using System.Linq;
using System.Reflection;
using CustomAttributeTable;
using System.Collections.Generic;

namespace CustomAttributeTableTests
{
   public partial class AttributeTableReflectionContext
   {
      public class AttributeTableProjectedParameterInfo : DelegatingParameterInfo, IAttributeTableProjector
      {
         public AttributeTableProjectedParameterInfo(ParameterInfo parameter, AttributeTableReflectionContext reflectionContext)
            : base(parameter)
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

            // TODO PP: Remove commented code.
            //// Then get base attributes, add only if Inherit = true AND (Multiple = true OR attribute not already exists).
            //if (inherit && BaseType != null && !BaseType.Equals(typeof(object)))
            //{
            //   foreach (var ca in BaseType.GetCustomAttributes(attributeType, inherit))
            //   {
            //      AttributeUsageAttribute attributeUsage = GetAttributeUsage(ca.GetType());
            //      if (attributeUsage.Inherited && (attributeUsage.AllowMultiple || !result.Any(attr => attr.GetType().Equals(ca.GetType()))))
            //         result.Add(ca);
            //   }
            //}

            object[] arrResult = (object[])Array.CreateInstance(attributeType, result.Count);
            for (int i = 0; i < result.Count; i++)
            {
               arrResult[i] = result[i];
            }

            return arrResult;
         }
      }
   }

   
}
