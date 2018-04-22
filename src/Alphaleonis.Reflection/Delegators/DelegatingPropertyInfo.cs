using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Alphaleonis.Reflection
{
   // TODO PP (2018-04-21): Document
   public class DelegatingPropertyInfo : PropertyInfo
   {
      private readonly PropertyInfo m_property;

      public DelegatingPropertyInfo(PropertyInfo property)
      {
         m_property = property;
      }

      public override PropertyAttributes Attributes => m_property.Attributes;

      public override bool CanRead => m_property.CanRead;

      public override bool CanWrite => m_property.CanWrite;

      public override Type DeclaringType => m_property.DeclaringType;

      public override int MetadataToken => m_property.MetadataToken;

      public override Module Module => m_property.Module;

      public override string Name => m_property.Name;

      public override Type PropertyType => m_property.PropertyType;

      public override Type ReflectedType => m_property.ReflectedType;

      public PropertyInfo UnderlyingProperty => m_property;

      public override MethodInfo[] GetAccessors(bool nonPublic) => m_property.GetAccessors(nonPublic);

      public override object GetConstantValue() => m_property.GetConstantValue();

      public override object[] GetCustomAttributes(bool inherit) => m_property.GetCustomAttributes(inherit);

      public override object[] GetCustomAttributes(Type attributeType, bool inherit) => m_property.GetCustomAttributes(attributeType, inherit);

      public override IList<CustomAttributeData> GetCustomAttributesData() => m_property.GetCustomAttributesData();

      public override MethodInfo GetGetMethod(bool nonPublic) => m_property.GetGetMethod(nonPublic);

      public override ParameterInfo[] GetIndexParameters() => m_property.GetIndexParameters();

      public override Type[] GetOptionalCustomModifiers() => m_property.GetOptionalCustomModifiers();

      public override object GetRawConstantValue() => m_property.GetRawConstantValue();

      public override Type[] GetRequiredCustomModifiers() => m_property.GetRequiredCustomModifiers();

      public override MethodInfo GetSetMethod(bool nonPublic) => m_property.GetSetMethod(nonPublic);

      public override object GetValue(object obj, object[] index) => m_property.GetValue(obj, index);

      public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture) => m_property.GetValue(obj, invokeAttr, binder, index, culture);

      public override bool IsDefined(Type attributeType, bool inherit) => m_property.IsDefined(attributeType, inherit);

      public override void SetValue(object obj, object value, object[] index) => m_property.SetValue(obj, value, index);

      public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture) => m_property.SetValue(obj, value, invokeAttr, binder, index, culture);

      public override string ToString() => m_property.ToString();

      public override int GetHashCode() => m_property.GetHashCode();

      public override bool Equals(object obj)
      {
         DelegatingPropertyInfo other = obj as DelegatingPropertyInfo;
         if (other != null)
            return other.m_property.Equals(m_property);
         else
            return m_property.Equals(obj);
      }
   }
}
