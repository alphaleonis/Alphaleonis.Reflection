// Copyright (c) Peter Palotas
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Linq;

namespace Alphaleonis.Reflection.Context
{
   /// <summary>Represents an <see cref="IReflectionTableBuilder"/> that is associated with a specific type, used by various extension methods.</summary>
   public interface ITypedReflectionTableBuilder
   {
      /// <summary>Gets the builder associated with this instance.</summary>      
      IReflectionTableBuilder Builder { get; }
   }

   /// <summary>Represents an <see cref="IReflectionTableBuilder"/> that is associated with a specific type, used by various extension methods.</summary>
   /// <typeparam name="T">The type with which this builder is associated.</typeparam>
   public interface ITypedReflectionTableBuilder<T> : ITypedReflectionTableBuilder
   {
   }

   /// <summary>
   /// Represents an <see cref="IReflectionTableBuilder"/> that is associated with a specific type,
   /// using another type for reflection. Used by various extension methods.
   /// </summary>
   /// <typeparam name="T">The type with which this builder is associated.</typeparam>
   public interface IMappedTypedReflectionTableBuilder<T> : ITypedReflectionTableBuilder
   {
      /// <summary>Gets the target type to which to apply the changes.</summary>
      Type TargetType { get; }
   }
}
