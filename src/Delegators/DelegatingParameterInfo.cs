using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Alphaleonis.Reflection
{
   public class DelegatingParameterInfo : ParameterInfo
   {
      private readonly ParameterInfo m_parameter;

      public DelegatingParameterInfo(ParameterInfo parameter)
      {
         m_parameter = parameter;
      }

      public override ParameterAttributes Attributes => m_parameter.Attributes;

      public override object DefaultValue => m_parameter.DefaultValue;

      public override MemberInfo Member => m_parameter.Member;

      public override int MetadataToken => m_parameter.MetadataToken;

      public override string Name => m_parameter.Name;

      public override Type ParameterType => m_parameter.ParameterType;

      public override int Position => m_parameter.Position;

      public override object RawDefaultValue => m_parameter.RawDefaultValue;

      public ParameterInfo UnderlyingParameter => m_parameter;

      public override object[] GetCustomAttributes(bool inherit) => m_parameter.GetCustomAttributes(inherit);

      public override object[] GetCustomAttributes(Type attributeType, bool inherit) => m_parameter.GetCustomAttributes(attributeType, inherit);

      public override IList<CustomAttributeData> GetCustomAttributesData() => m_parameter.GetCustomAttributesData();

      public override Type[] GetOptionalCustomModifiers() => m_parameter.GetOptionalCustomModifiers();

      public override Type[] GetRequiredCustomModifiers() => m_parameter.GetRequiredCustomModifiers();

      public override bool IsDefined(Type attributeType, bool inherit) => m_parameter.IsDefined(attributeType, inherit);

      public override string ToString() => m_parameter.ToString();

      public override int GetHashCode() => m_parameter.GetHashCode();

      public override bool Equals(object obj)
      {
         DelegatingParameterInfo other = obj as DelegatingParameterInfo;
         if (other != null)
            return other.m_parameter.Equals(m_parameter);
         else
            return m_parameter.Equals(obj);
      }

      public override IEnumerable<CustomAttributeData> CustomAttributes => m_parameter.CustomAttributes;

      public override bool HasDefaultValue => m_parameter.HasDefaultValue;
   }
}
