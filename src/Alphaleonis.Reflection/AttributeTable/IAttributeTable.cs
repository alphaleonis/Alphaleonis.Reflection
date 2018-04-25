using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Alphaleonis.Reflection
{
   // TODO PP (2018-04-21): Document
   public interface IAttributeTable
   {
      IReadOnlyList<Attribute> GetCustomAttributes(MemberInfo member);
      IReadOnlyList<Attribute> GetCustomAttributes(ParameterInfo parameterInfo);
   }
}
