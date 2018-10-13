// Copyright (c) Peter Palotas
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphaleonis.Reflection.Context
{
   /// <summary>
   /// Class providing methods to be used as placeholders for arguments in an expression used to add
   /// attributes to parameters in a <see cref="IReflectionTableBuilder"/>.
   /// </summary>
   public static class Decorate
   {
      /// <summary>Represents a parameter of type <typeparamref name="T"/> in an expression, adding the specified <paramref name="attributes"/> to it.</summary>
      /// <typeparam name="T">The type of the parameter.</typeparam>
      /// <param name="attributes">The attributes to add to the parameter.</param>
      /// <returns>A placeholder value.</returns>
      public static T Parameter<T>(IEnumerable<Attribute> attributes) => default(T);

      /// <summary>Represents a parameter of type <typeparamref name="T"/> in an expression, adding the specified <paramref name="attributes"/> to it.</summary>
      /// <typeparam name="T">The type of the parameter.</typeparam>
      /// <param name="attributes">The attributes to add to the parameter.</param>
      /// <returns>A placeholder value.</returns>
      public static T Parameter<T>(params Attribute[] attributes) => default(T);
   }
}
