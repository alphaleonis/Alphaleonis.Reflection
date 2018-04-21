using System;
using System.Linq;

namespace Alphaleonis.Reflection
{
   internal interface IAttributeTableProjector
   {
      AttributeTableReflectionContext ReflectionContext { get; }
   }
}
