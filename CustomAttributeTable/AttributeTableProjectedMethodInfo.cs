using System;
using System.Linq;
using System.Reflection;
using CustomAttributeTable;

namespace CustomAttributeTableTests
{
   public partial class AttributeTableReflectionContext
   {
      public class AttributeTableProjectedMethodInfo : DelegatingMethodInfo, IAttributeTableProjector
      {
         public AttributeTableProjectedMethodInfo(MethodInfo method, AttributeTableReflectionContext reflectionContext)
            : base(method)
         {
            ReflectionContext = reflectionContext;
         }

         public AttributeTableReflectionContext ReflectionContext { get; }
      }
   }
}
