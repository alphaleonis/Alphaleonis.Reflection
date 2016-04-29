using System;
using System.Linq;
using System.Reflection;
using CustomAttributeTable;

namespace CustomAttributeTableTests
{
   public partial class AttributeTableReflectionContext
   {
      public class AttributeTableProjectedConstructorInfo : DelegatingConstructorInfo, IAttributeTableProjector
      {
         public AttributeTableProjectedConstructorInfo(ConstructorInfo constructor, AttributeTableReflectionContext reflectionContext)
            : base(constructor)
         {
            ReflectionContext = reflectionContext;
         }

         public AttributeTableReflectionContext ReflectionContext { get; }
      }
   }
}
