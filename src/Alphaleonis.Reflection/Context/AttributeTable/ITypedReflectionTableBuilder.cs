// Copyright (c) Peter Palotas
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Linq;

namespace Alphaleonis.Reflection.Context
{
   public interface ITypedReflectionTableBuilder
   {
      /// <summary>Gets the builder associated with this instance.</summary>
      /// <value>The builder.</value>
      IReflectionTableBuilder Builder { get; }
   }

   /// <summary>Represents an <see cref="IReflectionTableBuilder"/> that is associated with a specific type, used by various extension methods..</summary>
   /// <typeparam name="T">Generic type parameter.</typeparam>
   public interface ITypedReflectionTableBuilder<T> : ITypedReflectionTableBuilder
   {
   }

   public interface IMappedTypedReflectionTableBuilder<T> : ITypedReflectionTableBuilder
   {
      Type TargetType { get; }
   }
}
