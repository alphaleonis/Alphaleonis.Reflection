using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Alphaleonis.Reflection
{
   // TODO PP (2018-04-25): document
   public interface IAttributeTableBuilder
   {
      IAttributeTableBuilder AddMemberAttributes(MemberInfo member, IEnumerable<Attribute> attributes);
      IAttributeTableBuilder AddParameterAttributes(ParameterInfo parameter, IEnumerable<Attribute> attributes);
      IAttributeTable CreateTable();
   }
}
