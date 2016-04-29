using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace CustomAttributeTable
{
   /// <summary>
   /// Wraps a <see cref="PropertyInfo"/> object and delegates methods to that <c>PropertyInfo</c>. 
   /// Similar to what <see cref="System.TypeDelegator"/> does for <see cref="System.Type"/>.
   /// </summary>
   [Serializable]
   public class PropertyInfoDelegator : PropertyInfo
   {
      private PropertyInfo m_parent;

      public PropertyInfoDelegator(PropertyInfo parent)
      {
         if (parent == null)
            throw new ArgumentNullException("parent", "parent is null.");

         m_parent = parent;
      }

      #region Properties

      public override PropertyAttributes Attributes => m_parent.Attributes;
      public override bool CanRead => m_parent.CanRead;
      public override bool CanWrite => m_parent.CanWrite;
      public override Type PropertyType => m_parent.PropertyType;
      public override int MetadataToken => m_parent.MetadataToken;
      public override Module Module => m_parent.Module;
      public override Type ReflectedType => m_parent.ReflectedType;
      public override string Name => m_parent.Name;
      public override Type DeclaringType => m_parent.DeclaringType;

      #endregion

      #region Methods

      public override MethodInfo[] GetAccessors(bool nonPublic)
      {
         return m_parent.GetAccessors(nonPublic);
      }

      public override MethodInfo GetGetMethod(bool nonPublic)
      {
         return m_parent.GetGetMethod(nonPublic);
      }

      public override ParameterInfo[] GetIndexParameters()
      {
         return m_parent.GetIndexParameters();
      }

      public override MethodInfo GetSetMethod(bool nonPublic)
      {
         return m_parent.GetSetMethod(nonPublic);
      }

      public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
      {
         return m_parent.GetValue(obj, invokeAttr, binder, index, culture);
      }

      public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture)
      {
         m_parent.SetValue(obj, value, invokeAttr, binder, index, culture);
      }

      public override object[] GetCustomAttributes(Type attributeType, bool inherit)
      {
         return m_parent.GetCustomAttributes(attributeType, inherit);
      }

      public override object[] GetCustomAttributes(bool inherit)
      {
         return m_parent.GetCustomAttributes(inherit);
      }

      public override bool IsDefined(Type attributeType, bool inherit)
      {
         return m_parent.IsDefined(attributeType, inherit);
      }

      public override IList<CustomAttributeData> GetCustomAttributesData()
      {
         return m_parent.GetCustomAttributesData();
      }

      public override object GetConstantValue()
      {
         return m_parent.GetConstantValue();
      }

      public override int GetHashCode()
      {
         return m_parent.GetHashCode();
      }

      public override bool Equals(object obj)
      {
         return m_parent.Equals(obj);
      }

      public override Type[] GetOptionalCustomModifiers()
      {
         return m_parent.GetOptionalCustomModifiers();
      }

      public override object GetRawConstantValue()
      {
         return m_parent.GetRawConstantValue();
      }

      public override Type[] GetRequiredCustomModifiers()
      {
         return m_parent.GetRequiredCustomModifiers();
      }

      public override string ToString()
      {
         return m_parent.ToString();
      }

      #endregion
   }
}
