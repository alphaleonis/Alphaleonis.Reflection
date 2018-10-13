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
      protected class ProjectedPropertyInfo<TContext> : DelegatingPropertyInfo, IProjector<TContext> where TContext : CustomReflectionContextBase
      {
         public ProjectedPropertyInfo(PropertyInfo property, TContext reflectionContext)
            : base(property)
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

         public override bool IsDefined(Type attributeType, bool inherit)
         {
            if (attributeType.IsAssignableFrom(typeof(CustomReflectionContextIdAttribute)))
               return true;

            return base.IsDefined(attributeType, inherit);
         }

         public override MethodInfo GetGetMethod(bool nonPublic) => ReflectionContext.MapMember(base.GetGetMethod(nonPublic));

         public override MethodInfo GetSetMethod(bool nonPublic) => ReflectionContext.MapMember(base.GetSetMethod(nonPublic));

         public override MethodInfo[] GetAccessors(bool nonPublic) => ReflectionContext.MapMembers(base.GetAccessors(nonPublic));
         
         public override Type DeclaringType => ReflectionContext.MapType(base.DeclaringType);

         public override ParameterInfo[] GetIndexParameters() => ReflectionContext.MapParameters(base.GetIndexParameters());

         public override Type[] GetOptionalCustomModifiers() => ReflectionContext.MapTypes(base.GetOptionalCustomModifiers());

         public override Type[] GetRequiredCustomModifiers() => ReflectionContext.MapTypes(base.GetRequiredCustomModifiers());

         public override Type PropertyType => ReflectionContext.MapType(base.PropertyType);

         public override Type ReflectedType => ReflectionContext.MapType(base.ReflectedType);

         public override MethodInfo SetMethod => ReflectionContext.MapMember(base.SetMethod);
      }      
   }

   
}
