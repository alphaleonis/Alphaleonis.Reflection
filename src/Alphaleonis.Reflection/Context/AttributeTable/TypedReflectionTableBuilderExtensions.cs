// Copyright (c) Peter Palotas
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Alphaleonis.Reflection.Context
{
   /// <summary>Provides extension methods for an <see cref="ITypedReflectionTableBuilder"/>.</summary>
   public static class TypedReflectionTableBuilderExtensions
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
      public static ITypedReflectionTableBuilder<T> AddPropertyAttributes<T>(this ITypedReflectionTableBuilder<T> builder, string propertyName, params Attribute[] attributes)
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
      public static ITypedReflectionTableBuilder<T> AddPropertyAttributes<T>(this ITypedReflectionTableBuilder<T> builder, string propertyName, IEnumerable<Attribute> attributes)
      {
         builder.Builder.AddPropertyAttributes<T>(propertyName, attributes);
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
      public static ITypedReflectionTableBuilder<T> AddEventAttributes<T>(this ITypedReflectionTableBuilder<T> builder, string eventName, IEnumerable<Attribute> attributes)
      {
         builder.Builder.AddEventAttributes<T>(eventName, attributes);
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
      public static ITypedReflectionTableBuilder<T> AddEventAttributes<T>(this ITypedReflectionTableBuilder<T> builder, string eventName, params Attribute[] attributes)
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

      public static ITypedReflectionTableBuilder<T> AddFieldAttributes<T>(this ITypedReflectionTableBuilder<T> builder, string fieldName, IEnumerable<Attribute> attributes)
      {
         builder.Builder.AddFieldAttributes<T>(fieldName, attributes);
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
      public static ITypedReflectionTableBuilder<T> AddFieldAttributes<T>(this ITypedReflectionTableBuilder<T> builder, string fieldName, params Attribute[] attributes)
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
      public static ITypedReflectionTableBuilder<T> AddTypeAttributes<T>(this ITypedReflectionTableBuilder<T> builder, IEnumerable<Attribute> attributes)
      {
         builder.Builder.AddTypeAttributes<T>(attributes);
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
      public static ITypedReflectionTableBuilder<T> AddTypeAttributes<T>(this ITypedReflectionTableBuilder<T> builder, params Attribute[] attributes)
      {
         return AddTypeAttributes<T>(builder, attributes.AsEnumerable());
      }

      #endregion

      #region Parameter Attributes

      /// <summary>Adds attributes to the parameters of a method.</summary>
      /// <exception cref="ArgumentException">Thrown when one or more arguments have unsupported or
      /// illegal values.</exception>
      /// <typeparam name="T">The type of the object in which the method resides.</typeparam>
      /// <param name="builder">The reflection table builder.</param>
      /// <param name="expression">An expression representing a method invocation, where parameters are
      /// decorated using the <see cref="Decorate"/> class as placeholders for the parameters to which
      /// to add attributes, and <see cref="Param"/> for any parameters to ignore.</param>
      /// <returns>
      /// The <paramref name="builder"/> instance. Useful for chaining multiple calls.
      /// </returns>
      public static ITypedReflectionTableBuilder<T> AddParameterAttributes<T>(this ITypedReflectionTableBuilder<T> builder, Expression<Action<T>> expression)
      {
         builder.Builder.AddParameterAttributes<T>(expression);
         return builder;
      }

      /// <summary>Adds attributes to the return parameter of a method.</summary>
      /// <typeparam name="T">The type of the object in which the method resides.</typeparam>
      /// <param name="builder">The reflection table builder.</param>
      /// <param name="expression">An expression representing a method invocation.</param>
      /// <param name="attributes">The attributes to add to the return parameter.</param>
      /// <returns>
      /// The <paramref name="builder"/> instance. Useful for chaining multiple calls.
      /// </returns>
      public static ITypedReflectionTableBuilder<T> AddReturnParameterAttributes<T>(this ITypedReflectionTableBuilder<T> builder, Expression<Action<T>> expression, IEnumerable<Attribute> attributes)
      {
         builder.Builder.AddReturnParameterAttributes(expression, attributes);
         return builder;
      }

      /// <summary>Adds attributes to the return parameter of a method.</summary>
      /// <typeparam name="T">The type of the object in which the method resides.</typeparam>
      /// <param name="builder">The reflection table builder.</param>
      /// <param name="expression">An expression representing a method invocation.</param>
      /// <param name="attributes">The attributes to add to the return parameter.</param>
      /// <returns>
      /// The <paramref name="builder"/> instance. Useful for chaining multiple calls.
      /// </returns>
      public static ITypedReflectionTableBuilder<T> AddReturnParameterAttributes<T>(this ITypedReflectionTableBuilder<T> builder, Expression<Action<T>> expression, params Attribute[] attributes)
      {
         return AddReturnParameterAttributes(builder, expression, attributes.AsEnumerable());
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
      public static ITypedReflectionTableBuilder<T> AddMemberAttributes<T>(this ITypedReflectionTableBuilder<T> builder, Expression<Func<T, object>> expression, IEnumerable<Attribute> attributes)
      {
         builder.Builder.AddMemberAttributes<T>(expression, attributes);
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
      public static ITypedReflectionTableBuilder<T> AddMemberAttributes<T>(this ITypedReflectionTableBuilder<T> builder, Expression<Func<T, object>> expression, params Attribute[] attributes)
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
      public static ITypedReflectionTableBuilder<T> AddMemberAttributes<T>(this ITypedReflectionTableBuilder<T> builder, Expression<Action<T>> expression, IEnumerable<Attribute> attributes)
      {
         builder.Builder.AddMemberAttributes<T>(expression, attributes);
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
      public static ITypedReflectionTableBuilder<T> AddMemberAttributes<T>(this ITypedReflectionTableBuilder<T> builder, Expression<Action<T>> expression, params Attribute[] attributes)
      {
         return AddMemberAttributes<T>(builder, expression, attributes.AsEnumerable());
      }

      #endregion
   }
}
