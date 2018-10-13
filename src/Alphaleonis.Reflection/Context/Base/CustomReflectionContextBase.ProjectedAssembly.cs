// Copyright (c) Peter Palotas
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Globalization;

namespace Alphaleonis.Reflection.Context
{
   public partial class CustomReflectionContextBase
   {
      /// <summary>
      /// A default projector that wraps an <see cref="Assembly"/> for a reflection context.
      /// </summary>
      /// <remarks>
      /// This projector is intended to be used as a base class for projectors in derived reflection contexts. It 
      /// adds no functionality to the underlying object, other than ensuring that all
      /// types and members returned from the object are also mapped in the same reflection context.
      /// </remarks>
      /// <typeparam name="TContext">The type of the reflection context. Must inherit from
      /// <see cref="CustomReflectionContextBase"/>.</typeparam>
      protected class ProjectedAssembly<TContext> : DelegatingAssembly, IProjector<TContext> where TContext : CustomReflectionContextBase
      {
         /// <summary>Constructor.</summary>
         /// <param name="assembly">The assembly to wrap.</param>
         /// <param name="reflectionContext">The reflection context through which this instance was mapped.</param>
         public ProjectedAssembly(Assembly assembly, TContext reflectionContext) : base(assembly)
         {
            ReflectionContext = reflectionContext;
         }

         /// <summary>Gets the owner <see cref="ReflectionContext"/>.</summary>         
         public TContext ReflectionContext { get; }

         CustomReflectionContextBase IProjector.ReflectionContext => ReflectionContext;

         /// <inheritdoc/> 
         public override object[] GetCustomAttributes(bool inherit)
         {
            return ReflectionContext.AddContextIdentifierAttribute(base.GetCustomAttributes(inherit));
         }

         /// <inheritdoc/> 
         public override object[] GetCustomAttributes(Type attributeType, bool inherit)
         {
            return ReflectionContext.AddContextIdentifierAttributeIfRequested(attributeType, base.GetCustomAttributes(attributeType, inherit));
         }

         /// <inheritdoc/> 
         public override Type GetType(string name, bool throwOnError, bool ignoreCase) => ReflectionContext.MapType(base.GetType(name, throwOnError, ignoreCase));

         /// <inheritdoc/> 
         public override Type[] GetTypes() => ReflectionContext.MapTypes(base.GetTypes());

         /// <inheritdoc/> 
         public override Type[] GetExportedTypes() => ReflectionContext.MapTypes(base.GetExportedTypes());

         /// <inheritdoc/> 
         public override IEnumerable<TypeInfo> DefinedTypes => base.DefinedTypes.Select(type => ReflectionContext.MapType(type));

         /// <inheritdoc/> 
         public override MethodInfo EntryPoint => ReflectionContext.MapMember(base.EntryPoint);

         /// <inheritdoc/> 
         public override IEnumerable<Type> ExportedTypes => base.ExportedTypes.Select(type => ReflectionContext.MapType(type));

         /// <inheritdoc/> 
         public override Assembly GetSatelliteAssembly(CultureInfo culture) => ReflectionContext.MapAssembly(base.GetSatelliteAssembly(culture));

         /// <inheritdoc/> 
         public override Assembly GetSatelliteAssembly(CultureInfo culture, Version version) => ReflectionContext.MapAssembly(base.GetSatelliteAssembly(culture, version));

         /// <inheritdoc/> 
         public override Type GetType(string name) => ReflectionContext.MapType(base.GetType(name));

         /// <inheritdoc/> 
         public override Type GetType(string name, bool throwOnError) => ReflectionContext.MapType(base.GetType(name, throwOnError));
      }
   }

}
