// Copyright (c) Peter Palotas
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Linq;

namespace Alphaleonis.Reflection.Context
{
   /// <summary>
   /// A class used as a placeholder for arguments in an expression to add attributes to a parameter
   /// in an <see cref="IReflectionTableBuilder"/> for parameters to which no attributes are to be
   /// added.
   /// </summary>
   public static class Param
   {
      /// <summary>Represents a parameter of type <typeparamref name="T"/> in an expression.</summary>
      /// <typeparam name="T">The type of the parameter.</typeparam>
      /// <returns>A placeholder value.</returns>
      public static T OfType<T>()
      {
         return default(T);
      }
   }
}
