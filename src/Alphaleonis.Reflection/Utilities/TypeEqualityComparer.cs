// Copyright (c) Peter Palotas
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphaleonis.Reflection
{
   /// <summary>
   /// An equality comparer that will compare two <see langword="Type"/> instances for equality by 
   /// returning <see langword="true"/> if the underlying system type of the two instances are equal.
   /// </summary>
   [Serializable]
   public sealed class TypeEqualityComparer : IEqualityComparer<Type>
   {
      private readonly static TypeEqualityComparer s_default = new TypeEqualityComparer();

      public static IEqualityComparer<Type> Default
      {
         get
         {
            return s_default;
         }
      }

      public bool Equals(Type x, Type y)
      {
         if (object.ReferenceEquals(x, null))
            return object.ReferenceEquals(y, null);
         
         return x.Equals(y);
      }

      public int GetHashCode(Type obj)
      {
         if (obj == null)
            return 0;

         //The hash code for Type and TypeInfo is different if we do not use UnderlyingSystemType.
         return obj.UnderlyingSystemType.GetHashCode();
      }
   }


}
