using System;
using System.Linq;
using System.Reflection;
using CustomAttributeTable;

namespace CustomAttributeTableTests
{
   public partial class AttributeTableReflectionContext
   {
      public class AttributeTableProjectedParameterInfo : DelegatingParameterInfo, IAttributeTableProjector
      {
         public AttributeTableProjectedParameterInfo(ParameterInfo parameter, AttributeTableReflectionContext reflectionContext)
            : base(parameter)
         {
            ReflectionContext = reflectionContext;
         }

         public AttributeTableReflectionContext ReflectionContext { get; }
      }
   }

   
}
