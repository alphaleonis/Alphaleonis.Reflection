// Copyright (c) Peter Palotas
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Alphaleonis.Reflection.Context
{
   public abstract partial class CustomReflectionContextBase
   {
      private interface IProjector
      {
         CustomReflectionContextBase ReflectionContext { get; }
      }

      private interface IProjector<out TContext> : IProjector where TContext : CustomReflectionContextBase
      {
         new TContext ReflectionContext { get; }
      }

   }
}
