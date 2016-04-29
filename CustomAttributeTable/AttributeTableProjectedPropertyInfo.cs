using System;
using System.Linq;
using System.Reflection;
using CustomAttributeTable;

namespace CustomAttributeTableTests
{
   public partial class AttributeTableReflectionContext
   {
      private class AttributeTableProjectedPropertyInfo : DelegatingPropertyInfo, IAttributeTableProjector
      {
         public AttributeTableProjectedPropertyInfo(PropertyInfo property, AttributeTableReflectionContext reflectionContext)
            : base(property)
         {
            ReflectionContext = reflectionContext;
         }

         public AttributeTableReflectionContext ReflectionContext { get; }
      }
   }

   
}
