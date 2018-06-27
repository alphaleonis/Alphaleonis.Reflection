using Alphaleonis.Reflection.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Alphaleonis.Reflection.Context
{
   // TODO PP (2018-04-21): Document
   public partial class TableReflectionContext : CustomReflectionContextBase
   {
      #region Construction

      public TableReflectionContext(IReflectionTable table, TableReflectionContextOptions options)
      {
         if (table == null)
            throw new ArgumentNullException(nameof(table), $"{nameof(table)} is null.");

         Table = table;
         Options = options;
      }

      #endregion

      #region Properties

      private IReflectionTable Table { get; }

      public TableReflectionContextOptions Options { get; }

      #endregion

      #region Factories

      protected override Assembly MapAssemblyCore(Assembly assembly) => new ProjectedReflectionTableAssembly(assembly, this);

      protected override ConstructorInfo MapConstructorCore(ConstructorInfo constructor) => new ProjectedReflectionTableConstructorInfo(constructor, this);

      protected override EventInfo MapEventCore(EventInfo eventInfo) => new ProjectedReflectionTableEventInfo(eventInfo, this);

      protected override FieldInfo MapFieldCore(FieldInfo field) => new ProjectedReflectionTableFieldInfo(field, this);

      protected override MethodInfo MapMethodCore(MethodInfo method) => new ProjectedReflectionTableMethodInfo(method, this);

      protected override ParameterInfo MapParameterCore(ParameterInfo parameter) => new ProjectedReflectionTableParameterInfo(parameter, this);

      protected override PropertyInfo MapPropertyCore(PropertyInfo property) => new ProjectedReflectionTablePropertyInfo(property, this);

      protected override Type MapTypeCore(Type type) => new ProjectedReflectionTableType(type, this);
      
      #endregion

      #region Private Utility Methods

      //private static AttributeUsageAttribute GetAttributeUsage(ICustomAttributeProvider decoratedAttribute)
      //{
      //   return AttributeUtil.GetAttributeUsage(decoratedAttribute);
      //}

      #endregion


   }
}
