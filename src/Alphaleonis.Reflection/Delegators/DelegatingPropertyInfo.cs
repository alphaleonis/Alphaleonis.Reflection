using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Alphaleonis.Reflection
{
   /// <summary>
   /// Wraps a <see cref="PropertyInfo"/> instance and delegates all method calls to that
   /// <see cref="PropertyInfo" />.
   /// </summary>
   /// <remarks>
   /// <para>Derive from this type and override only those members you have to provide customization
   /// in.</para>
   /// <para><see cref="DelegatingPropertyInfo"/> derives from <see cref="PropertyInfo"/> and implements most
   /// of the properties and methods of <see cref="PropertyInfo"/>. For
   ///       each member it implements, <see cref="DelegatingPropertyInfo"/> automatically delegates to
   ///       the corresponding member of the internal <see cref="PropertyInfo"/>
   ///       object which was supplied as the argument to the constructor.  This internal
   ///       <see cref="PropertyInfo"/> is exposed to deriving classes by the <see langword="protected" />
   ///       <see cref="m_propertyImpl"/> field.</para>
   /// </remarks>
   public class DelegatingPropertyInfo : PropertyInfo
   {
      private readonly PropertyInfo m_propertyImpl;

      public DelegatingPropertyInfo(PropertyInfo property)
      {
         m_propertyImpl = property;
      }

      public override PropertyAttributes Attributes => m_propertyImpl.Attributes;

      public override bool CanRead => m_propertyImpl.CanRead;

      public override bool CanWrite => m_propertyImpl.CanWrite;

      public override Type DeclaringType => m_propertyImpl.DeclaringType;

      public override int MetadataToken => m_propertyImpl.MetadataToken;

      public override Module Module => m_propertyImpl.Module;

      public override string Name => m_propertyImpl.Name;

      public override Type PropertyType => m_propertyImpl.PropertyType;

      public override Type ReflectedType => m_propertyImpl.ReflectedType;

      public PropertyInfo UnderlyingProperty => m_propertyImpl;

      public override MethodInfo[] GetAccessors(bool nonPublic) => m_propertyImpl.GetAccessors(nonPublic);

      public override object GetConstantValue() => m_propertyImpl.GetConstantValue();

      public override object[] GetCustomAttributes(bool inherit) => m_propertyImpl.GetCustomAttributes(inherit);

      public override object[] GetCustomAttributes(Type attributeType, bool inherit) => m_propertyImpl.GetCustomAttributes(attributeType, inherit);

      public override IList<CustomAttributeData> GetCustomAttributesData() => m_propertyImpl.GetCustomAttributesData();

      public override MethodInfo GetGetMethod(bool nonPublic) => m_propertyImpl.GetGetMethod(nonPublic);

      public override ParameterInfo[] GetIndexParameters() => m_propertyImpl.GetIndexParameters();

      public override Type[] GetOptionalCustomModifiers() => m_propertyImpl.GetOptionalCustomModifiers();

      public override object GetRawConstantValue() => m_propertyImpl.GetRawConstantValue();

      public override Type[] GetRequiredCustomModifiers() => m_propertyImpl.GetRequiredCustomModifiers();

      public override MethodInfo GetSetMethod(bool nonPublic) => m_propertyImpl.GetSetMethod(nonPublic);

      public override object GetValue(object obj, object[] index) => m_propertyImpl.GetValue(obj, index);

      public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture) => m_propertyImpl.GetValue(obj, invokeAttr, binder, index, culture);

      public override bool IsDefined(Type attributeType, bool inherit) => m_propertyImpl.IsDefined(attributeType, inherit);

      public override void SetValue(object obj, object value, object[] index) => m_propertyImpl.SetValue(obj, value, index);

      public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture) => m_propertyImpl.SetValue(obj, value, invokeAttr, binder, index, culture);

      public override string ToString() => m_propertyImpl.ToString();

      public override int GetHashCode() => m_propertyImpl.GetHashCode();

      public override bool Equals(object obj)
      {
         DelegatingPropertyInfo other = obj as DelegatingPropertyInfo;
         if (other != null)
            return other.m_propertyImpl.Equals(m_propertyImpl);
         else
            return m_propertyImpl.Equals(obj);
      }
   }
}
