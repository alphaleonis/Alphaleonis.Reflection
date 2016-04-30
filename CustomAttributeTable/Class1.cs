using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CustomAttributeTable
{
   public static class Reflect
   {
      public static PropertyInfo GetProperty<T>(Expression<Func<T>> expression)
      {
         return GetProperty((LambdaExpression)expression);
      }

      public static PropertyInfo GetProperty(LambdaExpression expression)
      {
         MemberInfo member = null;

         MemberExpression body = expression.Body as MemberExpression;
         if (body == null)
         {
            UnaryExpression ubody = expression.Body as UnaryExpression;
            if (ubody == null)
               throw new ArgumentException($"Expression '{expression}' does not refer to a property.");

            body = ubody.Operand as MemberExpression;
            if (body == null)
               throw new ArgumentException($"Expression '{expression}' does not refer to a property.", "propertyLambda");

            member = body.Member;
         }
         else
         {
            member = body.Member;
         }

         PropertyInfo result = member as PropertyInfo;
         if (result == null)
            throw new ArgumentException($"Expression '{expression}' does not refer to a property.");

         return result;
      }

      public static MethodInfo GetMethod(Expression<Action> expression)
      {
         return GetMethod((LambdaExpression)expression);
      }

      public static MethodInfo GetMethod<T>(Expression<Func<T>> expression)
      {
         return GetMethod((LambdaExpression)expression);
      }

      public static MethodInfo GetMethod(LambdaExpression expression)
      {
         MethodCallExpression outermostExpression = expression.Body as MethodCallExpression;

         if (outermostExpression == null)
            throw new ArgumentException("Invalid Expression. Expression should consist of a Method call only.");

         return outermostExpression.Method;
      }
   }

   public static class Reflect<TSource>
   {
      public static PropertyInfo GetProperty<T>(Expression<Func<TSource, T>> expression)
      {
         return Reflect.GetProperty((LambdaExpression)expression);
      }


      public static MethodInfo GetMethod(Expression<Action> expression)
      {
         return Reflect.GetMethod((LambdaExpression)expression);
      }

      public static MethodInfo GetMethod(Expression<Action<TSource>> expression)
      {
         return Reflect.GetMethod((LambdaExpression)expression);
      }

      public static MethodInfo GetMethod<TResult>(Expression<Func<TResult>> expression)
      {
         return Reflect.GetMethod((LambdaExpression)expression);
      }

      public static MethodInfo GetMethod<TResult>(Expression<Func<TSource, TResult>> expression)
      {
         return Reflect.GetMethod((LambdaExpression)expression);
      }

   }
}