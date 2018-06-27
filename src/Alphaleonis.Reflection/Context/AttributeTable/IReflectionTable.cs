using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Alphaleonis.Reflection.Context
{
   /// <summary>
   /// A table that is used by <see cref="TableReflectionContext"/> to add attributes and members to
   /// a type and its members.
   /// </summary>
   /// <seealso cref="ReflectionTableBuilder"/>
   public interface IReflectionTable
   {
      /// <summary>Gets the custom attributes defined for the specified <paramref name="member"/>.</summary>
      /// <param name="member">The member for which to retrieve the attributes.</param>
      /// <returns>The custom attributes defined in this table for the specified <paramref name="member"/>.</returns>
      /// <remarks>This method does not return any attributes actually defined on the member in code, only those present in this table.</remarks>
      IReadOnlyList<Attribute> GetCustomAttributes(MemberInfo member);

      /// <summary>Gets the custom attributes defined for the specified <paramref name="parameter"/>.</summary>
      /// <param name="parameter">The parameter for which to retrieve the attributes.</param>
      /// <returns>The custom attributes defined in this table for the specified <paramref name="parameter"/>.</returns>
      /// <remarks>This method does not return any attributes actually defined on the parameter in code, only those present in this table.</remarks>
      IReadOnlyList<Attribute> GetCustomAttributes(ParameterInfo parameter);
   }
}
