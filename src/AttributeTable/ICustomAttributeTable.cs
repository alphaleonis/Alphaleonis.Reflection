using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Alphaleonis.Reflection
{
   public interface ICustomAttributeTable
   {
      IEnumerable<Attribute> GetCustomAttributes(Type type);
      IEnumerable<Attribute> GetCustomAttributes(MemberInfo member);
      IEnumerable<Attribute> GetCustomAttributes(ParameterInfo parameterInfo);
   }
}
