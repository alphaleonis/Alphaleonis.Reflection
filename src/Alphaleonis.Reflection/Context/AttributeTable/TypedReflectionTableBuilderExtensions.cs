using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Alphaleonis.Reflection.Context
{
   public static class TypedReflectionTableBuilderExtensions
   {
      #region AddPropertyAttributes

      public static ITypedReflectionTableBuilder<T> AddPropertyAttributes<T>(this ITypedReflectionTableBuilder<T> builder, string propertyName, params Attribute[] attributes)
      {
         return AddPropertyAttributes<T>(builder, propertyName, attributes.AsEnumerable());
      }

      public static ITypedReflectionTableBuilder<T> AddPropertyAttributes<T>(this ITypedReflectionTableBuilder<T> builder, string propertyName, IEnumerable<Attribute> attributes)
      {
         builder.Builder.AddPropertyAttributes<T>(propertyName, attributes);
         return builder;
      }

      #endregion

      #region AddEventAttributes

      public static ITypedReflectionTableBuilder<T> AddEventAttributes<T>(this ITypedReflectionTableBuilder<T> builder, string eventName, IEnumerable<Attribute> attributes)
      {
         builder.Builder.AddEventAttributes<T>(eventName, attributes);
         return builder;
      }

      public static ITypedReflectionTableBuilder<T> AddEventAttributes<T>(this ITypedReflectionTableBuilder<T> builder, string eventName, params Attribute[] attributes)
      {
         return AddEventAttributes<T>(builder, eventName, attributes.AsEnumerable());
      }

      #endregion

      #region AddFieldAttributes

      public static ITypedReflectionTableBuilder<T> AddFieldAttributes<T>(this ITypedReflectionTableBuilder<T> builder, string fieldName, IEnumerable<Attribute> attributes)
      {
         builder.Builder.AddFieldAttributes<T>(fieldName, attributes);
         return builder;
      }

      public static ITypedReflectionTableBuilder<T> AddFieldAttributes<T>(this ITypedReflectionTableBuilder<T> builder, string fieldName, params Attribute[] attributes)
      {
         return AddFieldAttributes<T>(builder, fieldName, attributes.AsEnumerable());
      }

      #endregion

      #region AddTypeAttributes

      public static ITypedReflectionTableBuilder<T> AddTypeAttributes<T>(this ITypedReflectionTableBuilder<T> builder, IEnumerable<Attribute> attributes)
      {
         builder.Builder.AddTypeAttributes<T>(attributes);
         return builder;
      }

      public static ITypedReflectionTableBuilder<T> AddTypeAttributes<T>(this ITypedReflectionTableBuilder<T> builder, params Attribute[] attributes)
      {
         return AddTypeAttributes<T>(builder, attributes.AsEnumerable());
      }

      #endregion

      #region Parameter Attributes

      public static ITypedReflectionTableBuilder<T> AddParameterAttributes<T>(this ITypedReflectionTableBuilder<T> builder, Expression<Action<T>> expression)
      {
         builder.Builder.AddParameterAttributes<T>(expression);
         return builder;
      }

      public static ITypedReflectionTableBuilder<T> AddReturnParameterAttributes<T>(this ITypedReflectionTableBuilder<T> builder, Expression<Action<T>> expression, IEnumerable<Attribute> attributes)
      {
         builder.Builder.AddReturnParameterAttributes(expression, attributes);
         return builder;
      }

      public static ITypedReflectionTableBuilder<T> AddReturnParameterAttributes<T>(this ITypedReflectionTableBuilder<T> builder, Expression<Action<T>> expression, params Attribute[] attributes)
      {
         return AddReturnParameterAttributes(builder, expression, attributes.AsEnumerable());
      }

      #endregion

      #region Member Attributes

      public static ITypedReflectionTableBuilder<T> AddMemberAttributes<T>(this ITypedReflectionTableBuilder<T> builder, Expression<Func<T, object>> expression, IEnumerable<Attribute> attributes)
      {
         builder.Builder.AddMemberAttributes<T>(expression, attributes);
         return builder;
      }

      public static ITypedReflectionTableBuilder<T> AddMemberAttributes<T>(this ITypedReflectionTableBuilder<T> builder, Expression<Func<T, object>> expression, params Attribute[] attributes)
      {
         return AddMemberAttributes<T>(builder, expression, attributes.AsEnumerable());
      }

      public static ITypedReflectionTableBuilder<T> AddMemberAttributes<T>(this ITypedReflectionTableBuilder<T> builder, Expression<Action<T>> expression, IEnumerable<Attribute> attributes)
      {
         builder.Builder.AddMemberAttributes<T>(expression, attributes);
         return builder;
      }

      public static ITypedReflectionTableBuilder<T> AddMemberAttributes<T>(this ITypedReflectionTableBuilder<T> builder, Expression<Action<T>> expression, params Attribute[] attributes)
      {
         return AddMemberAttributes<T>(builder, expression, attributes.AsEnumerable());
      }

      #endregion
   }
}
