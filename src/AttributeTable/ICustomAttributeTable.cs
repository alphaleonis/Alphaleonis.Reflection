using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Alphaleonis.Reflection
{
   // TODO PP (2018-04-21): Document
   public interface ICustomAttributeTable
   {
      IEnumerable<Attribute> GetCustomAttributes(Type type);
      IEnumerable<Attribute> GetCustomAttributes(MemberInfo member);
      IEnumerable<Attribute> GetCustomAttributes(ParameterInfo parameterInfo);
   }
}
