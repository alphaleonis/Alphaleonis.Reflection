using System;
using System.Linq;
using System.Reflection;
using CustomAttributeTable;
using System.Collections.Generic;

namespace CustomAttributeTableTests
{
   public partial class AttributeTableReflectionContext
   {
      private class AttributeTableProjectedPropertyInfo : DelegatingPropertyInfo, IAttributeTableProjector
      {
         public AttributeTableProjectedPropertyInfo(PropertyInfo property, AttributeTableReflectionContext reflectionContext)
            : base(property)
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
            if (inherit && (ReflectionContext.Options & AttributeTableReflectionContextOptions.HonorPropertyAttributeInheritance) != 0)
            {
               // ...if it is different from the declaring type of this method, we get all attributes from there and add them as well, depending
               // on their attribute usage settings.
               MemberInfo baseProperty = this.GetParentDefinition();

               // Then get base attributes, add only if Inherit = true AND (Multiple = true OR attribute not already exists).
               if (baseProperty != null && !baseProperty.DeclaringType.Equals(DeclaringType))
               {
                  foreach (var ca in baseProperty.GetCustomAttributes(attributeType, true))
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

         public override MethodInfo GetGetMethod(bool nonPublic) => ReflectionContext.MapMember(base.GetGetMethod(nonPublic));

         public override MethodInfo GetSetMethod(bool nonPublic) => ReflectionContext.MapMember(base.GetGetMethod(nonPublic));

         public override MethodInfo[] GetAccessors(bool nonPublic) => ReflectionContext.MapMembers(base.GetAccessors(nonPublic));
         
         public override Type DeclaringType => ReflectionContext.MapType(base.DeclaringType);

         public override ParameterInfo[] GetIndexParameters() => ReflectionContext.MapParameters(base.GetIndexParameters());

         public override Type[] GetOptionalCustomModifiers() => ReflectionContext.MapTypes(base.GetOptionalCustomModifiers());

         public override Type[] GetRequiredCustomModifiers() => ReflectionContext.MapTypes(base.GetRequiredCustomModifiers());

         public override Type PropertyType => ReflectionContext.MapType(base.PropertyType);

         public override Type ReflectedType => ReflectionContext.MapType(base.ReflectedType);

         public override MethodInfo SetMethod => ReflectionContext.MapMember(base.SetMethod);

         public override bool IsDefined(Type attributeType, bool inherit)
         {
            // Then add any attributes defined in the reflection context table.
            if (ReflectionContext.Table.GetCustomAttributes(this).Any(attr => attributeType.IsAssignableFrom(attr.GetType())))
               return true;

            if (base.IsDefined(attributeType, false))
               return true;

            if (inherit && (ReflectionContext.Options & AttributeTableReflectionContextOptions.HonorPropertyAttributeInheritance) != 0)
            {
               return this.GetParentDefinition()?.IsDefined(attributeType, true) == true;
            }

            return false;
         }
      }      
   }

   
}
