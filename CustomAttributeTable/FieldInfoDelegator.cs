using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace CustomAttributeTable
{

   /// <summary>
   /// Wraps a <see cref="FieldInfo"/> object and delegates methods to that <c>FieldInfo</c>. 
   /// Similar to what <see cref="System.TypeDelegator"/> does for <see cref="System.Type"/>.
   /// </summary>
   public class FieldInfoDelegator : FieldInfo
   {
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1051:DoNotDeclareVisibleInstanceFields", Justification = "Follows the same pattern as TypeDelegator in the framework")]
      protected readonly FieldInfo m_fieldImpl;

      public FieldInfoDelegator(FieldInfo parent)
      {
         if (parent == null)
            throw new ArgumentNullException(nameof(parent), $"{nameof(parent)} is null.");

         m_fieldImpl = parent;
      }

      public override FieldAttributes Attributes
      {
         get
         {
            return m_fieldImpl.Attributes;
         }
      }

      public override Type DeclaringType
      {
         get
         {
            return m_fieldImpl.DeclaringType;
         }
      }

      public override RuntimeFieldHandle FieldHandle
      {
         get
         {
            return m_fieldImpl.FieldHandle;
         }
      }

      public override Type FieldType
      {
         get
         {
            return m_fieldImpl.FieldType;
         }
      }

      public override string Name
      {
         get
         {
            return m_fieldImpl.Name;
         }
      }

      public override Type ReflectedType
      {
         get
         {
            return m_fieldImpl.ReflectedType;
         }
      }

      public override object[] GetCustomAttributes(bool inherit)
      {
         return m_fieldImpl.GetCustomAttributes(inherit);
      }

      public override object[] GetCustomAttributes(Type attributeType, bool inherit)
      {
         return m_fieldImpl.GetCustomAttributes(attributeType, inherit);
      }

      public override object GetValue(object obj)
      {
         return m_fieldImpl.GetValue(obj);
      }

      public override bool IsDefined(Type attributeType, bool inherit)
      {
         return m_fieldImpl.IsDefined(attributeType, inherit);
      }

      public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture)
      {
         m_fieldImpl.SetValue(obj, value, invokeAttr, binder, culture);
      }

      public override bool Equals(object obj)
      {
         FieldInfoDelegator otherDelegator = obj as FieldInfoDelegator;
         if (otherDelegator != null)
            return otherDelegator.m_fieldImpl.Equals(m_fieldImpl);

         FieldInfo other = obj as FieldInfo;
         if (other != null)
            return m_fieldImpl.Equals(other);

         return false;
      }

      public override int GetHashCode()
      {
         return m_fieldImpl.GetHashCode();
      }

      public override Type[] GetOptionalCustomModifiers()
      {
         return m_fieldImpl.GetOptionalCustomModifiers();
      }

      public override object GetRawConstantValue()
      {
         return m_fieldImpl.GetRawConstantValue();
      }

      public override Type[] GetRequiredCustomModifiers()
      {
         return m_fieldImpl.GetRequiredCustomModifiers();
      }

      public override object GetValueDirect(TypedReference obj)
      {
         return m_fieldImpl.GetValueDirect(obj);
      }

      public override void SetValueDirect(TypedReference obj, object value)
      {
         m_fieldImpl.SetValueDirect(obj, value);
      }

      public override IList<CustomAttributeData> GetCustomAttributesData()
      {
         return m_fieldImpl.GetCustomAttributesData();
      }      
   }
}
