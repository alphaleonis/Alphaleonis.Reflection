using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphaleonis.Reflection
{
   // TODO PP (2018-04-21): Document
   public static class Decorate
   {
      // TODO PP: Add overload accepting params Attribute[] as well.
      public static T Parameter<T>(IEnumerable<Attribute> attributes) => default(T);
   }
}
