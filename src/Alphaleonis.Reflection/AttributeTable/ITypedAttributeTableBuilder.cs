using System;
using System.Linq;

namespace Alphaleonis.Reflection
{
   public interface ITypedAttributeTableBuilder<T>
   {
      IAttributeTableBuilder Builder { get; }
   }
}
