using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Alphaleonis.Reflection
{
   public static class ICustomAttributeTableBuilderExtensions
   {
      private static MethodInfo s_decorateEnumerableMethodInfo = Reflect.GetMethod(() => Decorate.Parameter<object>(default(IEnumerable<Attribute>))).GetGenericMethodDefinition();

      #region Property Attributes

      public static ICustomAttributeTableBuilder AddPropertyAttributes<T>(this ICustomAttributeTableBuilder self, string propertyName, IEnumerable<Attribute> attributes)
      {
         return self.AddPropertyAttributes(typeof(T), propertyName, attributes);
      }

      public static ICustomAttributeTableBuilder AddPropertyAttributes<T>(this ICustomAttributeTableBuilder self, string propertyName, params Attribute[] attributes)
      {
         return self.AddPropertyAttributes(typeof(T), propertyName, attributes);
      }

      #endregion

      #region Event Attributes

      public static ICustomAttributeTableBuilder AddEventAttributes<T>(this ICustomAttributeTableBuilder self, string eventName, IEnumerable<Attribute> attributes)
      {
         return self.AddEventAttributes(typeof(T), eventName, attributes);
      }

      public static ICustomAttributeTableBuilder AddEventAttributes<T>(this ICustomAttributeTableBuilder self, string eventName, params Attribute[] attributes)
      {
         return self.AddEventAttributes(typeof(T), eventName, attributes);
      }

      #endregion

      #region Field Attributes

      public static ICustomAttributeTableBuilder AddFieldAttributes<T>(this ICustomAttributeTableBuilder self, string fieldName, IEnumerable<Attribute> attributes)
      {
         return self.AddFieldAttributes(typeof(T), fieldName, attributes);
      }

      public static ICustomAttributeTableBuilder AddFieldAttributes<T>(this ICustomAttributeTableBuilder self, string fieldName, params Attribute[] attributes)
      {
         return self.AddFieldAttributes(typeof(T), fieldName, attributes);
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

      private static ICustomAttributeTableBuilder AddParameterAttributes(ICustomAttributeTableBuilder self, LambdaExpression expression)
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
               self.AddParameterAttributes(parameter, attributes);
            }
         }

         return self;
      }

      public static ICustomAttributeTableBuilder AddParameterAttributes(this ICustomAttributeTableBuilder self, Expression<Action> expression)
      {
         return AddParameterAttributes(self, (LambdaExpression)expression);
      }

      public static ICustomAttributeTableBuilder AddParameterAttributes<T>(this ICustomAttributeTableBuilder self, Expression<Action<T>> expression)
      {
         return AddParameterAttributes(self, (LambdaExpression)expression);
      }

      public static ICustomAttributeTableBuilder AddReturnParameterAttributes(this ICustomAttributeTableBuilder self, Expression<Action> expression, params Attribute[] attributes)
      {
         var method = Reflect.GetMethod(expression);

         return self.AddParameterAttributes(method.ReturnParameter, attributes);
      }

      public static ICustomAttributeTableBuilder AddReturnParameterAttributes<T>(this ICustomAttributeTableBuilder self, Expression<Action<T>> expression, params Attribute[] attributes)
      {
         var method = Reflect.GetMethod<T>(expression);
         if (!method.DeclaringType.Equals(typeof(T)))
            throw new ArgumentException($"The type '{typeof(T).FullName}' does not declare a method '{method.Name}'.");

         return self.AddParameterAttributes(method.ReturnParameter, attributes);
      }

      #endregion

      #region Member Attributes

      public static ICustomAttributeTableBuilder AddMemberAttributes(this ICustomAttributeTableBuilder self, Expression<Action> expression, IEnumerable<Attribute> attributes)
      {
         var member = Reflect.GetMember(expression);
         return self.AddMemberAttributes(member, attributes);
      }

      public static ICustomAttributeTableBuilder AddMemberAttributes<T>(this ICustomAttributeTableBuilder self, Expression<Func<T, object>> expression, IEnumerable<Attribute> attributes)
      {
         var member = Reflect.GetMember<T>(expression);
         if (!member.DeclaringType.Equals(typeof(T)))
            throw new ArgumentException($"The type '{typeof(T).FullName}' does not declare a member '{member.Name}'.");

         return self.AddMemberAttributes(member, attributes);
      }

      public static ICustomAttributeTableBuilder AddMemberAttributes<T>(this ICustomAttributeTableBuilder self, Expression<Action<T>> expression, IEnumerable<Attribute> attributes)
      {
         var member = Reflect.GetMember<T>(expression);
         if (!member.DeclaringType.Equals(typeof(T)))
            throw new ArgumentException($"The type '{typeof(T).FullName}' does not declare a member '{member.Name}'.");

         return self.AddMemberAttributes(member, attributes);
      }

      #endregion

      #region Type Attributes

      public static ICustomAttributeTableBuilder AddTypeAttributes(this ICustomAttributeTableBuilder self, Type type, params Attribute[] attributes)
      {
         return self.AddMemberAttributes(type, attributes);
      }

      public static ICustomAttributeTableBuilder AddTypeAttributes<T>(this ICustomAttributeTableBuilder self, params Attribute[] attributes)
      {
         return AddTypeAttributes<T>(self, attributes.AsEnumerable());
      }

      public static ICustomAttributeTableBuilder AddTypeAttributes<T>(this ICustomAttributeTableBuilder self, IEnumerable<Attribute> attributes)
      {
         return self.AddMemberAttributes(typeof(T), attributes);
      }

      #endregion
   }
}
