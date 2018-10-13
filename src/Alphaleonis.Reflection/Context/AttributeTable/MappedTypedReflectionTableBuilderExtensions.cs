// Copyright (c) Peter Palotas
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Alphaleonis.Reflection.Context
{
   /// <summary>Provides extension methods for <see cref="ITypedReflectionTableBuilder"/>s.</summary>
   public static class MappedTypedReflectionTableBuilderExtensions
   {
      #region AddPropertyAttributes

      /// <summary>
      /// Adds the specified <paramref name="attributes"/> to the property with the given name in the type <typeparamref name="T"/>.
      /// </summary>
      /// <typeparam name="T">The type to act on.</typeparam>
      /// <param name="builder">The reflection table builder.</param>
      /// <param name="propertyName">The name of the property to add attributes to.</param>
      /// <param name="attributes">The attributes to add to the property.</param>
      /// <returns>The <paramref name="builder"/> instance. Useful for chaining multiple calls.</returns>
      public static IMappedTypedReflectionTableBuilder<T> AddPropertyAttributes<T>(this IMappedTypedReflectionTableBuilder<T> builder, string propertyName, params Attribute[] attributes)
      {
         return AddPropertyAttributes<T>(builder, propertyName, attributes.AsEnumerable());
      }

      /// <summary>
      /// Adds the specified <paramref name="attributes"/> to the property with the given name in the type <typeparamref name="T"/>.
      /// </summary>
      /// <typeparam name="T">The type to act on.</typeparam>
      /// <param name="builder">The reflection table builder.</param>
      /// <param name="propertyName">The name of the property to add attributes to.</param>
      /// <param name="attributes">The attributes to add to the property.</param>
      /// <returns>The <paramref name="builder"/> instance. Useful for chaining multiple calls.</returns>
      public static IMappedTypedReflectionTableBuilder<T> AddPropertyAttributes<T>(this IMappedTypedReflectionTableBuilder<T> builder, string propertyName, IEnumerable<Attribute> attributes)
      {
         builder.Builder.AddPropertyAttributes(builder.TargetType, propertyName, attributes);
         return builder;
      }

      #endregion

      #region AddEventAttributes

      /// <summary>
      /// Adds the specified <paramref name="attributes"/> to the event with the given name in the type <typeparamref name="T"/>.
      /// </summary>
      /// <typeparam name="T">The type to act on.</typeparam>
      /// <param name="builder">The reflection table builder.</param>
      /// <param name="eventName">The name of the event to add attributes to.</param>
      /// <param name="attributes">The attributes to add to the event.</param>
      /// <returns>The <paramref name="builder"/> instance. Useful for chaining multiple calls.</returns>
      public static IMappedTypedReflectionTableBuilder<T> AddEventAttributes<T>(this IMappedTypedReflectionTableBuilder<T> builder, string eventName, IEnumerable<Attribute> attributes)
      {
         builder.Builder.AddEventAttributes(builder.TargetType, eventName, attributes);
         return builder;
      }

      /// <summary>
      /// Adds the specified <paramref name="attributes"/> to the event with the given name in the type <typeparamref name="T"/>.
      /// </summary>
      /// <typeparam name="T">The type to act on.</typeparam>
      /// <param name="builder">The reflection table builder.</param>
      /// <param name="eventName">The name of the event to add attributes to.</param>
      /// <param name="attributes">The attributes to add to the event.</param>
      /// <returns>The <paramref name="builder"/> instance. Useful for chaining multiple calls.</returns>
      public static IMappedTypedReflectionTableBuilder<T> AddEventAttributes<T>(this IMappedTypedReflectionTableBuilder<T> builder, string eventName, params Attribute[] attributes)
      {
         return AddEventAttributes<T>(builder, eventName, attributes.AsEnumerable());
      }

      #endregion

      #region AddFieldAttributes

      /// <summary>
      /// Adds the specified <paramref name="attributes"/> to the field with the given name in the type <typeparamref name="T"/>.
      /// </summary>
      /// <typeparam name="T">The type to act on.</typeparam>
      /// <param name="builder">The reflection table builder.</param>
      /// <param name="fieldName">The name of the field to add attributes to.</param>
      /// <param name="attributes">The attributes to add to the field.</param>
      /// <returns>The <paramref name="builder"/> instance. Useful for chaining multiple calls.</returns>

      public static IMappedTypedReflectionTableBuilder<T> AddFieldAttributes<T>(this IMappedTypedReflectionTableBuilder<T> builder, string fieldName, IEnumerable<Attribute> attributes)
      {
         builder.Builder.AddFieldAttributes(builder.TargetType, fieldName, attributes);
         return builder;
      }

      /// <summary>
      /// Adds the specified <paramref name="attributes"/> to the field with the given name in the type <typeparamref name="T"/>.
      /// </summary>
      /// <typeparam name="T">The type to act on.</typeparam>
      /// <param name="builder">The reflection table builder.</param>
      /// <param name="fieldName">The name of the field to add attributes to.</param>
      /// <param name="attributes">The attributes to add to the field.</param>
      /// <returns>The <paramref name="builder"/> instance. Useful for chaining multiple calls.</returns>

      public static IMappedTypedReflectionTableBuilder<T> AddFieldAttributes<T>(this IMappedTypedReflectionTableBuilder<T> builder, string fieldName, params Attribute[] attributes)
      {
         return AddFieldAttributes<T>(builder, fieldName, attributes.AsEnumerable());
      }

      #endregion

      #region AddTypeAttributes

      /// <summary>
      /// Adds the specified <paramref name="attributes"/> to the specified type <typeparamref name="T"/>.
      /// </summary>
      /// <typeparam name="T">The type to add attributes to.</typeparam>
      /// <param name="builder">The reflection table builder.</param>
      /// <param name="attributes">The attributes to add to the type.</param>
      /// <returns>
      /// The <paramref name="builder"/> instance. Useful for chaining multiple calls.
      /// </returns>
      public static IMappedTypedReflectionTableBuilder<T> AddTypeAttributes<T>(this IMappedTypedReflectionTableBuilder<T> builder, IEnumerable<Attribute> attributes)
      {
         builder.Builder.AddTypeAttributes(builder.TargetType, attributes);
         return builder;
      }

      /// <summary>
      /// Adds the specified <paramref name="attributes"/> to the specified type <typeparamref name="T"/>.
      /// </summary>
      /// <typeparam name="T">The type to add attributes to.</typeparam>
      /// <param name="builder">The reflection table builder.</param>
      /// <param name="attributes">The attributes to add to the type.</param>
      /// <returns>
      /// The <paramref name="builder"/> instance. Useful for chaining multiple calls.
      /// </returns>
      public static IMappedTypedReflectionTableBuilder<T> AddTypeAttributes<T>(this IMappedTypedReflectionTableBuilder<T> builder, params Attribute[] attributes)
      {
         return AddTypeAttributes<T>(builder, attributes.AsEnumerable());
      }

      #endregion

      #region Member Attributes

      /// <summary>Adds attributes to the member of a class.</summary>
      /// <typeparam name="T">The type of the object in which the member resides.</typeparam>
      /// <param name="builder">The reflection table builder.</param>
      /// <param name="expression">An expression representing an invocation of the member.</param>
      /// <param name="attributes">The attributes to add to the member.</param>
      /// <returns>
      /// The <paramref name="builder"/> instance. Useful for chaining multiple calls.
      /// </returns>
      public static IMappedTypedReflectionTableBuilder<T> AddMemberAttributes<T>(this IMappedTypedReflectionTableBuilder<T> builder, Expression<Func<T, object>> expression, IEnumerable<Attribute> attributes)
      {
         var member = Reflect.GetMember(expression);
         member = GetClassMember(member, builder.TargetType);
         builder.Builder.AddMemberAttributes(member, attributes);
         return builder;
      }

      /// <summary>Adds attributes to the member of a class.</summary>
      /// <typeparam name="T">The type of the object in which the member resides.</typeparam>
      /// <param name="builder">The reflection table builder.</param>
      /// <param name="expression">An expression representing an invocation of the member.</param>
      /// <param name="attributes">The attributes to add to the member.</param>
      /// <returns>
      /// The <paramref name="builder"/> instance. Useful for chaining multiple calls.
      /// </returns>
      public static IMappedTypedReflectionTableBuilder<T> AddMemberAttributes<T>(this IMappedTypedReflectionTableBuilder<T> builder, Expression<Func<T, object>> expression, params Attribute[] attributes)
      {
         return AddMemberAttributes<T>(builder, expression, attributes.AsEnumerable());
      }

      /// <summary>Adds attributes to the member of a class.</summary>
      /// <typeparam name="T">The type of the object in which the member resides.</typeparam>
      /// <param name="builder">The reflection table builder.</param>
      /// <param name="expression">An expression representing an invocation of the member.</param>
      /// <param name="attributes">The attributes to add to the member.</param>
      /// <returns>
      /// The <paramref name="builder"/> instance. Useful for chaining multiple calls.
      /// </returns>
      public static IMappedTypedReflectionTableBuilder<T> AddMemberAttributes<T>(this IMappedTypedReflectionTableBuilder<T> builder, Expression<Action<T>> expression, IEnumerable<Attribute> attributes)
      {
         var member = Reflect.GetMember(expression);
         member = GetClassMember(member, builder.TargetType);
         builder.Builder.AddMemberAttributes(member, attributes);
         return builder;
      }

      /// <summary>Adds attributes to the member of a class.</summary>
      /// <typeparam name="T">The type of the object in which the member resides.</typeparam>
      /// <param name="builder">The reflection table builder.</param>
      /// <param name="expression">An expression representing an invocation of the member.</param>
      /// <param name="attributes">The attributes to add to the member.</param>
      /// <returns>
      /// The <paramref name="builder"/> instance. Useful for chaining multiple calls.
      /// </returns>
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
