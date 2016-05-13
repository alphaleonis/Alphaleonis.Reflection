using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CustomAttributeTable;

namespace CustomAttributeTableTests
{
   public partial class AttributeTableReflectionContext
   {
      private class AttributeTableProjectedType : DelegatingType, IAttributeTableProjector
      {
         #region Constructor

         public AttributeTableProjectedType(Type delegatingType, AttributeTableReflectionContext context)
            : base(delegatingType)
         {
            ReflectionContext = context;
         }

         #endregion

         #region Field Projection

         public override FieldInfo GetField(string name, BindingFlags bindingAttr)
         {
            return ReflectionContext.MapMember(base.GetField(name, bindingAttr));
         }

         public override FieldInfo[] GetFields(BindingFlags bindingAttr)
         {
            return base.GetFields(bindingAttr).Select(field => ReflectionContext.MapMember(field)).ToArray();
         }

         #endregion

         #region Properties

         public AttributeTableReflectionContext ReflectionContext { get; }

         public override Type BaseType => ReflectionContext.MapType(base.BaseType?.GetTypeInfo());

         public override Assembly Assembly => ReflectionContext.MapAssembly(base.Assembly);

         #endregion

         #region Public Methods

         public override object[] GetCustomAttributes(bool inherit)
         {
            return GetCustomAttributes(typeof(Attribute), inherit);
         }

         public override object[] GetCustomAttributes(Type attributeType, bool inherit)
         {
            // Start with the table attributes.
            List<object> result = new List<object>();

            result.Add(ReflectionContext.ContextIdentifierAttribute);

            result.AddRange(ReflectionContext.Table.GetCustomAttributes(this).Where(attr => attributeType.IsAssignableFrom(attr.GetType())));

            // Then check this type, without inheritance. Add only attributes if Multiple = true OR attribute not already exists.
            foreach (var ca in base.GetCustomAttributes(attributeType, false))
            {
               if (GetAttributeUsage(ca.GetType()).AllowMultiple || !result.Any(attr => attr.GetType().Equals(ca.GetType())))
                  result.Add(ca);
            }

            // Then get base attributes, add only if Inherit = true AND (Multiple = true OR attribute not already exists).
            if (inherit && BaseType != null && !BaseType.Equals(typeof(object)))
            {
               foreach (var ca in BaseType.GetCustomAttributes(attributeType, inherit))
               {
                  AttributeUsageAttribute attributeUsage = GetAttributeUsage(ca.GetType());
                  if (attributeUsage.Inherited && (attributeUsage.AllowMultiple || !result.Any(attr => attr.GetType().Equals(ca.GetType()))))
                     result.Add(ca);
               }
            }

            object[] arrResult = (object[])Array.CreateInstance(attributeType, result.Count);
            for (int i = 0; i < result.Count; i++)
            {
               arrResult[i] = result[i];
            }
            
            return arrResult;
         }

         protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
         {
            return ReflectionContext.MapMember(base.GetPropertyImpl(name, bindingAttr, binder, returnType, types, modifiers));
         }

         public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
         {
            return base.GetProperties(bindingAttr).Select(prop => ReflectionContext.MapMember(prop)).ToArray();
         }

         protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
         {
            return ReflectionContext.MapMember(base.GetMethodImpl(name, bindingAttr, binder, callConvention, types, modifiers));
         }

         public override MethodInfo[] GetMethods(BindingFlags bindingAttr)
         {
            return base.GetMethods(bindingAttr).Select(method => ReflectionContext.MapMember(method)).ToArray();
         }

         #endregion
      }
   }

   
}
