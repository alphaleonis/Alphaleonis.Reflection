using System;
using System.Linq;

namespace CustomAttributeTableTests
{
   internal interface IAttributeTableProjector
   {
      AttributeTableReflectionContext ReflectionContext { get; }
   }
}
