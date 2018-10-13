// Copyright (c) Peter Palotas
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Alphaleonis.Reflection.Context
{
   public partial class CustomReflectionContextBase
   {
      protected class ProjectedMethodInfo<TContext> : DelegatingMethodInfo, IProjector<TContext> where TContext : CustomReflectionContextBase
      {
         public ProjectedMethodInfo(MethodInfo method, TContext reflectionContext)
            : base(method)
         {
            ReflectionContext = reflectionContext;
         }

         public TContext ReflectionContext { get; }

         CustomReflectionContextBase IProjector.ReflectionContext => ReflectionContext;

         public override Type DeclaringType => ReflectionContext.MapType(base.DeclaringType);

         public override MethodInfo GetBaseDefinition() => ReflectionContext.MapMember(base.GetBaseDefinition());

         public override object[] GetCustomAttributes(bool inherit)
         {
            return GetCustomAttributes(typeof(Attribute), inherit);
         }

         public override object[] GetCustomAttributes(Type attributeType, bool inherit)
         {
            return ReflectionContext.AddContextIdentifierAttributeIfRequested(attributeType, base.GetCustomAttributes(attributeType, inherit));
         }


         public override ParameterInfo[] GetParameters() => ReflectionContext.MapParameters(base.GetParameters());

         public override Type[] GetGenericArguments() => ReflectionContext.MapTypes(base.GetGenericArguments());

         public override MethodInfo GetGenericMethodDefinition() => ReflectionContext.MapMember(base.GetGenericMethodDefinition());

         public override bool IsDefined(Type attributeType, bool inherit)
         {
            if (attributeType.IsAssignableFrom(typeof(CustomReflectionContextIdAttribute)))
               return true;

            return base.IsDefined(attributeType, inherit);
         }

         public override MethodInfo MakeGenericMethod(params Type[] typeArguments) => ReflectionContext.MapMember(base.MakeGenericMethod(ReflectionContext.MapTypes(typeArguments)));

         public override Type ReflectedType => ReflectionContext.MapType(base.ReflectedType);

         public override ParameterInfo ReturnParameter => ReflectionContext.MapParameter(base.ReturnParameter);

         public override Type ReturnType => ReflectionContext.MapType(base.ReturnType);

         public override ICustomAttributeProvider ReturnTypeCustomAttributes => ReturnType;
      }
   }
}
