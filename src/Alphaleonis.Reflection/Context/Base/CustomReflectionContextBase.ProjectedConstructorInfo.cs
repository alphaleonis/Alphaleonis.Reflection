using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Alphaleonis.Reflection.Context
{
   public partial class CustomReflectionContextBase
   {
      protected class ProjectedConstructorInfo<TContext> : DelegatingConstructorInfo, IProjector<TContext> where TContext : CustomReflectionContextBase
      {
         public ProjectedConstructorInfo(ConstructorInfo constructor, TContext reflectionContext)
            : base(constructor)
         {
            ReflectionContext = reflectionContext;
         }

         public TContext ReflectionContext { get; }

         CustomReflectionContextBase IProjector.ReflectionContext => ReflectionContext;

         public override Type DeclaringType => ReflectionContext.MapType(base.DeclaringType);

         public override object[] GetCustomAttributes(bool inherit) => GetCustomAttributes(typeof(Attribute), inherit);

         public override object[] GetCustomAttributes(Type attributeType, bool inherit)
         {
            return ReflectionContext.AddContextIdentifierAttributeIfRequested(attributeType, base.GetCustomAttributes(attributeType, inherit));
         }

         public override Type[] GetGenericArguments() => ReflectionContext.MapTypes(base.GetGenericArguments());

         public override ParameterInfo[] GetParameters() => ReflectionContext.MapParameters(base.GetParameters());

         public override bool IsDefined(Type attributeType, bool inherit)
         {
            if (attributeType.IsAssignableFrom(typeof(CustomReflectionContextIdAttribute)))
               return true;

            return base.IsDefined(attributeType, inherit);          
         }

         public override Type ReflectedType => ReflectionContext.MapType(base.ReflectedType);         
      }
   }
}
