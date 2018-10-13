// Copyright (c) Peter Palotas
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Alphaleonis.Reflection.Context
{
   public static class MappedTypedReflectionTableBuilderExtensions
   {
      #region AddPropertyAttributes

      public static IMappedTypedReflectionTableBuilder<T> AddPropertyAttributes<T>(this IMappedTypedReflectionTableBuilder<T> builder, string propertyName, params Attribute[] attributes)
      {
         return AddPropertyAttributes<T>(builder, propertyName, attributes.AsEnumerable());
      }

      public static IMappedTypedReflectionTableBuilder<T> AddPropertyAttributes<T>(this IMappedTypedReflectionTableBuilder<T> builder, string propertyName, IEnumerable<Attribute> attributes)
      {
         builder.Builder.AddPropertyAttributes(builder.TargetType, propertyName, attributes);
         return builder;
      }

      #endregion

      #region AddEventAttributes

      public static IMappedTypedReflectionTableBuilder<T> AddEventAttributes<T>(this IMappedTypedReflectionTableBuilder<T> builder, string eventName, IEnumerable<Attribute> attributes)
      {
         builder.Builder.AddEventAttributes(builder.TargetType, eventName, attributes);
         return builder;
      }

      public static IMappedTypedReflectionTableBuilder<T> AddEventAttributes<T>(this IMappedTypedReflectionTableBuilder<T> builder, string eventName, params Attribute[] attributes)
      {
         return AddEventAttributes<T>(builder, eventName, attributes.AsEnumerable());
      }

      #endregion

      #region AddFieldAttributes

      public static IMappedTypedReflectionTableBuilder<T> AddFieldAttributes<T>(this IMappedTypedReflectionTableBuilder<T> builder, string fieldName, IEnumerable<Attribute> attributes)
      {
         builder.Builder.AddFieldAttributes(builder.TargetType, fieldName, attributes);
         return builder;
      }

      public static IMappedTypedReflectionTableBuilder<T> AddFieldAttributes<T>(this IMappedTypedReflectionTableBuilder<T> builder, string fieldName, params Attribute[] attributes)
      {
         return AddFieldAttributes<T>(builder, fieldName, attributes.AsEnumerable());
      }

      #endregion

      #region AddTypeAttributes

      public static IMappedTypedReflectionTableBuilder<T> AddTypeAttributes<T>(this IMappedTypedReflectionTableBuilder<T> builder, IEnumerable<Attribute> attributes)
      {
         builder.Builder.AddTypeAttributes(builder.TargetType, attributes);
         return builder;
      }

      public static IMappedTypedReflectionTableBuilder<T> AddTypeAttributes<T>(this IMappedTypedReflectionTableBuilder<T> builder, params Attribute[] attributes)
      {
         return AddTypeAttributes<T>(builder, attributes.AsEnumerable());
      }

      #endregion

      #region Parameter Attributes

      //public static ITypedReflectionTableBuilder<T> AddParameterAttributes<T>(this ITypedReflectionTableBuilder<T> builder, Expression<Action<T>> expression)
      //{
      //   builder.Builder.AddParameterAttributes<T>(expression);
      //   return builder;
      //}

      //public static ITypedReflectionTableBuilder<T> AddReturnParameterAttributes<T>(this ITypedReflectionTableBuilder<T> builder, Expression<Action<T>> expression, IEnumerable<Attribute> attributes)
      //{
      //   builder.Builder.AddReturnParameterAttributes(expression, attributes);
      //   return builder;
      //}

      //public static ITypedReflectionTableBuilder<T> AddReturnParameterAttributes<T>(this ITypedReflectionTableBuilder<T> builder, Expression<Action<T>> expression, params Attribute[] attributes)
      //{
      //   return AddReturnParameterAttributes(builder, expression, attributes.AsEnumerable());
      //}

      #endregion

      #region Member Attributes

      public static IMappedTypedReflectionTableBuilder<T> AddMemberAttributes<T>(this IMappedTypedReflectionTableBuilder<T> builder, Expression<Func<T, object>> expression, IEnumerable<Attribute> attributes)
      {
         var member = Reflect.GetMember(expression);
         member = GetClassMember(member, builder.TargetType);
         builder.Builder.AddMemberAttributes(member, attributes);
         return builder;
      }

      public static IMappedTypedReflectionTableBuilder<T> AddMemberAttributes<T>(this IMappedTypedReflectionTableBuilder<T> builder, Expression<Func<T, object>> expression, params Attribute[] attributes)
      {
         return AddMemberAttributes<T>(builder, expression, attributes.AsEnumerable());
      }

      public static IMappedTypedReflectionTableBuilder<T> AddMemberAttributes<T>(this IMappedTypedReflectionTableBuilder<T> builder, Expression<Action<T>> expression, IEnumerable<Attribute> attributes)
      {
         var member = Reflect.GetMember(expression);
         member = GetClassMember(member, builder.TargetType);
         builder.Builder.AddMemberAttributes(member, attributes);
         return builder;
      }

      public static IMappedTypedReflectionTableBuilder<T> AddMemberAttributes<T>(this IMappedTypedReflectionTableBuilder<T> builder, Expression<Action<T>> expression, params Attribute[] attributes)
      {
         return AddMemberAttributes<T>(builder, expression, attributes.AsEnumerable());
      }

      private static MemberInfo GetClassMember(MemberInfo interfaceMember, Type classType)
      {
         if (interfaceMember is PropertyInfo interfaceProperty)
         {
            var map = classType.GetInterfaceMap(interfaceMember.DeclaringType);
            var ifcGetMethod = interfaceProperty.GetGetMethod();
            var ifcSetMethod = interfaceProperty.GetSetMethod();
            MethodInfo targetSetter = null;
            MethodInfo targetGetter = null;

            for (int i = 0; i < map.InterfaceMethods.Length; i++)
            {
               if (ifcGetMethod != null && map.InterfaceMethods[i] == ifcGetMethod)
               {
                  targetGetter = map.TargetMethods[i];
                  break;
               }

               if (ifcSetMethod != null && map.InterfaceMethods[i] == ifcSetMethod)
               {
                  targetSetter = map.TargetMethods[i];
                  break;
               }
            }

            foreach (var targetProperty in classType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
               if (targetGetter != null && targetProperty.GetGetMethod() == targetGetter)
                  return targetProperty;
               else if (targetSetter != null && targetProperty.GetSetMethod() == targetSetter)
                  return targetProperty;
            }

            throw new Exception("Not found");
         }
         if (interfaceMember is EventInfo interfaceEvent)
         {
            var map = classType.GetInterfaceMap(interfaceMember.DeclaringType);
            var ifcAdder = interfaceEvent.GetAddMethod();
            var ifcRemover = interfaceEvent.GetRemoveMethod();
            MethodInfo targetAdder = null;
            MethodInfo targetRemover = null;

            for (int i = 0; i < map.InterfaceMethods.Length; i++)
            {
               if (ifcAdder != null && map.InterfaceMethods[i] == ifcAdder)
               {
                  targetAdder = map.TargetMethods[i];
                  break;
               }

               if (ifcRemover != null && map.InterfaceMethods[i] == ifcRemover)
               {
                  targetRemover = map.TargetMethods[i];
                  break;
               }
            }

            foreach (var targetEvent in classType.GetEvents(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly))
            {
               if (targetRemover != null && targetEvent.GetRemoveMethod() == targetRemover)
                  return targetEvent;
               else if (targetAdder != null && targetEvent.GetAddMethod() == targetAdder)
                  return targetEvent;
            }

            throw new Exception("Not found");
         }
         else if (interfaceMember is MethodInfo interfaceMethod)
         {
            var map = classType.GetInterfaceMap(interfaceMember.DeclaringType);

            for (int i = 0; i < map.InterfaceMethods.Length; i++)
            {
               if (interfaceMethod == map.InterfaceMethods[i])
                  return map.TargetMethods[i];
            }
         }
         else
            throw new NotSupportedException();

         throw new KeyNotFoundException();
      }
      #endregion
   }
}
