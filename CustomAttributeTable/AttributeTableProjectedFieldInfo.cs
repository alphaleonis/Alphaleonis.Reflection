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

         public override object[] GetCustomAttributes(bool inherit)
         {
            throw new NotImplementedException();
         }

         public override object[] GetCustomAttributes(Type attributeType, bool inherit)
         {
            throw new NotImplementedException();
         }

         public override Type[] GetOptionalCustomModifiers() => ReflectionContext.MapTypes(base.GetOptionalCustomModifiers());

         public override Type[] GetRequiredCustomModifiers() => ReflectionContext.MapTypes(base.GetRequiredCustomModifiers());

         public override bool IsDefined(Type attributeType, bool inherit)
         {
            throw new NotImplementedException();
         }

         public override Type ReflectedType => ReflectionContext.MapType(base.ReflectedType);
      }
   }
}
