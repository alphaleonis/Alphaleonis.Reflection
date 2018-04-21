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

      #region Private Fields

      private ImmutableDictionary<Type, TypeMetadata>.Builder m_metadata;
      //private static MethodInfo s_decorateArrayMethodInfo = Reflect.GetMethod(() => Decorate.Parameter<object>(default(Attribute[]))).GetGenericMethodDefinition();
      private static MethodInfo s_decorateEnumerableMethodInfo = Reflect.GetMethod(() => Decorate.Parameter<object>(default(IEnumerable<Attribute>))).GetGenericMethodDefinition();

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

      #region Add Type Attributes

      public CustomAttributeTableBuilder AddTypeAttributes(Type type, params Attribute[] attributes)
      {
         return AddMemberAttributes(type, attributes);
      }

      public CustomAttributeTableBuilder AddTypeAttributes(Type type, IEnumerable<Attribute> attributes)
      {
         return AddMemberAttributes(type, attributes);
      }

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

      #endregion

      #region AddEventAttributes

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

      #endregion

      #region AddFieldAttributes

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

      #endregion  

      #region Add Parameter Attributes

      private static MethodCallExpression GetMethodCallExpression(Expression expression)
      {
         MethodCallExpression methodCallExpression = expression as MethodCallExpression;
         if (methodCallExpression != null)
         {
            return methodCallExpression;
         }

         UnaryExpression unaryExpression = expression as UnaryExpression;
         if (unaryExpression != null)
         {
            return GetMethodCallExpression(unaryExpression.Operand);
         }

         return null;
      }

      private CustomAttributeTableBuilder AddParameterAttributes(LambdaExpression expression)
      {
         MethodCallExpression methodCallExpression = expression.Body as MethodCallExpression;
         if (methodCallExpression == null)
            throw new ArgumentException("Expression is not a single method call expression.");

         MethodInfo targetMethod = (MethodInfo)Reflect.GetMemberInternal(methodCallExpression, true);
         var parameters = targetMethod.GetParameters();
         for (int i = 0; i < parameters.Length; i++)
         {
            var parameter = parameters[i];
            var argCall = GetMethodCallExpression(methodCallExpression.Arguments[i]);            

            if (argCall != null && argCall.Method.IsGenericMethod && (s_decorateEnumerableMethodInfo.Equals(argCall.Method.GetGenericMethodDefinition())))
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

      public CustomAttributeTableBuilder AddReturnParameterAttributes(Expression<Action> expression, params Attribute[] attributes)
      {
         return AddReturnParameterAttributes((LambdaExpression)expression, attributes);
      }

      public CustomAttributeTableBuilder AddReturnParameterAttributes<T>(Expression<Action<T>> expression, params Attribute[] attributes)
      {
         return AddReturnParameterAttributes((LambdaExpression)expression, attributes);
      }

      private CustomAttributeTableBuilder AddReturnParameterAttributes(LambdaExpression expression, IEnumerable<Attribute> attributes)
      {
         // TODO PP: Doesn't work properly probably... need to check for declaredonly!
         var member = Reflect.GetMethod(expression);
         var method = member as MethodInfo;
         m_metadata[method.DeclaringType] = GetTypeMetadata(method.DeclaringType).AddMethodReturnParameterAttributes(new MethodKey(method), attributes);
         return this;
      }

      #endregion      

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

      public CustomAttributeTableBuilder AddMemberAttributes(MemberInfo member, IEnumerable<Attribute> attributes)
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

   // TODO PP: Change AddMemberAttribute to specific methods instead, it seems that it may be needed. Why!?
}
