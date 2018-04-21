using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Text;
using System.Threading.Tasks;

namespace Alphaleonis.Reflection
{
   // TODO PP (2018-04-21): Document
   // TODO PP: Clean up this class. It probably needs a bit more checks and stuff.
   // TODO PP: Perhaps we should have a "DeclaredOnly" flag to all these methods?
   public static class Reflect
   {
      public static PropertyInfo GetProperty<T, U>(Expression<Func<U, T>> expression)
      {
         return GetProperty((LambdaExpression)expression);
      }

      public static PropertyInfo GetProperty<T>(Expression<Func<T>> expression)
      {
         return GetProperty((LambdaExpression)expression);
      }

      public static PropertyInfo GetProperty<T>(Expression<Action<T>> expression)
      {
         return GetProperty((LambdaExpression)expression);
      }

      internal static PropertyInfo GetProperty(LambdaExpression expression)
      {
         var memberInfo = GetMemberInternal(expression.Body, false) as PropertyInfo;
         if (memberInfo == null)
            throw new ArgumentException($"The expression {expression} does not reference a property.");

         return memberInfo;
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
         return GetMemberInternal(expression.Body, false);
      }

      internal static MemberInfo GetMemberInternal(Expression expression, bool declaredOnly)
      {
         MemberInfo member = null;
         Type ownerType;

         MethodCallExpression methodCallExpr = expression as MethodCallExpression;
         if (methodCallExpr != null)
         {
            member = methodCallExpr.Method;
            ownerType = methodCallExpr.Object == null ? methodCallExpr.Method.DeclaringType : methodCallExpr.Object.Type;
         }
         else
         {
            MemberExpression body = expression as MemberExpression;
            if (body == null)
            {
               UnaryExpression ubody = expression as UnaryExpression;
               if (ubody == null)
                  throw new ArgumentException($"Expression '{expression}' does not refer to a property, field or event.");

               return GetMemberInternal(ubody.Operand, declaredOnly);
            }
            else
            {
               member = body.Member;
            }

            ownerType = body.Expression?.Type ?? member.DeclaringType;
         }

         if (member is PropertyInfo)
         {
            if (!member.DeclaringType.Equals(ownerType))
            {
               // We are accessing the property in one type, but the PropertyInfo we got is from a base type. 
               // So we need to check if this property exists on the derived type as well.
               var declaredMember = ownerType.GetProperties(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | (declaredOnly ? BindingFlags.DeclaredOnly : 0))
                  .FirstOrDefault(p => p.GetBaseDefinition()?.Equals(member) == true);

               if (declaredMember == null && declaredOnly)
               {
                  throw new ArgumentException($"The property {member.Name} is not declared on type {ownerType}, it's base declaration is on {member.DeclaringType}.");
               }
               else if (declaredMember != null)
               {
                  member = declaredMember;
               }
            }
         }
         else if (member is MethodInfo)
         {
            if (!member.DeclaringType.Equals(ownerType))
            {
               // We are accessing the property in one type, but the PropertyInfo we got is from a base type. 
               // So we need to check if this property exists on the derived type as well.
               var declaredMember = ownerType.GetMethods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | (declaredOnly ? BindingFlags.DeclaredOnly : 0))
                  .FirstOrDefault(p => p.GetBaseDefinition()?.Equals(member.GetBaseDefinition()) == true);

               if (declaredMember == null && declaredOnly)
               {
                  throw new ArgumentException($"The method {member} is not declared on type {ownerType}, it's base declaration is on {member.DeclaringType}.");
               }
               else if (declaredMember != null)
               {
                  member = declaredMember;
               }
            }
         }

         return member;
      }

      public static MethodInfo GetMethod(Expression<Action> expression)
      {
         return GetMethod((LambdaExpression)expression);
      }

      public static MethodInfo GetMethod<T>(Expression<Action<T>> expression)
      {
         return GetMethod((LambdaExpression)expression);
      }

      public static MethodInfo GetMethod(LambdaExpression expression)
      {
         MethodCallExpression methodCallExpression = expression.Body as MethodCallExpression ?? throw new ArgumentException("Invalid Expression. Expression should consist of a Method call only.");
         if (methodCallExpression.Object != null && !methodCallExpression.Method.DeclaringType.Equals(methodCallExpression.Object.Type))
         {
            var bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            var result = methodCallExpression.Object.Type.GetMethods(bindingFlags).FirstOrDefault(m => m.GetBaseDefinition().Equals(methodCallExpression.Method.GetBaseDefinition()));

            return result;
         }

         return methodCallExpression.Method;
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