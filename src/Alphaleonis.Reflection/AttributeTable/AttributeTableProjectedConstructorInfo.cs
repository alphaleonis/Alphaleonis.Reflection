using Alphaleonis.Reflection;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Alphaleonis.Reflection
{
   public partial class AttributeTableReflectionContext
   {
      public class AttributeTableProjectedConstructorInfo : DelegatingConstructorInfo, IAttributeTableProjector
      {
         public AttributeTableProjectedConstructorInfo(ConstructorInfo constructor, AttributeTableReflectionContext reflectionContext)
            : base(constructor)
         {
            ReflectionContext = reflectionContext;
         }

         public AttributeTableReflectionContext ReflectionContext { get; }

         public override Type DeclaringType => ReflectionContext.MapType(base.DeclaringType);

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

            object[] arrResult = (object[])Array.CreateInstance(attributeType, result.Count);
            for (int i = 0; i < result.Count; i++)
            {
               arrResult[i] = result[i];
            }

            return arrResult;
         }

         public override Type[] GetGenericArguments() => ReflectionContext.MapTypes(base.GetGenericArguments());

         public override ParameterInfo[] GetParameters() => ReflectionContext.MapParameters(base.GetParameters());

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
               return this.GetParentDefinition()?.IsDefined(attributeType, true) == true;
            }

            return false;
         }

         public override Type ReflectedType => ReflectionContext.MapType(base.ReflectedType);         
      }
   }
}
