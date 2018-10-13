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
      protected class ProjectedParameterInfo<TContext> : DelegatingParameterInfo, IProjector<TContext> where TContext : CustomReflectionContextBase
      {
         public ProjectedParameterInfo(ParameterInfo parameter, TContext reflectionContext)
            : base(parameter)
         {
            ReflectionContext = reflectionContext;
         }

         public TContext ReflectionContext { get; }

         CustomReflectionContextBase IProjector.ReflectionContext => ReflectionContext;

         public override object[] GetCustomAttributes(bool inherit)
         {
            return GetCustomAttributes(typeof(Attribute), inherit);
         }

         public override object[] GetCustomAttributes(Type attributeType, bool inherit)
         {
            return ReflectionContext.AddContextIdentifierAttributeIfRequested(attributeType, base.GetCustomAttributes(attributeType, inherit));
         }

         public override Type[] GetOptionalCustomModifiers() => ReflectionContext.MapTypes(base.GetOptionalCustomModifiers());

         public override Type[] GetRequiredCustomModifiers() => ReflectionContext.MapTypes(base.GetRequiredCustomModifiers());

         public override bool IsDefined(Type attributeType, bool inherit)
         {
            if (attributeType.IsAssignableFrom(typeof(CustomReflectionContextIdAttribute)))
               return true;

            return base.IsDefined(attributeType, inherit);
         }

         public override MemberInfo Member => ReflectionContext.MapMember(base.Member);

         public override Type ParameterType => ReflectionContext.MapType(base.ParameterType);         
      }
   }

   
}
