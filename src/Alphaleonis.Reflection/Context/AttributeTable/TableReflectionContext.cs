// Copyright (c) Peter Palotas
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Alphaleonis.Reflection.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Alphaleonis.Reflection.Context
{
   /// <summary>
   /// A reflection context that provides additional attributes to Types, members and parameters
   /// based on a static table.
   /// </summary>
   public partial class TableReflectionContext : CustomReflectionContextBase
   {
      #region Construction

      /// <summary>Constructor.</summary>
      /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
      /// <param name="table">The reflection table to use for adding attributes to types and members.</param>
      /// <param name="options">Options controlling various aspects of the reflection context.</param>
      public TableReflectionContext(IReflectionTable table, TableReflectionContextOptions options)
      {
         Table = table ?? throw new ArgumentNullException(nameof(table), $"{nameof(table)} is null.");
         Options = options;
      }

      #endregion

      #region Properties

      /// <summary>Gets the underlying <see cref="IReflectionTable"/> of this reflection context.</summary>
      private IReflectionTable Table { get; }

      /// <summary>Gets options passed to the constructor of this reflection context.</summary>
      public TableReflectionContextOptions Options { get; }

      #endregion

      #region Factories

      /// <inheritdoc/>
      protected override Assembly MapAssemblyCore(Assembly assembly) => new ProjectedReflectionTableAssembly(assembly, this);

      /// <inheritdoc/>
      protected override ConstructorInfo MapConstructorCore(ConstructorInfo constructor) => new ProjectedReflectionTableConstructorInfo(constructor, this);

      /// <inheritdoc/>
      protected override EventInfo MapEventCore(EventInfo eventInfo) => new ProjectedReflectionTableEventInfo(eventInfo, this);

      /// <inheritdoc/>
      protected override FieldInfo MapFieldCore(FieldInfo field) => new ProjectedReflectionTableFieldInfo(field, this);

      /// <inheritdoc/>
      protected override MethodInfo MapMethodCore(MethodInfo method) => new ProjectedReflectionTableMethodInfo(method, this);

      /// <inheritdoc/>
      protected override ParameterInfo MapParameterCore(ParameterInfo parameter) => new ProjectedReflectionTableParameterInfo(parameter, this);

      /// <inheritdoc/>
      protected override PropertyInfo MapPropertyCore(PropertyInfo property) => new ProjectedReflectionTablePropertyInfo(property, this);

      /// <inheritdoc/>
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
