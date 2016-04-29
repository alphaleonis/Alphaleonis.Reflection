using CustomAttributeTable;
using System;
using System.Linq;
using System.Reflection;

namespace CustomAttributeTableTests
{
   public partial class AttributeTableReflectionContext 
   {
      private class AttributeTableProjectedAssembly : DelegatingAssembly, IAttributeTableProjector
      {
         public AttributeTableProjectedAssembly(Assembly assembly, AttributeTableReflectionContext context) : base(assembly)
         {
            ReflectionContext = context;
         }

         public AttributeTableReflectionContext ReflectionContext { get; }

         public override object[] GetCustomAttributes(bool inherit)
         {
            return ReflectionContext.AddContextIdentifierAttribute(base.GetCustomAttributes(inherit));
         }

         public override object[] GetCustomAttributes(Type attributeType, bool inherit)
         {
            return ReflectionContext.AddContextIdentifierAttribute(base.GetCustomAttributes(attributeType, inherit));
         }

         public override Type GetType(string name, bool throwOnError, bool ignoreCase)
         {
            return ReflectionContext.MapType(base.GetType(name, throwOnError, ignoreCase));
         }

         public override Type[] GetTypes()
         {
            return ReflectionContext.MapTypes(base.GetTypes()).ToArray();
         }

         public override Type[] GetExportedTypes()
         {
            return ReflectionContext.MapTypes(base.GetExportedTypes()).ToArray();
         }
      }
   }

}
