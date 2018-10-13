// Copyright (c) Peter Palotas
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Alphaleonis.Reflection
{
   /// <summary>
   /// Wraps a <see cref="ParameterInfo"/> instance and delegates all method calls to that
   /// <see cref="ParameterInfo" />.
   /// </summary>
   /// <remarks>
   /// <para>Derive from this type and override only those members you have to provide customization
   /// in.</para>
   /// <para><see cref="DelegatingParameterInfo"/> derives from <see cref="ParameterInfo"/> and implements most
   /// of the properties and methods of <see cref="ParameterInfo"/>. For
   ///       each member it implements, <see cref="DelegatingParameterInfo"/> automatically delegates to
   ///       the corresponding member of the internal <see cref="ParameterInfo"/>
   ///       object which was supplied as the argument to the constructor.  This internal
   ///       <see cref="ParameterInfo"/> is exposed to deriving classes by the <see langword="protected" />
   ///       <see cref="m_parameterImpl"/> field.</para>
   /// </remarks>
   public class DelegatingParameterInfo : ParameterInfo
   { 
      private readonly ParameterInfo m_parameterImpl;

      public DelegatingParameterInfo(ParameterInfo parameter)
      {
         m_parameterImpl = parameter;
      }

      public override ParameterAttributes Attributes => m_parameterImpl.Attributes;

      public override object DefaultValue => m_parameterImpl.DefaultValue;

      public override MemberInfo Member => m_parameterImpl.Member;

      public override int MetadataToken => m_parameterImpl.MetadataToken;

      public override string Name => m_parameterImpl.Name;

      public override Type ParameterType => m_parameterImpl.ParameterType;

      public override int Position => m_parameterImpl.Position;

      public override object RawDefaultValue => m_parameterImpl.RawDefaultValue;

      public ParameterInfo UnderlyingParameter => m_parameterImpl;

      public override object[] GetCustomAttributes(bool inherit) => m_parameterImpl.GetCustomAttributes(inherit);

      public override object[] GetCustomAttributes(Type attributeType, bool inherit) => m_parameterImpl.GetCustomAttributes(attributeType, inherit);

      public override IList<CustomAttributeData> GetCustomAttributesData() => m_parameterImpl.GetCustomAttributesData();

      public override Type[] GetOptionalCustomModifiers() => m_parameterImpl.GetOptionalCustomModifiers();

      public override Type[] GetRequiredCustomModifiers() => m_parameterImpl.GetRequiredCustomModifiers();

      public override bool IsDefined(Type attributeType, bool inherit) => m_parameterImpl.IsDefined(attributeType, inherit);

      public override string ToString() => m_parameterImpl.ToString();

      public override int GetHashCode() => m_parameterImpl.GetHashCode();

      public override bool Equals(object obj)
      {
         DelegatingParameterInfo other = obj as DelegatingParameterInfo;
         if (other != null)
            return other.m_parameterImpl.Equals(m_parameterImpl);
         else
            return m_parameterImpl.Equals(obj);
      }

      public override IEnumerable<CustomAttributeData> CustomAttributes => m_parameterImpl.CustomAttributes;

      public override bool HasDefaultValue => m_parameterImpl.HasDefaultValue;
   }
}
