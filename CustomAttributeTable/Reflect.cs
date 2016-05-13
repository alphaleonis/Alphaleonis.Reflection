using CustomAttributeTableTests;
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
      public static PropertyInfo GetProperty<T>(Expression<Action<T>> expression)
      {
         return GetProperty((LambdaExpression)expression);
      }

      public static MemberInfo GetMember<T>(Expression<Action<T>> expression)
      {
         return GetMember((LambdaExpression)expression);
      }

      public static MemberInfo GetMember<T>(Expression<Func<T, object>> expression)
      {
         return GetMember((LambdaExpression)expression);
      }

      public static MemberInfo GetMember(LambdaExpression expression)
      {
         MemberInfo member = null;
         Type ownerType;

         MethodCallExpression methodCallExpr = expression.Body as MethodCallExpression;
         if (methodCallExpr != null)
         {
            member = methodCallExpr.Method;
            ownerType = methodCallExpr.Object == null ? methodCallExpr.Method.DeclaringType : methodCallExpr.Object.Type;
         }
         else
         {
            MemberExpression body = expression.Body as MemberExpression;
            if (body == null)
            {
               UnaryExpression ubody = expression.Body as UnaryExpression;
               if (ubody == null)
                  throw new ArgumentException($"Expression '{expression}' does not refer to a property, field or event.");

               body = ubody.Operand as MemberExpression;
               if (body == null)
                  throw new ArgumentException($"Expression '{expression}' does not refer to a property, field or event.");

               member = body.Member;
            }
            else
            {
               member = body.Member;
            }

            ownerType = body.Expression.Type;
         }

         if (member is PropertyInfo)
         {
            if (!member.DeclaringType.Equals(ownerType))
            {
               // We are accessing the property in one type, but the PropertyInfo we got is from a base type. 
               // So we need to check if this property exists on the derived type as well.
               member = ownerType.GetProperties(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                  .FirstOrDefault(p => p.GetBaseDefinition()?.Equals(member) == true) ?? member;
            }
         }
         else if (member is MethodInfo)
         {
            if (!member.DeclaringType.Equals(ownerType))
            {
               // We are accessing the property in one type, but the PropertyInfo we got is from a base type. 
               // So we need to check if this property exists on the derived type as well.
               member = ownerType.GetMethods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                  .FirstOrDefault(p => p.GetBaseDefinition()?.Equals(member) == true) ?? member;
            }
         }

         return member;
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

         PropertyInfo property = member as PropertyInfo;
         if (property == null)
            throw new ArgumentException($"Expression '{expression}' does not refer to a property.");

         if (!property.DeclaringType.Equals(body.Expression.Type))
         {
            // We are accessing the property in one type, but the PropertyInfo we got is from a base type. 
            // So we need to check if this property exists on the derived type as well.
            property = body.Expression.Type.GetProperties(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).FirstOrDefault(p => p.GetBaseDefinition()?.Equals(property) == true) ?? property;
         }

         return property;
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