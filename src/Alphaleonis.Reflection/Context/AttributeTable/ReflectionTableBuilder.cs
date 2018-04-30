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
   /// <see cref="TableReflectionContext"/>.
   /// </summary>
   public partial class ReflectionTableBuilder : IReflectionTableBuilder
   {
      // TODO PP (2018-04-30): Document members of this class.
      #region Private Fields

      private readonly ImmutableDictionary<Type, TypeMetadata>.Builder m_metadata;

      #endregion

      #region Constructor

      public ReflectionTableBuilder()
      {
         m_metadata = ImmutableDictionary.CreateBuilder<Type, TypeMetadata>(TypeEqualityComparerIgnoringTypeParameters.Default);
      }

      #endregion

      #region General Methods

      public IReflectionTable CreateTable()
      {
         return new ReflectionTable(m_metadata.ToImmutable());
      }

      #endregion

      #region Add Attributes

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

      private TypeMetadata GetTypeMetadata(Type type)
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
