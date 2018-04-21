using System;
using System.Linq;

namespace Alphaleonis.Reflection
{
   // TODO PP (2018-04-21): Document
   [Flags]
   public enum AttributeTableReflectionContextOptions
   {
      Default = 0,
      HonorPropertyAttributeInheritance = 1,
      HonorEventAttributeInheritance = 2
   }
}
