using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

namespace Alphaleonis.Reflection
{
   public interface ICustomAttributeTableBuilder
   {
      ICustomAttributeTableBuilder AddEventAttributes(Type type, string eventName, IEnumerable<Attribute> attributes);
      ICustomAttributeTableBuilder AddFieldAttributes(Type type, string fieldName, IEnumerable<Attribute> attributes);
      ICustomAttributeTableBuilder AddMemberAttributes(MemberInfo member, IEnumerable<Attribute> attributes);
      ICustomAttributeTableBuilder AddParameterAttributes(ParameterInfo parameter, IEnumerable<Attribute> attributes);
      ICustomAttributeTableBuilder AddPropertyAttributes(Type type, string propertyName, IEnumerable<Attribute> attributes);
      ICustomAttributeTableBuilder AddTypeAttributes(Type type, IEnumerable<Attribute> attributes);
      ICustomAttributeTable CreateTable();
   }

   public partial class CustomAttributeTableBuilder : ICustomAttributeTableBuilder
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

      #region Private Fields

      private readonly ImmutableDictionary<Type, TypeMetadata>.Builder m_metadata;

      #endregion

      #region Constructor

      public CustomAttributeTableBuilder()
      {
         m_metadata = ImmutableDictionary.CreateBuilder<Type, TypeMetadata>(TypeEqualityComparerIgnoringTypeParameters.Default);
      }

      #endregion

      #region General Methods

      public ICustomAttributeTable CreateTable()
      {
         return new CustomAttributeTable(m_metadata.ToImmutable());
      }

      #endregion

      #region Add Attributes

      public ICustomAttributeTableBuilder AddTypeAttributes(Type type, IEnumerable<Attribute> attributes)
      {
         return AddMemberAttributes(type, attributes);
      }

      public ICustomAttributeTableBuilder AddPropertyAttributes(Type type, string propertyName, IEnumerable<Attribute> attributes)
      {
         if (type == null)
            throw new ArgumentNullException(nameof(type), $"{nameof(type)} is null.");

         if (string.IsNullOrEmpty(propertyName))
            throw new ArgumentException($"{nameof(propertyName)} is null or empty.", nameof(propertyName));

         if (attributes == null)
            throw new ArgumentNullException(nameof(attributes), $"{nameof(attributes)} is null.");

         PropertyInfo property = type.GetTypeInfo().GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);
         if (property == null)
            throw new ArgumentException($"The type {type.FullName} does not declare a property named \"{propertyName}\".");

         AddMemberAttributes(property, attributes);
         return this;
      }

      public ICustomAttributeTableBuilder AddEventAttributes(Type type, string eventName, IEnumerable<Attribute> attributes)
      {
         if (type == null)
            throw new ArgumentNullException(nameof(type), $"{nameof(type)} is null.");

         if (string.IsNullOrEmpty(eventName))
            throw new ArgumentException($"{nameof(eventName)} is null or empty.", nameof(eventName));

         if (attributes == null)
            throw new ArgumentNullException(nameof(attributes), $"{nameof(attributes)} is null.");

         EventInfo eventInfo = type.GetTypeInfo().GetEvent(eventName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);
         if (eventInfo == null)
            throw new ArgumentException($"The type {type.FullName} does not declare an event named \"{eventName}\".");

         AddMemberAttributes(eventInfo, attributes);
         return this;
      }

      public ICustomAttributeTableBuilder AddFieldAttributes(Type type, string fieldName, IEnumerable<Attribute> attributes)
      {
         if (type == null)
            throw new ArgumentNullException(nameof(type), $"{nameof(type)} is null.");

         if (string.IsNullOrEmpty(fieldName))
            throw new ArgumentException($"{nameof(fieldName)} is null or empty.", nameof(fieldName));

         if (attributes == null)
            throw new ArgumentNullException(nameof(attributes), $"{nameof(attributes)} is null.");

         FieldInfo field = type.GetTypeInfo().GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);
         if (field == null)
            throw new ArgumentException($"The type {type.FullName} does not declare a field named \"{fieldName}\".");

         AddMemberAttributes(field, attributes);
         return this;
      }

      public ICustomAttributeTableBuilder AddParameterAttributes(ParameterInfo parameter, IEnumerable<Attribute> attributes)
      {
         if (parameter == null)
            throw new ArgumentNullException(nameof(parameter), $"{nameof(parameter)} is null.");

         if (attributes == null)
            throw new ArgumentNullException(nameof(attributes), $"{nameof(attributes)} is null.");

         Type type = parameter.Member.DeclaringType;
         m_metadata[type] = GetTypeMetadata(type).AddMethodParameterAttributes(new MethodKey(parameter.Member as MethodBase), parameter.Position, attributes);
         return this;
      }

      public ICustomAttributeTableBuilder AddMemberAttributes(MemberInfo member, IEnumerable<Attribute> attributes)
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
               CustomAttributeTableBuilder result = this;
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

   // TODO PP: Add a simplified builder that is specific to a type, eg. builder.ForType<MyType>().AddMemberAttributes(c => c.MyProperty);
   //                                                                                                                ^ Note: No generic argument here!
}
