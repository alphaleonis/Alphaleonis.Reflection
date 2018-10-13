// Copyright (c) Peter Palotas
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Alphaleonis.Reflection.Context
{
   /// <summary>
   /// A class used to build the table to be used for adding reflection information
   /// in a <see cref="TableReflectionContext"/>.
   /// </summary>
   public partial class ReflectionTableBuilder : IReflectionTableBuilder
   {      
      #region Private Fields

      private readonly ImmutableDictionary<Type, TypeMetadata>.Builder m_metadata;

      #endregion

      #region Constructor

      /// <summary>Default constructor.</summary>
      public ReflectionTableBuilder()
      {
         m_metadata = ImmutableDictionary.CreateBuilder<Type, TypeMetadata>(TypeEqualityComparerIgnoringTypeParameters.Default);
      }

      #endregion

      #region General Methods

      /// <summary>Creates an immutable <see cref="IReflectionTable"/> based on the contents of this builder.</summary>
      /// <returns>A new instance of an immutable <see cref="IReflectionTable"/>.</returns>
      public IReflectionTable CreateTable()
      {
         return new ReflectionTable(m_metadata.ToImmutable());
      }

      #endregion

      #region Add Attributes

      /// <summary>Adds the specified <paramref name="attributes"/> to the specified <paramref name="parameter"/> in this reflection table.</summary>
      /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
      /// <param name="parameter">The parameter to add one or more attributes to.</param>
      /// <param name="attributes">The attributes to add to the <paramref name="parameter"/>.</param>
      /// <returns>This instance of the <see cref="ReflectionTableBuilder"/>, allowing chaining calls to multiple methods.</returns>
      public IReflectionTableBuilder AddParameterAttributes(ParameterInfo parameter, IEnumerable<Attribute> attributes)
      {
         if (parameter == null)
            throw new ArgumentNullException(nameof(parameter), $"{nameof(parameter)} is null.");

         if (attributes == null)
            throw new ArgumentNullException(nameof(attributes), $"{nameof(attributes)} is null.");

         Type type = parameter.Member.DeclaringType;
         m_metadata[type] = GetTypeMetadata(type).AddMethodParameterAttributes(new MethodKey(parameter.Member as MethodBase), parameter.Position, attributes);
         return this;
      }

      /// <summary>Adds the specified <paramref name="attributes"/> to the specified <paramref name="member"/> in this reflection table.</summary>
      /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
      /// <param name="member">The parameter to add one or more attributes to.</param>
      /// <param name="attributes">The attributes to add to the <paramref name="member"/>.</param>
      /// <returns>This instance of the <see cref="ReflectionTableBuilder"/>, allowing chaining calls to multiple methods.</returns>
      public IReflectionTableBuilder AddMemberAttributes(MemberInfo member, IEnumerable<Attribute> attributes)
      {         
         if (member == null)
            throw new ArgumentNullException(nameof(member), $"{nameof(member)} is null.");

         if (attributes == null)
            throw new ArgumentNullException(nameof(attributes), $"{nameof(attributes)} is null.");


         switch (member)
         {
            case Type type:
               m_metadata[type] = GetTypeMetadata(type).AddTypeAttributes(attributes);
               break;

            case MethodBase method:
               m_metadata[method.DeclaringType] = GetTypeMetadata(method.DeclaringType).AddMethodAttributes(new MethodKey(method), attributes);
               break;

            default:
               var declaringType = member.DeclaringType;
               m_metadata[declaringType] = GetTypeMetadata(declaringType).AddMemberAttributes(new MemberKey(member.MemberType, member.Name), attributes);
               ReflectionTableBuilder result = this;
               break;
         }

         return this;
      }

      #endregion

      #region Private Methods

      private protected override TypeMetadata GetTypeMetadata(Type type)
      {
         TypeMetadata metadata;
         if (!m_metadata.TryGetValue(type, out metadata))
         {
            metadata = TypeMetadata.Empty;
         }
         return metadata;
      }

      #endregion
   }
}
