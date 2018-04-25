using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Alphaleonis.Reflection
{
   /// <summary>
   /// Wraps a <see cref="FieldInfo"/> instance and delegates all method calls to that
   /// <see cref="FieldInfo" />.
   /// </summary>
   /// <remarks>
   /// <para>Derive from this type and override only those members you have to provide customization
   /// in.</para>
   /// <para><see cref="DelegatingFieldInfo"/> derives from <see cref="FieldInfo"/> and implements most
   /// of the properties and methods of <see cref="FieldInfo"/>. For
   ///       each member it implements, <see cref="DelegatingFieldInfo"/> automatically delegates to
   ///       the corresponding member of the internal <see cref="FieldInfo"/>
   ///       object which was supplied as the argument to the constructor.  This internal
   ///       <see cref="FieldInfo"/> is exposed to deriving classes by the <see langword="protected" />
   ///       <see cref="m_fieldImpl"/> field.</para>
   /// </remarks>
   public class DelegatingFieldInfo : FieldInfo
   {
      private readonly FieldInfo m_fieldImpl;

      public DelegatingFieldInfo(FieldInfo field)
      {
         m_fieldImpl = field;
      }

      public override FieldAttributes Attributes => m_fieldImpl.Attributes;

      public override Type DeclaringType => m_fieldImpl.DeclaringType;

      public override RuntimeFieldHandle FieldHandle => m_fieldImpl.FieldHandle;

      public override Type FieldType => m_fieldImpl.FieldType;

      public override bool IsSecurityCritical => m_fieldImpl.IsSecurityCritical;

      public override bool IsSecuritySafeCritical => m_fieldImpl.IsSecuritySafeCritical;

      public override bool IsSecurityTransparent => m_fieldImpl.IsSecurityTransparent;

      public override int MetadataToken => m_fieldImpl.MetadataToken;

      public override Module Module => m_fieldImpl.Module;

      public override string Name => m_fieldImpl.Name;

      public override Type ReflectedType => m_fieldImpl.ReflectedType;

      public FieldInfo UnderlyingField => m_fieldImpl;

      public override object[] GetCustomAttributes(bool inherit) => m_fieldImpl.GetCustomAttributes(inherit);

      public override object[] GetCustomAttributes(Type attributeType, bool inherit) => m_fieldImpl.GetCustomAttributes(attributeType, inherit);

      public override IList<CustomAttributeData> GetCustomAttributesData() => m_fieldImpl.GetCustomAttributesData();

      public override Type[] GetOptionalCustomModifiers() => m_fieldImpl.GetOptionalCustomModifiers();

      public override object GetRawConstantValue() => m_fieldImpl.GetRawConstantValue();

      public override Type[] GetRequiredCustomModifiers() => m_fieldImpl.GetRequiredCustomModifiers();

      public override object GetValue(object obj) => m_fieldImpl.GetValue(obj);

      public override object GetValueDirect(TypedReference obj) => m_fieldImpl.GetValueDirect(obj);

      public override bool IsDefined(Type attributeType, bool inherit) => m_fieldImpl.IsDefined(attributeType, inherit);

      public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture) => m_fieldImpl.SetValue(obj, value, invokeAttr, binder, culture);

      public override void SetValueDirect(TypedReference obj, object value) => m_fieldImpl.SetValueDirect(obj, value);

      public override string ToString() => m_fieldImpl.ToString();

      public override int GetHashCode() => m_fieldImpl.GetHashCode();

      public override bool Equals(object obj)
      {
         DelegatingFieldInfo delegatingFieldInfo = obj as DelegatingFieldInfo;
         if (delegatingFieldInfo != null)
            return delegatingFieldInfo.m_fieldImpl.Equals(m_fieldImpl);
         else
            return m_fieldImpl.Equals(obj);
      }

      public override IEnumerable<CustomAttributeData> CustomAttributes => m_fieldImpl.CustomAttributes;
   }

   
}
