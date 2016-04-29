using System;
using System.Linq;
using System.Reflection;
using CustomAttributeTable;

namespace CustomAttributeTableTests
{

   public partial class AttributeTableReflectionContext
   {
      private class AttributeTableProjectedFieldInfo : DelegatingFieldInfo, IAttributeTableProjector
      {
         public AttributeTableProjectedFieldInfo(FieldInfo field, AttributeTableReflectionContext reflectionContext)
            : base(field)
         {
            ReflectionContext = reflectionContext;
         }

         public AttributeTableReflectionContext ReflectionContext { get; }

         public override Type FieldType => ReflectionContext.MapType(base.FieldType);

         public override Type DeclaringType => ReflectionContext.MapType(base.DeclaringType);         
      }
   }
}
