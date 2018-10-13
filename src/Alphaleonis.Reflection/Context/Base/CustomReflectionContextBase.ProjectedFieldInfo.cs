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
      /// A default projector that wraps a <see cref="FieldInfo"/> for a reflection context.
      /// </summary>
      /// <remarks>
      /// This projector is intended to be used as a base class for projectors in derived reflection contexts. It 
      /// adds no functionality to the underlying object, other than ensuring that all
      /// types and members returned from the object are also mapped in the same reflection context.
      /// </remarks>
      /// <typeparam name="TContext">The type of the reflection context. Must inherit from
      /// <see cref="CustomReflectionContextBase"/>.</typeparam>
      protected class ProjectedFieldInfo<TContext> : DelegatingFieldInfo, IProjector<TContext> where TContext : CustomReflectionContextBase
      {
         /// <summary>Constructor.</summary>
         /// <param name="field">The <see cref="FieldInfo"/> to wrap.</param>
         /// <param name="reflectionContext">The owner reflection context.</param>
         public ProjectedFieldInfo(FieldInfo field, TContext reflectionContext)
            : base(field)
         {
            ReflectionContext = reflectionContext;
         }

         /// <summary>Gets the owner <see cref="ReflectionContext"/>.</summary>     
         public TContext ReflectionContext { get; }

         CustomReflectionContextBase IProjector.ReflectionContext => ReflectionContext;

         /// <inheritdoc/> 
         public override Type FieldType => ReflectionContext.MapType(base.FieldType);

         /// <inheritdoc/> 
         public override Type DeclaringType => ReflectionContext.MapType(base.DeclaringType);

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
         public override Type[] GetOptionalCustomModifiers() => ReflectionContext.MapTypes(base.GetOptionalCustomModifiers());

         /// <inheritdoc/> 
         public override Type[] GetRequiredCustomModifiers() => ReflectionContext.MapTypes(base.GetRequiredCustomModifiers());

         /// <inheritdoc/> 
         public override Type ReflectedType => ReflectionContext.MapType(base.ReflectedType);
      }
   }
}
