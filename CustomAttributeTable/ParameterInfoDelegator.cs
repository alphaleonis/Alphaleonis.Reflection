using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace CustomAttributeTable
{

   public class ParameterInfoDelegator : ParameterInfo
   {
      private readonly ParameterInfo m_parameterInfo;

      public ParameterInfoDelegator(ParameterInfo parameterInfo)
      {
         m_parameterInfo = parameterInfo;
      }

      #region Properties

      public override object DefaultValue
      {
         get { return m_parameterInfo.DefaultValue; }
      }

      public override string Name
      {
         get { return m_parameterInfo.Name; }
      }

      public override MemberInfo Member
      {
         get { return m_parameterInfo.Member; }
      }

      public override ParameterAttributes Attributes
      {
         get { return m_parameterInfo.Attributes; }
      }

      public override IEnumerable<CustomAttributeData> CustomAttributes
      {
         get { return m_parameterInfo.CustomAttributes; }
      }

      public override bool HasDefaultValue
      {
         get { return m_parameterInfo.HasDefaultValue; }
      }

      public override int MetadataToken
      {
         get { return m_parameterInfo.MetadataToken; }
      }

      public override Type ParameterType
      {
         get { return m_parameterInfo.ParameterType; }
      }

      public override int Position
      {
         get { return m_parameterInfo.Position; }
      }

      public override object RawDefaultValue
      {
         get { return m_parameterInfo.RawDefaultValue; }
      }

      #endregion

      #region Methods

      public override object[] GetCustomAttributes(bool inherit)
      {
         return m_parameterInfo.GetCustomAttributes(inherit);
      }

      public override object[] GetCustomAttributes(Type attributeType, bool inherit)
      {
         return m_parameterInfo.GetCustomAttributes(attributeType, inherit);
      }

      public override bool IsDefined(Type attributeType, bool inherit)
      {
         return m_parameterInfo.IsDefined(attributeType, inherit);
      }

      public override bool Equals(object obj)
      {
         return m_parameterInfo.Equals(obj);
      }

      public override IList<CustomAttributeData> GetCustomAttributesData()
      {
         return m_parameterInfo.GetCustomAttributesData();
      }

      public override int GetHashCode()
      {
         return m_parameterInfo.GetHashCode();
      }

      public override Type[] GetOptionalCustomModifiers()
      {
         return m_parameterInfo.GetOptionalCustomModifiers();
      }

      public override Type[] GetRequiredCustomModifiers()
      {
         return m_parameterInfo.GetRequiredCustomModifiers();
      }

      public override string ToString()
      {
         return m_parameterInfo.ToString();
      }

      #endregion
   }

      
}
