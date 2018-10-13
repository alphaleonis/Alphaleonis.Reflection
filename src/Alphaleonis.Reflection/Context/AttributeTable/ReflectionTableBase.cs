// Copyright (c) Peter Palotas
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;

namespace Alphaleonis.Reflection.Context
{
   /// <summary>Base class containing common functionality for classes representing a <see cref="IReflectionTable"/>.</summary>
   public abstract partial class ReflectionTableBase
   {
      private protected abstract TypeMetadata GetTypeMetadata(Type type);

      /// <summary>
      /// Gets the custom attributes applied to the specified member in this reflection table.
      /// </summary>
      /// <remarks>
      /// This method does not perform any reflection on the member in question. Only those attributes
      /// that have been added to the member in this table will be returned.
      /// </remarks>
      /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
      /// <param name="member">The member to retrieve the custom attributes for.</param>
      /// <returns>
      /// The custom attributes that were added to the specified <paramref name="member"/> in this
      /// reflection table.
      /// </returns>
      public IReadOnlyList<Attribute> GetCustomAttributes(MemberInfo member)
      {
         if (member == null)
            throw new ArgumentNullException(nameof(member));

         switch (member.MemberType)
         {
            case MemberTypes.Event:
            case MemberTypes.Field:
            case MemberTypes.Property:
               var result = GetMemberAttributes(member);

               return result;

            case MemberTypes.Method:
            case MemberTypes.Constructor:
               MethodMetadata methodMetadata;
               if (GetTypeMetadata(member.DeclaringType).MethodAttributes.TryGetValue(new MethodKey(member as MethodBase), out methodMetadata))
               {
                  return methodMetadata.MethodAttributes;
               }
               else
               {
                  return ImmutableList<Attribute>.Empty;
               }

            case MemberTypes.TypeInfo:
            case MemberTypes.NestedType:
               return GetTypeMetadata((Type)member).TypeAttributes;

            case MemberTypes.Custom:
            default:
               return ImmutableList<Attribute>.Empty;
         }
      }

      /// <summary>
      /// Gets the custom attributes applied to the specified parameter in this reflection table.
      /// </summary>
      /// <remarks>
      /// This method does not perform any reflection on the parameter in question. Only those attributes
      /// that have been added to the parameter in this table will be returned.
      /// </remarks>
      /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
      /// <param name="parameter">The parameter to retrieve the custom attributes for.</param>
      /// <returns>
      /// The custom attributes that were added to the specified <paramref name="parameter"/> in this
      /// reflection table.
      /// </returns>
      public IReadOnlyList<Attribute> GetCustomAttributes(ParameterInfo parameter)
      {
         if (parameter == null)
            throw new ArgumentNullException(nameof(parameter));

         var typeMetadata = GetTypeMetadata(parameter.Member.DeclaringType);
         MethodMetadata methodMetadata;
         if (typeMetadata.MethodAttributes.TryGetValue(new MethodKey(parameter.Member as MethodBase), out methodMetadata))
         {
            if (parameter.Position == -1)
               return methodMetadata.ReturnParameterAttributes;
            else
               return methodMetadata.ParameterAttributes[parameter.Position];
         }
         else
         {
            return ImmutableList<Attribute>.Empty;
         }
      }

      private IImmutableList<Attribute> GetMemberAttributes(MemberInfo member)
      {
         IImmutableList<Attribute> attributes;
         if (GetTypeMetadata(member.DeclaringType).MemberAttributes.TryGetValue(new MemberKey(member), out attributes))
         {
            return attributes;
         }
         else
         {
            return ImmutableList<Attribute>.Empty;
         }
      }
   }
}
