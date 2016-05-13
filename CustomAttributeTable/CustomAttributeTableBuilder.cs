using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CustomAttributeTable
{
   public static class Decorate
   {
      public static T Parameter<T>(IEnumerable<Attribute> attributes)
      {
         return default(T);
      }
   }

   public class CustomAttributeTableBuilder
   {
      #region Private Fields

      private ImmutableDictionary<Type, TypeMetadata>.Builder m_metadata;
      //private static MethodInfo s_decorateArrayMethodInfo = Reflect.GetMethod(() => Decorate.Parameter<object>(default(Attribute[]))).GetGenericMethodDefinition();
      private static MethodInfo s_decorateEnumerableMethodInfo = Reflect.GetMethod(() => Decorate.Parameter<object>(default(IEnumerable<Attribute>))).GetGenericMethodDefinition();

      #endregion

      #region Constructor

      public CustomAttributeTableBuilder()
      {
         m_metadata = ImmutableDictionary.CreateBuilder<Type, TypeMetadata>(TypeEqualityComparer.Default);
      }

      #endregion

      #region General Methods

      public ICustomAttributeTable CreateTable()
      {
         return new CustomAttributeTable(m_metadata.ToImmutable());
      }

      #endregion

      #region Add Type Attributes

      public CustomAttributeTableBuilder AddTypeAttributes<T>(params Attribute[] attributes)
      {
         return AddTypeAttributes<T>(attributes.AsEnumerable());
      }

      public CustomAttributeTableBuilder AddTypeAttributes<T>(IEnumerable<Attribute> attributes)
      {
         return AddMemberAttributes(typeof(T), attributes);
      }

      #endregion

      #region Add Property Attributes

      public CustomAttributeTableBuilder AddPropertyAttributes<T>(string propertyName, IEnumerable<Attribute> attributes)
      {
         return AddPropertyAttributes(typeof(T), propertyName, attributes);
      }

      public CustomAttributeTableBuilder AddPropertyAttributes(Type type, string propertyName, IEnumerable<Attribute> attributes)
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

      public CustomAttributeTableBuilder AddFieldAttributes<T>(string fieldName, IEnumerable<Attribute> attributes)
      {
         return AddFieldAttributes(typeof(T), fieldName, attributes);
      }

      public CustomAttributeTableBuilder AddFieldAttributes(Type type, string fieldName, IEnumerable<Attribute> attributes)
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

      public CustomAttributeTableBuilder AddEventAttributes(Type type, string eventName, IEnumerable<Attribute> attributes)
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

      public CustomAttributeTableBuilder AddEventAttributes<T>(string eventName, IEnumerable<Attribute> attributes)
      {
         return AddEventAttributes(typeof(T), eventName, attributes);
      }

      // TODO PP: Remove commented code.
      //public CustomAttributeTableBuilder AddPropertyAttributes<T>(string propertyName, IEnumerable<Attribute> attributes)
      //{
      //   return AddPropertyAttributes(typeof(T), propertyName, attributes);
      //}

      //public CustomAttributeTableBuilder AddPropertyAttributes(PropertyInfo property, IEnumerable<Attribute> attributes)
      //{
      //   AddMemberAttributes(property, attributes);
      //   return this;
      //}

      //public CustomAttributeTableBuilder AddPropertyAttributes<T>(Expression<Func<T, object>> expression, IEnumerable<Attribute> attributes)
      //{
      //   return AddPropertyAttributes((LambdaExpression)expression, attributes);
      //}

      //public CustomAttributeTableBuilder AddPropertyAttributes<T>(Expression<Action<T>> expression, IEnumerable<Attribute> attributes)
      //{
      //   return AddPropertyAttributes((LambdaExpression)expression, attributes);
      //}

      //private CustomAttributeTableBuilder AddPropertyAttributes(LambdaExpression expression, IEnumerable<Attribute> attributes)
      //{
      //   AddPropertyAttributes(Reflect.GetProperty(expression), attributes);
      //   return this;
      //}

      #endregion

      #region Add Parameter Attributes

      private CustomAttributeTableBuilder AddParameterAttributes(LambdaExpression expression)
      {
         MethodCallExpression methodCallExpression = expression.Body as MethodCallExpression;
         if (methodCallExpression == null)
            throw new ArgumentException("Expression is not a single method call expression.");

         MethodInfo targetMethod = methodCallExpression.Method;
         var parameters = targetMethod.GetParameters();
         for (int i = 0; i < parameters.Length; i++)
         {
            var parameter = parameters[i];
            var argCall = methodCallExpression.Arguments[i] as MethodCallExpression;
            if (argCall != null && argCall.Method.IsGenericMethod &&               
               (s_decorateEnumerableMethodInfo.Equals(argCall.Method.GetGenericMethodDefinition())))
            {
               Expression attrArg = argCall.Arguments[0];
               var argLamb = Expression.Lambda<Func<IEnumerable<Attribute>>>(attrArg).Compile();
               var attributes = argLamb();               
               AddParameterAttributes(parameter, attributes);
            }
         }

         return this;
      }

      public CustomAttributeTableBuilder AddParameterAttributes(Expression<Action> expression)
      {
         return AddParameterAttributes((LambdaExpression)expression);
      }

      public CustomAttributeTableBuilder AddParameterAttributes<T>(Expression<Action<T>> expression)
      {
         return AddParameterAttributes((LambdaExpression)expression);
      }
      
      public CustomAttributeTableBuilder AddParameterAttributes(ParameterInfo parameter, IEnumerable<Attribute> attributes)
      {
         if (parameter == null)
            throw new ArgumentNullException(nameof(parameter), $"{nameof(parameter)} is null.");

         if (attributes == null)
            throw new ArgumentNullException(nameof(attributes), $"{nameof(attributes)} is null.");

         Type type = parameter.Member.DeclaringType;
         m_metadata[type] = GetTypeMetadata(type).AddMethodParameterAttributes(new MethodKey(parameter.Member as MethodBase), parameter.Position, attributes);
         return this;
      }

      #endregion

      //#region Add Method Attributes

      //public CustomAttributeTableBuilder AddMethodAttributes(MethodBase method, IEnumerable<Attribute> attributes)
      //{
      //   if (method == null)
      //      throw new ArgumentNullException(nameof(method), $"{nameof(method)} is null.");

      //   if (attributes == null)
      //      throw new ArgumentNullException(nameof(attributes), $"{nameof(attributes)} is null.");

      //   AddMemberAttributes((MemberInfo)method, attributes);
      //   return this;
      //}

      //#endregion

      #region Add Member Attributes

      public CustomAttributeTableBuilder AddMemberAttributes<T>(Expression<Func<T, object>> expression, IEnumerable<Attribute> attributes)
      {
         var member = Reflect.GetMember<T>(expression);
         if (!member.DeclaringType.Equals(typeof(T)))
            throw new ArgumentException($"The type '{typeof(T).FullName}' does not declare a member '{member.Name}'.");

         return AddMemberAttributes(member, attributes);
      }

      public CustomAttributeTableBuilder AddMemberAttributes<T>(Expression<Action<T>> expression, IEnumerable<Attribute> attributes)
      {
         var member = Reflect.GetMember<T>(expression);
         if (!member.DeclaringType.Equals(typeof(T)))
            throw new ArgumentException($"The type '{typeof(T).FullName}' does not declare a member '{member.Name}'.");

         return AddMemberAttributes(member, attributes);
      }      

      public CustomAttributeTableBuilder AddMemberAttributes(Type type, string memberName, IEnumerable<Attribute> attributes)
      {
         m_metadata[type] = GetTypeMetadata(type).AddMemberAttributes(memberName, attributes);
         return this;
      }

      public CustomAttributeTableBuilder AddMemberAttributes(MemberInfo member, IEnumerable<Attribute> attributes)
      {
         if (member == null)
            throw new ArgumentNullException(nameof(member), $"{nameof(member)} is null.");

         if (attributes == null)
            throw new ArgumentNullException(nameof(attributes), $"{nameof(attributes)} is null.");

         if (member is Type)
         {
            Type type = member as Type;
            m_metadata[type] = GetTypeMetadata(type).AddTypeAttributes(attributes);
         }
         else if (member is MethodBase)
         {
            var method = member as MethodBase;
            m_metadata[method.DeclaringType] = GetTypeMetadata(method.DeclaringType).AddMethodAttributes(new MethodKey(method), attributes);
         }
         else
         {
            AddMemberAttributes(member.DeclaringType, member.Name, attributes);
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

      #region Nested Types

      private class MethodMetadata
      {
         public MethodMetadata(int parameterCount)
         {
            ParameterAttributes = ImmutableList.CreateRange(Enumerable.Range(1, parameterCount).Select(p => (IImmutableList<Attribute>)ImmutableList<Attribute>.Empty)).ToImmutableList();
            ReturnParameterAttributes = ImmutableList<Attribute>.Empty;
            MethodAttributes = ImmutableList<Attribute>.Empty;
         }

         public MethodMetadata(IImmutableList<IImmutableList<Attribute>> parameterAttributes, IImmutableList<Attribute> returnParameterAttributes, IImmutableList<Attribute> methodAttributes)
         {
            ParameterAttributes = parameterAttributes;
            ReturnParameterAttributes = returnParameterAttributes;
            MethodAttributes = methodAttributes;
         }

         public IImmutableList<IImmutableList<Attribute>> ParameterAttributes { get; }
         public IImmutableList<Attribute> ReturnParameterAttributes { get; }
         public IImmutableList<Attribute> MethodAttributes { get; }

         public MethodMetadata AddParameterAttributes(int parameterIndex, IEnumerable<Attribute> attributes)
         {
            return new MethodMetadata(ParameterAttributes.SetItem(parameterIndex, ParameterAttributes[parameterIndex].AddRange(attributes)), ReturnParameterAttributes, MethodAttributes);
         }

         public MethodMetadata AddReturnParameterAttributes(IEnumerable<Attribute> attributes)
         {
            return new MethodMetadata(ParameterAttributes, ReturnParameterAttributes.AddRange(attributes), MethodAttributes);
         }

         public MethodMetadata AddMethodAttributes(IEnumerable<Attribute> methodAttributes)
         {
            return new MethodMetadata(ParameterAttributes, ReturnParameterAttributes, MethodAttributes.AddRange(methodAttributes));
         }
      }

      private class TypeMetadata
      {
         public static readonly TypeMetadata Empty = new TypeMetadata();

         #region Constructors

         private TypeMetadata()
         {
            MemberAttributes = ImmutableDictionary<string, IImmutableList<Attribute>>.Empty;
            MethodAttributes = ImmutableDictionary<MethodKey, MethodMetadata>.Empty;
            TypeAttributes = ImmutableList<Attribute>.Empty;
         }

         private TypeMetadata(IImmutableList<Attribute> typeAttributes, IImmutableDictionary<string, IImmutableList<Attribute>> memberAttributes, IImmutableDictionary<MethodKey, MethodMetadata> methodAttributes)
         {
            MemberAttributes = memberAttributes;
            MethodAttributes = methodAttributes;
            TypeAttributes = typeAttributes;
         }

         #endregion

         #region Properties

         public IImmutableDictionary<string, IImmutableList<Attribute>> MemberAttributes { get; }
         public IImmutableDictionary<MethodKey, MethodMetadata> MethodAttributes { get; }
         public IImmutableList<Attribute> TypeAttributes { get; }

         #endregion

         #region Methods

         public TypeMetadata AddTypeAttributes(IEnumerable<Attribute> attributes)
         {
            return new TypeMetadata(TypeAttributes.AddRange(attributes), MemberAttributes, MethodAttributes);
         }

         public TypeMetadata AddMethodAttributes(MethodKey method, IEnumerable<Attribute> attributes)
         {
            var methodMetadata = GetMethodMetadata(method).AddMethodAttributes(attributes);
            return new TypeMetadata(TypeAttributes, MemberAttributes, MethodAttributes.SetItem(method, methodMetadata));
         }

         public TypeMetadata AddMethodReturnParameterAttributes(MethodKey method, IEnumerable<Attribute> attributes)
         {
            var methodMetadata = GetMethodMetadata(method).AddReturnParameterAttributes(attributes);
            return new TypeMetadata(TypeAttributes, MemberAttributes, MethodAttributes.SetItem(method, methodMetadata));
         }

         public TypeMetadata AddMethodParameterAttributes(MethodKey method, int parameterIndex, IEnumerable<Attribute> attributes)
         {
            var methodMetadata = GetMethodMetadata(method).AddParameterAttributes(parameterIndex, attributes);
            return new TypeMetadata(TypeAttributes, MemberAttributes, MethodAttributes.SetItem(method, methodMetadata));
         }

         public TypeMetadata AddMemberAttributes(string memberName, IEnumerable<Attribute> attributes)
         {
            IImmutableList<Attribute> list;
            if (!MemberAttributes.TryGetValue(memberName, out list))
            {
               list = ImmutableList.CreateRange<Attribute>(attributes);
            }
            else
            {
               list = list.AddRange(attributes);
            }

            return new TypeMetadata(TypeAttributes, MemberAttributes.SetItem(memberName, list), MethodAttributes);
         }

         private MethodMetadata GetMethodMetadata(MethodKey key)
         {
            MethodMetadata metadata;
            if (!MethodAttributes.TryGetValue(key, out metadata))
            {
               metadata = new MethodMetadata(key.Parameters.Count);
            }

            return metadata;
         }
         #endregion
      }

      public class MethodKey : IEquatable<MethodKey>
      {
         public MethodKey(MethodBase method)
         {
            if (method == null)
               throw new ArgumentNullException(nameof(method), $"{nameof(method)} is null.");

            MethodName = method.Name;
            TypeArguments = method.GetGenericArguments().ToImmutableArray();
            Parameters = method.GetParameters().Select(p => p.ParameterType).ToImmutableArray();
         }

         public string MethodName { get; }

         public IImmutableList<Type> TypeArguments { get; }

         public IImmutableList<Type> Parameters { get; }

         public bool Equals(MethodKey other)
         {
            if (Object.ReferenceEquals(other, null))
               return false;

            return MethodName.Equals(other.MethodName) &&
                   TypeArguments.SequenceEqual(other.TypeArguments) &&
                   Parameters.SequenceEqual(other.Parameters);
         }

         public override bool Equals(object obj)
         {
            return Equals(obj as MethodKey);
         }

         public override int GetHashCode()
         {
            return MethodName.GetHashCode() + 11 * Parameters.Count.GetHashCode();
         }
      }

      

      private class CustomAttributeTable : ICustomAttributeTable
      {
         private ImmutableDictionary<Type, TypeMetadata> m_metadata;

         public CustomAttributeTable(ImmutableDictionary<Type, TypeMetadata> metadata)
         {
            m_metadata = metadata;
         }

         public IEnumerable<Attribute> GetCustomAttributes(Type type)
         {
            return GetTypeMetadata(type).TypeAttributes;
         }

         public IEnumerable<Attribute> GetCustomAttributes(MemberInfo member)
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
                  return GetCustomAttributes((Type)member);

               case MemberTypes.Custom:
               default:
                  return ImmutableList<Attribute>.Empty;
            }
         }

         public IEnumerable<Attribute> GetCustomAttributes(ParameterInfo parameterInfo)
         {
            if (parameterInfo == null)
               throw new ArgumentNullException(nameof(parameterInfo));

            var typeMetadata = GetTypeMetadata(parameterInfo.Member.DeclaringType);
            MethodMetadata methodMetadata;
            if (typeMetadata.MethodAttributes.TryGetValue(new MethodKey(parameterInfo.Member as MethodBase), out methodMetadata))
            {
               return methodMetadata.ParameterAttributes[parameterInfo.Position];
            }
            else
            {
               return ImmutableList<Attribute>.Empty;
            }
         }

         private TypeMetadata GetTypeMetadata(Type type)
         {
            TypeMetadata metadata;
            if (m_metadata.TryGetValue(type, out metadata))
            {
               return metadata;
            }

            return TypeMetadata.Empty;
         }

         private IEnumerable<Attribute> GetMemberAttributes(MemberInfo member)
         {
            IImmutableList<Attribute> attributes;
            if (GetTypeMetadata(member.DeclaringType).MemberAttributes.TryGetValue(member.Name, out attributes))
            {
               return attributes;
            }
            else
            {
               return ImmutableList<Attribute>.Empty;
            }
         }
      }

      #endregion
   }
}
