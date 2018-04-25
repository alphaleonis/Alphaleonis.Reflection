using System;
using System.Linq;

namespace Alphaleonis.Reflection
{
   public interface ITypedCustomAttributeTableBuilder<T>
   {
      ICustomAttributeTableBuilder Builder { get; }
   }
}
