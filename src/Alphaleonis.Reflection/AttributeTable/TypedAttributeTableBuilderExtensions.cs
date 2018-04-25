using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Alphaleonis.Reflection
{
   public static class TypedAttributeTableBuilderExtensions
   {
      #region AddPropertyAttributes

      public static ITypedAttributeTableBuilder<T> AddPropertyAttributes<T>(this ITypedAttributeTableBuilder<T> builder, string propertyName, params Attribute[] attributes)
      {
         return AddPropertyAttributes<T>(builder, propertyName, attributes.AsEnumerable());
      }

      public static ITypedAttributeTableBuilder<T> AddPropertyAttributes<T>(this ITypedAttributeTableBuilder<T> builder, string propertyName, IEnumerable<Attribute> attributes)
      {
         builder.Builder.AddPropertyAttributes<T>(propertyName, attributes);
         return builder;
      }

      #endregion

      #region AddEventAttributes

      public static ITypedAttributeTableBuilder<T> AddEventAttributes<T>(this ITypedAttributeTableBuilder<T> builder, string eventName, IEnumerable<Attribute> attributes)
      {
         builder.Builder.AddEventAttributes<T>(eventName, attributes);
         return builder;
      }

      public static ITypedAttributeTableBuilder<T> AddEventAttributes<T>(this ITypedAttributeTableBuilder<T> builder, string eventName, params Attribute[] attributes)
      {
         return AddEventAttributes<T>(builder, eventName, attributes.AsEnumerable());
      }

      #endregion

      #region AddFieldAttributes

      public static ITypedAttributeTableBuilder<T> AddFieldAttributes<T>(this ITypedAttributeTableBuilder<T> builder, string fieldName, IEnumerable<Attribute> attributes)
      {
         builder.Builder.AddFieldAttributes<T>(fieldName, attributes);
         return builder;
      }

      public static ITypedAttributeTableBuilder<T> AddFieldAttributes<T>(this ITypedAttributeTableBuilder<T> builder, string fieldName, params Attribute[] attributes)
      {
         return AddFieldAttributes<T>(builder, fieldName, attributes.AsEnumerable());
      }

      #endregion

      #region AddTypeAttributes

      public static ITypedAttributeTableBuilder<T> AddTypeAttributes<T>(this ITypedAttributeTableBuilder<T> builder, IEnumerable<Attribute> attributes)
      {
         builder.Builder.AddTypeAttributes<T>(attributes);
         return builder;
      }

      public static ITypedAttributeTableBuilder<T> AddTypeAttributes<T>(this ITypedAttributeTableBuilder<T> builder, params Attribute[] attributes)
      {
         return AddTypeAttributes<T>(builder, attributes.AsEnumerable());
      }

      #endregion

      #region Parameter Attributes

      public static ITypedAttributeTableBuilder<T> AddParameterAttributes<T>(this ITypedAttributeTableBuilder<T> builder, Expression<Action<T>> expression)
      {
         builder.Builder.AddParameterAttributes<T>(expression);
         return builder;
      }

      public static ITypedAttributeTableBuilder<T> AddReturnParameterAttributes<T>(this ITypedAttributeTableBuilder<T> builder, Expression<Action<T>> expression, IEnumerable<Attribute> attributes)
      {
         builder.Builder.AddReturnParameterAttributes(expression, attributes);
         return builder;
      }

      public static ITypedAttributeTableBuilder<T> AddReturnParameterAttributes<T>(this ITypedAttributeTableBuilder<T> builder, Expression<Action<T>> expression, params Attribute[] attributes)
      {
         return AddReturnParameterAttributes(builder, expression, attributes.AsEnumerable());
      }

      #endregion

      #region Member Attributes

      public static ITypedAttributeTableBuilder<T> AddMemberAttributes<T>(this ITypedAttributeTableBuilder<T> builder, Expression<Func<T, object>> expression, IEnumerable<Attribute> attributes)
      {
         builder.Builder.AddMemberAttributes<T>(expression, attributes);
         return builder;
      }

      public static ITypedAttributeTableBuilder<T> AddMemberAttributes<T>(this ITypedAttributeTableBuilder<T> builder, Expression<Func<T, object>> expression, params Attribute[] attributes)
      {
         return AddMemberAttributes<T>(builder, expression, attributes.AsEnumerable());
      }

      public static ITypedAttributeTableBuilder<T> AddMemberAttributes<T>(this ITypedAttributeTableBuilder<T> builder, Expression<Action<T>> expression, IEnumerable<Attribute> attributes)
      {
         builder.Builder.AddMemberAttributes<T>(expression, attributes);
         return builder;
      }

      public static ITypedAttributeTableBuilder<T> AddMemberAttributes<T>(this ITypedAttributeTableBuilder<T> builder, Expression<Action<T>> expression, params Attribute[] attributes)
      {
         return AddMemberAttributes<T>(builder, expression, attributes.AsEnumerable());
      }

      #endregion
   }
}
