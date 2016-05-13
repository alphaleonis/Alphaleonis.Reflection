using System;
using System.Linq;
using System.Reflection;
using CustomAttributeTable;

namespace CustomAttributeTableTests
{
   public partial class AttributeTableReflectionContext
   {
      private class AttributeTableProjectedEventInfo : DelegatingEventInfo, IAttributeTableProjector
      {
         public AttributeTableProjectedEventInfo(EventInfo eventInfo, AttributeTableReflectionContext reflectionContext) 
            : base(eventInfo)
         {
            ReflectionContext = reflectionContext;
         }

         public AttributeTableReflectionContext ReflectionContext { get; }
      }
   }
}
