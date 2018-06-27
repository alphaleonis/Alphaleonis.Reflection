using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace Alphaleonis.Reflection.Context
{   
   public partial class ReflectionTableBase
   {
      protected struct MethodKey : IEquatable<MethodKey>
      {
         public MethodKey(MethodBase method)
         {
            if (method == null)
               throw new ArgumentNullException(nameof(method), $"{nameof(method)} is null.");

            bool stop = method.DeclaringType.IsGenericType;
            MethodInfo methodInfo = method as MethodInfo;
            if (methodInfo != null && method.IsGenericMethod && !method.IsGenericMethodDefinition)
            {
               method = methodInfo.GetGenericMethodDefinition();
            }

            // If the MethodInfo comes from a closed generic type, we need to get the MethodInfo belonging to the open generic type.
            if (method.DeclaringType.IsGenericType && !method.DeclaringType.IsGenericTypeDefinition)
            {
               method = MethodBase.GetMethodFromHandle(method.MethodHandle, method.DeclaringType.GetGenericTypeDefinition().TypeHandle);
            }

            MethodName = method.Name;
            TypeArguments = method.IsGenericMethod ? method.GetGenericArguments().Length : 0;
            Parameters = method.GetParameters().Select(p => p.ParameterType.UnderlyingSystemType).ToImmutableArray();
         }

         public string MethodName { get; }

         public int TypeArguments { get; }

         public IImmutableList<Type> Parameters { get; }

         public bool Equals(MethodKey other)
         {
            if (Object.ReferenceEquals(other, null))
               return false;

            return MethodName.Equals(other.MethodName) &&
                   TypeArguments == other.TypeArguments &&
                   Parameters.SequenceEqual(other.Parameters, ParameterInfoComparer.Default);
         }

         public override bool Equals(object obj)
         {
            return obj is MethodKey other && Equals(other);
         }

         public override int GetHashCode()
         {
            return MethodName.GetHashCode() + 11 * Parameters.Count.GetHashCode();
         }

         public static bool operator ==(MethodKey self, MethodKey other)
         {
            return self.Equals(other);
         }

         public static bool operator !=(MethodKey self, MethodKey other)
         {
            return !self.Equals(other);
         }

         private class ParameterInfoComparer : IEqualityComparer<Type>
         {
            public static readonly ParameterInfoComparer Default = new ParameterInfoComparer();

            public bool Equals(Type x, Type y)
            {
               if (x == null)
                  return y == null;

               if (y == null)
                  return false;


               if (x.IsGenericParameter)
               {
                  if (!y.IsGenericParameter)
                     return false;

                  return x.GenericParameterPosition == y.GenericParameterPosition;
               }
               else
               {
                  return x.UnderlyingSystemType.Equals(y.UnderlyingSystemType);
               }
            }

            public int GetHashCode(Type obj)
            {
               if (obj == null)
                  return 0;

               if (obj.IsGenericParameter)
                  return obj.GenericParameterPosition.GetHashCode();

               return obj.UnderlyingSystemType.GetHashCode();
            }
         }
      }
   }
}
