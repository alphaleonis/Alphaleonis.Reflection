using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphaleonis.Reflection
{
   public static class Decorate
   {
      public static T Parameter<T>(IEnumerable<Attribute> attributes) => default(T);

      public static T Parameter<T>(params Attribute[] attributes) => default(T);
   }
}
