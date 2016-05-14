using System;
using System.Linq;
using System.Reflection;
using CustomAttributeTable;
using System.Collections.Generic;

namespace CustomAttributeTableTests
{
   public partial class AttributeTableReflectionContext
   {
      private class AttributeTableProjectedFieldInfo : DelegatingFieldInfo, IAttributeTableProjector
      {
         public AttributeTableProjectedFieldInfo(FieldInfo field, AttributeTableReflectionContext reflectionContext)
            : base(field)
         {
            ReflectionContext = reflectionContext;
         }

         public AttributeTableReflectionContext ReflectionContext { get; }

         public override Type FieldType => ReflectionContext.MapType(base.FieldType);

         public override Type DeclaringType => ReflectionContext.MapType(base.DeclaringType);

         public override object[] GetCustomAttributes(bool inherit)
         {
            return GetCustomAttributes(typeof(Attribute), inherit);
         }

         public override object[] GetCustomAttributes(Type attributeType, bool inherit)
         {
            List<object> result = new List<object>();

            // Add the context identifier attribute.
            result.Add(ReflectionContext.ContextIdentifierAttribute);

            // Then add all the attributes from the attribute table.
            result.AddRange(ReflectionContext.Table.GetCustomAttributes(this).Where(attr => attributeType.IsAssignableFrom(attr.GetType())));

            // Then check this type, without inheritance. (Field attributes are not inherited). Add only attributes if Multiple = true OR attribute not already exists.
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

         public override Type[] GetOptionalCustomModifiers() => ReflectionContext.MapTypes(base.GetOptionalCustomModifiers());

         public override Type[] GetRequiredCustomModifiers() => ReflectionContext.MapTypes(base.GetRequiredCustomModifiers());

         public override bool IsDefined(Type attributeType, bool inherit)
         {
            throw new NotImplementedException();
         }

         public override Type ReflectedType => ReflectionContext.MapType(base.ReflectedType);
      }
   }
}
