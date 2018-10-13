// Copyright (c) Peter Palotas
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Linq;

namespace Alphaleonis.Reflection.Context
{
   /// <summary>
   /// Internal attribute used to detect whether a member has been mapped using a particular
   /// reflection context.
   /// </summary>
   [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
   public class CustomReflectionContextIdAttribute : Attribute
   {
      /// <summary>Constructor.</summary>
      /// <param name="contextId">Identifier of the reflection context.</param>
      public CustomReflectionContextIdAttribute(Guid contextId)
      {
         ContextId = contextId;
      }

      /// <summary>Gets the identifier of the reflection context.</summary>
      /// <value>The identifier of the reflection context.</value>
      public Guid ContextId { get; }
   }
}
