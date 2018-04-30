using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Alphaleonis.Reflection.Context
{
   public static partial class ReflectionTableBuilderExtensions
   {
      private static MethodInfo s_decorateEnumerableMethodInfo = Reflect.GetMethod(() => Decorate.Parameter<object>(default(IEnumerable<Attribute>))).GetGenericMethodDefinition();
      private static MethodInfo s_decorateParamsMethodInfo = Reflect.GetMethod(() => Decorate.Parameter<object>(default(Attribute[]))).GetGenericMethodDefinition();

      #region Property Attributes

      public static IReflectionTableBuilder AddPropertyAttributes<T>(this IReflectionTableBuilder builder, string propertyName, IEnumerable<Attribute> attributes)
      {
         return AddPropertyAttributes(builder, typeof(T), propertyName, attributes);
      }

      public static IReflectionTableBuilder AddPropertyAttributes<T>(this IReflectionTableBuilder builder, string propertyName, params Attribute[] attributes)
      {
         return AddPropertyAttributes(builder, typeof(T), propertyName, attributes);
      }

      public static IReflectionTableBuilder AddPropertyAttributes(this IReflectionTableBuilder builder, Type type, string propertyName, IEnumerable<Attribute> attributes)
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

         return builder.AddMemberAttributes(property, attributes);
      }

      public static IReflectionTableBuilder AddPropertyAttributes<T>(this IReflectionTableBuilder builder, Type type, string propertyName, params Attribute[] attributes)
      {
         return AddPropertyAttributes(builder, type, propertyName, attributes.AsEnumerable());
      }

      #endregion

      #region Event Attributes

      public static IReflectionTableBuilder AddEventAttributes<T>(this IReflectionTableBuilder builder, string eventName, IEnumerable<Attribute> attributes)
      {
         return AddEventAttributes(builder, typeof(T), eventName, attributes);
      }

      public static IReflectionTableBuilder AddEventAttributes<T>(this IReflectionTableBuilder builder, string eventName, params Attribute[] attributes)
      {
         return AddEventAttributes(builder, typeof(T), eventName, attributes);
      }

      public static IReflectionTableBuilder AddEventAttributes(this IReflectionTableBuilder builder, Type type, string eventName, IEnumerable<Attribute> attributes)
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

         return builder.AddMemberAttributes(eventInfo, attributes);
      }

      public static IReflectionTableBuilder AddEventAttributes(this IReflectionTableBuilder builder, Type type, string eventName, params Attribute[] attributes)
      {
         return AddEventAttributes(builder, type, eventName, attributes.AsEnumerable());
      }

      #endregion

      #region Field Attributes

      public static IReflectionTableBuilder AddFieldAttributes<T>(this IReflectionTableBuilder builder, string fieldName, IEnumerable<Attribute> attributes)
      {
         return AddFieldAttributes(builder, typeof(T), fieldName, attributes);
      }

      public static IReflectionTableBuilder AddFieldAttributes<T>(this IReflectionTableBuilder builder, string fieldName, params Attribute[] attributes)
      {
         return AddFieldAttributes(builder, typeof(T), fieldName, attributes);
      }

      public static IReflectionTableBuilder AddFieldAttributes(this IReflectionTableBuilder builder, Type type, string fieldName, IEnumerable<Attribute> attributes)
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

         return builder.AddMemberAttributes(field, attributes);
      }

      public static IReflectionTableBuilder AddFieldAttributes(this IReflectionTableBuilder builder, Type type, string fieldName, params Attribute[] attributes)
      {
         return AddFieldAttributes(builder, type, fieldName, attributes.AsEnumerable());
      }

      #endregion

      #region Parameter Attributes

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

      private static IReflectionTableBuilder AddParameterAttributes(IReflectionTableBuilder builder, LambdaExpression expression)
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

            if (argCall != null)
            {
               if (argCall.Method.IsGenericMethod && (s_decorateEnumerableMethodInfo.Equals(argCall.Method.GetGenericMethodDefinition())))
               {
                  Expression attrArg = argCall.Arguments[0];
                  var argLamb = Expression.Lambda<Func<IEnumerable<Attribute>>>(attrArg).Compile();
                  var attributes = argLamb();
                  builder.AddParameterAttributes(parameter, attributes);
               }
               else if (s_decorateParamsMethodInfo.Equals(argCall.Method.GetGenericMethodDefinition()))
               {
                  Expression attrArg = argCall.Arguments[0];
                  var argLamb = Expression.Lambda<Func<Attribute[]>>(attrArg).Compile();
                  var attributes = argLamb();
                  builder.AddParameterAttributes(parameter, attributes);
               }
            }
         }

         return builder;
      }

      public static IReflectionTableBuilder AddParameterAttributes(this IReflectionTableBuilder builder, Expression<Action> expression)
      {
         return AddParameterAttributes(builder, (LambdaExpression)expression);
      }

      public static IReflectionTableBuilder AddParameterAttributes<T>(this IReflectionTableBuilder builder, Expression<Action<T>> expression)
      {
         return AddParameterAttributes(builder, (LambdaExpression)expression);
      }

      public static IReflectionTableBuilder AddReturnParameterAttributes(this IReflectionTableBuilder builder, Expression<Action> expression, params Attribute[] attributes)
      {
         return AddReturnParameterAttributes(builder, expression, attributes.AsEnumerable());
      }

      public static IReflectionTableBuilder AddReturnParameterAttributes(this IReflectionTableBuilder builder, Expression<Action> expression, IEnumerable<Attribute> attributes)
      {
         var method = Reflect.GetMethod(expression);
         return builder.AddParameterAttributes(method.ReturnParameter, attributes);
      }

      public static IReflectionTableBuilder AddReturnParameterAttributes<T>(this IReflectionTableBuilder builder, Expression<Action<T>> expression, params Attribute[] attributes)
      {
         return AddReturnParameterAttributes(builder, expression, attributes.AsEnumerable());
      }

      public static IReflectionTableBuilder AddReturnParameterAttributes<T>(this IReflectionTableBuilder builder, Expression<Action<T>> expression, IEnumerable<Attribute> attributes)
      {
         var method = Reflect.GetMethod<T>(expression);
         if (!method.DeclaringType.Equals(typeof(T)))
            throw new ArgumentException($"The type '{typeof(T).FullName}' does not declare a method '{method.Name}'.");

         return builder.AddParameterAttributes(method.ReturnParameter, attributes);
      }

      #endregion

      #region Member Attributes

      public static IReflectionTableBuilder AddMemberAttributes(this IReflectionTableBuilder builder, Expression<Action> expression, params Attribute[] attributes)
      {
         return AddMemberAttributes(builder, expression, attributes.AsEnumerable());
      }

      public static IReflectionTableBuilder AddMemberAttributes(this IReflectionTableBuilder builder, Expression<Action> expression, IEnumerable<Attribute> attributes)
      {
         var member = Reflect.GetMember(expression);
         return builder.AddMemberAttributes(member, attributes);
      }

      public static IReflectionTableBuilder AddMemberAttributes<T>(this IReflectionTableBuilder builder, Expression<Func<T, object>> expression, params Attribute[] attributes)
      {
         return AddMemberAttributes(builder, expression, attributes.AsEnumerable());
      }

      public static IReflectionTableBuilder AddMemberAttributes<T>(this IReflectionTableBuilder builder, Expression<Func<T, object>> expression, IEnumerable<Attribute> attributes)
      {
         var member = Reflect.GetMember<T>(expression);
         if (!member.DeclaringType.Equals(typeof(T)))
            throw new ArgumentException($"The type '{typeof(T).FullName}' does not declare a member '{member.Name}'.");

         return builder.AddMemberAttributes(member, attributes);
      }

      public static IReflectionTableBuilder AddMemberAttributes<T>(this IReflectionTableBuilder builder, Expression<Action<T>> expression, params Attribute[] attributes)
      {
         return AddMemberAttributes(builder, expression, attributes.AsEnumerable());
      }

      public static IReflectionTableBuilder AddMemberAttributes<T>(this IReflectionTableBuilder builder, Expression<Action<T>> expression, IEnumerable<Attribute> attributes)
      {
         var member = Reflect.GetMember<T>(expression);
         if (!member.DeclaringType.Equals(typeof(T)))
            throw new ArgumentException($"The type '{typeof(T).FullName}' does not declare a member '{member.Name}'.");

         return builder.AddMemberAttributes(member, attributes);
      }

      #endregion

      #region Type Attributes

      public static IReflectionTableBuilder AddTypeAttributes(this IReflectionTableBuilder builder, Type type, params Attribute[] attributes)
      {
         return builder.AddMemberAttributes(type, attributes.AsEnumerable());
      }

      public static IReflectionTableBuilder AddTypeAttributes(this IReflectionTableBuilder builder, Type type, IEnumerable<Attribute> attributes)
      {
         return builder.AddMemberAttributes(type, attributes);
      }

      public static IReflectionTableBuilder AddTypeAttributes<T>(this IReflectionTableBuilder builder, params Attribute[] attributes)
      {
         return AddTypeAttributes<T>(builder, attributes.AsEnumerable());
      }

      public static IReflectionTableBuilder AddTypeAttributes<T>(this IReflectionTableBuilder builder, IEnumerable<Attribute> attributes)
      {
         return builder.AddMemberAttributes(typeof(T), attributes);
      }

      #endregion

      #region ForType

      public static IReflectionTableBuilder ForType<T>(this IReflectionTableBuilder builder, Action<ITypedReflectionTableBuilder<T>> action)
      {
         action(new TypedReflectionTableBuilder<T>(builder));
         return builder;
      }

      #endregion
   }
}
