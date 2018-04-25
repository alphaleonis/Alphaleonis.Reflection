using System;
using System.Collections.Generic;

namespace Alphaleonis.Reflection
{

   public partial class CustomAttributeTableBuilder
   {
      /// <summary>A type equality comparer that ignores type parameters.</summary>
      private class TypeEqualityComparerIgnoringTypeParameters : IEqualityComparer<Type>
      {
         public static readonly IEqualityComparer<Type> Default = new TypeEqualityComparerIgnoringTypeParameters();

         public bool Equals(Type x, Type y)
         {
            if (x == null)
               return y == null;

            if (y == null)
               return false;

            if (x.IsGenericType)
               x = x.GetGenericTypeDefinition();

            if (y.IsGenericType)
               y = y.GetGenericTypeDefinition();

            return x.UnderlyingSystemType.Equals(y.UnderlyingSystemType);
         }

         public int GetHashCode(Type obj)
         {
            if (obj == null)
               return 0;

            if (obj.IsGenericType)
               obj = obj.GetGenericTypeDefinition();

            return obj.GetHashCode();
         }
      }      
   }

   // TODO PP: Add a simplified builder that is specific to a type, eg. builder.ForType<MyType>().AddMemberAttributes(c => c.MyProperty);
   //                                                                                                                ^ Note: No generic argument here!
}
