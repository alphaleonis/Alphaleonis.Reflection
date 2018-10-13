// Copyright (c) Peter Palotas
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Alphaleonis.Reflection.Context
{   
   /// <summary>Represents a type that is used to build an immutable <see cref="IReflectionTable"/>.</summary>
   public interface IReflectionTableBuilder : IReflectionTable
   {
      /// <summary>Adds the specified <paramref name="attributes"/> to the specified <paramref name="member"/>.</summary>
      /// <param name="member">The member to add the attributes to.</param>
      /// <param name="attributes">The attributes to add.</param>
      /// <returns>This <see cref="IReflectionTableBuilder"/>, allowing for chaining multiple method calls together.</returns>
      IReflectionTableBuilder AddMemberAttributes(MemberInfo member, IEnumerable<Attribute> attributes);

      /// <summary>Adds the specified <paramref name="attributes"/> to the specified <paramref name="parameter"/>.</summary>
      /// <param name="parameter">The parameter to add the attributes to.</param>
      /// <param name="attributes">The attributes to add.</param>
      /// <returns>This <see cref="IReflectionTableBuilder"/>, allowing for chaining multiple method calls together.</returns>
      IReflectionTableBuilder AddParameterAttributes(ParameterInfo parameter, IEnumerable<Attribute> attributes);

      /// <summary>Creates an immutable table containing information about the attributes added to various members via
      ///          this builder.</summary>
      /// <returns>The <see cref="IReflectionTable"/> containing the attributes added to this table..</returns>
      IReflectionTable CreateTable();
   }
}
