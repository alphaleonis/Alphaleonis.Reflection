using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Alphaleonis.Reflection
{
   public class DelegatingFieldInfo : FieldInfo
   {
      private readonly FieldInfo m_field;

      public DelegatingFieldInfo(FieldInfo field)
      {
         m_field = field;
      }

      public override FieldAttributes Attributes => m_field.Attributes;

      public override Type DeclaringType => m_field.DeclaringType;

      public override RuntimeFieldHandle FieldHandle => m_field.FieldHandle;

      public override Type FieldType => m_field.FieldType;

      public override bool IsSecurityCritical => m_field.IsSecurityCritical;

      public override bool IsSecuritySafeCritical => m_field.IsSecuritySafeCritical;

      public override bool IsSecurityTransparent => m_field.IsSecurityTransparent;

      public override int MetadataToken => m_field.MetadataToken;

      public override Module Module => m_field.Module;

      public override string Name => m_field.Name;

      public override Type ReflectedType => m_field.ReflectedType;

      public FieldInfo UnderlyingField => m_field;

      public override object[] GetCustomAttributes(bool inherit) => m_field.GetCustomAttributes(inherit);

      public override object[] GetCustomAttributes(Type attributeType, bool inherit) => m_field.GetCustomAttributes(attributeType, inherit);

      public override IList<CustomAttributeData> GetCustomAttributesData() => m_field.GetCustomAttributesData();

      public override Type[] GetOptionalCustomModifiers() => m_field.GetOptionalCustomModifiers();

      public override object GetRawConstantValue() => m_field.GetRawConstantValue();

      public override Type[] GetRequiredCustomModifiers() => m_field.GetRequiredCustomModifiers();

      public override object GetValue(object obj) => m_field.GetValue(obj);

      public override object GetValueDirect(TypedReference obj) => m_field.GetValueDirect(obj);

      public override bool IsDefined(Type attributeType, bool inherit) => m_field.IsDefined(attributeType, inherit);

      public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture) => m_field.SetValue(obj, value, invokeAttr, binder, culture);

      public override void SetValueDirect(TypedReference obj, object value) => m_field.SetValueDirect(obj, value);

      public override string ToString() => m_field.ToString();

      public override int GetHashCode() => m_field.GetHashCode();

      public override bool Equals(object obj)
      {
         DelegatingFieldInfo delegatingFieldInfo = obj as DelegatingFieldInfo;
         if (delegatingFieldInfo != null)
            return delegatingFieldInfo.m_field.Equals(m_field);
         else
            return m_field.Equals(obj);
      }

      public override IEnumerable<CustomAttributeData> CustomAttributes => m_field.CustomAttributes;
   }

   
}
