using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CustomAttributeTable
{

   public interface ICustomAttributeTable
   {
      IEnumerable<Attribute> GetCustomAttributes(MemberInfo member, bool inherit = false);

      IEnumerable<Attribute> GetCustomAttributes(ParameterInfo parameterInfo);
   }
   



}
