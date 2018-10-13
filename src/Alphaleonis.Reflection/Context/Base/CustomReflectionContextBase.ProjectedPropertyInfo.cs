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
      /// <summary>
      /// A default projector that wraps a <see cref="PropertyInfo"/> for a reflection context.
      /// </summary>
      /// <remarks>
      /// This projector is intended to be used as a base class for projectors in derived reflection contexts. It 
      /// adds no functionality to the underlying object, other than ensuring that all
      /// types and members returned from the object are also mapped in the same reflection context.
      /// </remarks>
      /// <typeparam name="TContext">The type of the reflection context. Must inherit from
      /// <see cref="CustomReflectionContextBase"/>.</typeparam>
      protected class ProjectedPropertyInfo<TContext> : DelegatingPropertyInfo, IProjector<TContext> where TContext : CustomReflectionContextBase
      {
         /// <summary>Constructor.</summary>
         /// <param name="property">The property to wrap.</param>
         /// <param name="reflectionContext">The owner refleciton context.</param>
         public ProjectedPropertyInfo(PropertyInfo property, TContext reflectionContext)
            : base(property)
         {
            ReflectionContext = reflectionContext;
         }

         /// <summary>The owner reflection context.</summary>
         public TContext ReflectionContext { get; }

         CustomReflectionContextBase IProjector.ReflectionContext => ReflectionContext;

         /// <inheritdoc/> 
         public override object[] GetCustomAttributes(bool inherit) => GetCustomAttributes(typeof(Attribute), inherit);

         /// <inheritdoc/> 
         public override object[] GetCustomAttributes(Type attributeType, bool inherit) => ReflectionContext.AddContextIdentifierAttributeIfRequested(attributeType, base.GetCustomAttributes(attributeType, inherit));

         /// <inheritdoc/> 
         public override bool IsDefined(Type attributeType, bool inherit)
         {
            if (attributeType.IsAssignableFrom(typeof(CustomReflectionContextIdAttribute)))
               return true;

            return base.IsDefined(attributeType, inherit);
         }

         /// <inheritdoc/> 
         public override MethodInfo GetGetMethod(bool nonPublic) => ReflectionContext.MapMember(base.GetGetMethod(nonPublic));

         /// <inheritdoc/> 
         public override MethodInfo GetSetMethod(bool nonPublic) => ReflectionContext.MapMember(base.GetSetMethod(nonPublic));

         /// <inheritdoc/> 
         public override MethodInfo[] GetAccessors(bool nonPublic) => ReflectionContext.MapMembers(base.GetAccessors(nonPublic));

         /// <inheritdoc/> 
         public override Type DeclaringType => ReflectionContext.MapType(base.DeclaringType);

         /// <inheritdoc/> 
         public override ParameterInfo[] GetIndexParameters() => ReflectionContext.MapParameters(base.GetIndexParameters());

         /// <inheritdoc/> 
         public override Type[] GetOptionalCustomModifiers() => ReflectionContext.MapTypes(base.GetOptionalCustomModifiers());

         /// <inheritdoc/> 
         public override Type[] GetRequiredCustomModifiers() => ReflectionContext.MapTypes(base.GetRequiredCustomModifiers());

         /// <inheritdoc/> 
         public override Type PropertyType => ReflectionContext.MapType(base.PropertyType);

         /// <inheritdoc/> 
         public override Type ReflectedType => ReflectionContext.MapType(base.ReflectedType);

         /// <inheritdoc/> 
         public override MethodInfo SetMethod => ReflectionContext.MapMember(base.SetMethod);

         /// <inheritdoc/> 
         public override MethodInfo GetMethod => ReflectionContext.MapMember(base.GetMethod);         
      }      
   }

   
}
