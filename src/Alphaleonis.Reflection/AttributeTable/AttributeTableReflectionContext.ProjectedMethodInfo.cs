using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Alphaleonis.Reflection
{
   public partial class AttributeTableReflectionContext
   {
      public class ProjectedMethodInfo : DelegatingMethodInfo, IAttributeTableProjector
      {
         public ProjectedMethodInfo(MethodInfo method, AttributeTableReflectionContext reflectionContext)
            : base(method)
         {
            ReflectionContext = reflectionContext;
         }

         public AttributeTableReflectionContext ReflectionContext { get; }

         public override Type DeclaringType => ReflectionContext.MapType(base.DeclaringType);

         public override MethodInfo GetBaseDefinition()
         {
            return ReflectionContext.MapMember(base.GetBaseDefinition());
         }

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

         public override ParameterInfo[] GetParameters() => ReflectionContext.MapParameters(base.GetParameters());

         public override Type[] GetGenericArguments() => ReflectionContext.MapTypes(base.GetGenericArguments());

         public override MethodInfo GetGenericMethodDefinition() => ReflectionContext.MapMember(base.GetGenericMethodDefinition());

         public override bool IsDefined(Type attributeType, bool inherit)
         {
            // Then add any attributes defined in the reflection context table.
            if (ReflectionContext.Table.GetCustomAttributes(this).Any(attr => attributeType.IsAssignableFrom(attr.GetType())))
               return true;

            if (base.IsDefined(attributeType, false))
               return true;

            if (inherit)
            {
               return this.GetParentDefinition()?.IsDefined(attributeType, true) == true;
            }

            return false;
         }

         public override MethodInfo MakeGenericMethod(params Type[] typeArguments) => ReflectionContext.MapMember(base.MakeGenericMethod(ReflectionContext.MapTypes(typeArguments)));

         public override Type ReflectedType => ReflectionContext.MapType(base.ReflectedType);

         public override ParameterInfo ReturnParameter => ReflectionContext.MapParameter(base.ReturnParameter);

         public override Type ReturnType => ReflectionContext.MapType(base.ReturnType);

         public override ICustomAttributeProvider ReturnTypeCustomAttributes
         {
            get
            {
               throw new NotImplementedException();
            }
         }
      }
   }
}
