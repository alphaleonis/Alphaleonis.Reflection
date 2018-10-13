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
   /// <para><see cref="DelegatingParameterInfo"/> derives from <see cref="ParameterInfo"/> and
   /// implements most
   /// of the properties and methods of <see cref="ParameterInfo"/>. For
   ///       each member it implements, <see cref="DelegatingParameterInfo"/> automatically delegates
   ///       to the corresponding member of the internal <see cref="ParameterInfo"/>
   ///       object which was supplied as the argument to the constructor.  This internal
   ///       <see cref="ParameterInfo"/> is exposed to deriving classes by the
   ///       <see cref="UnderlyingParameter"/> property.</para>
   /// </remarks>
   public class DelegatingParameterInfo : ParameterInfo
   { 
      private readonly ParameterInfo m_parameterImpl;

      /// <summary>Constructor.</summary>
      /// <param name="parameter">The underlying parameter to delegate all calls to.</param>
      public DelegatingParameterInfo(ParameterInfo parameter)
      {
         m_parameterImpl = parameter;
      }

      /// <inheritdoc/>
      public override ParameterAttributes Attributes => m_parameterImpl.Attributes;

      /// <inheritdoc/>
      public override object DefaultValue => m_parameterImpl.DefaultValue;

      /// <inheritdoc/>
      public override MemberInfo Member => m_parameterImpl.Member;

      /// <inheritdoc/>
      public override int MetadataToken => m_parameterImpl.MetadataToken;

      /// <inheritdoc/>
      public override string Name => m_parameterImpl.Name;

      /// <inheritdoc/>
      public override Type ParameterType => m_parameterImpl.ParameterType;

      /// <inheritdoc/>
      public override int Position => m_parameterImpl.Position;

      /// <inheritdoc/>
      public override object RawDefaultValue => m_parameterImpl.RawDefaultValue;

      /// <inheritdoc/>
      public ParameterInfo UnderlyingParameter => m_parameterImpl;

      /// <inheritdoc/>
      public override object[] GetCustomAttributes(bool inherit) => m_parameterImpl.GetCustomAttributes(inherit);

      /// <inheritdoc/>
      public override object[] GetCustomAttributes(Type attributeType, bool inherit) => m_parameterImpl.GetCustomAttributes(attributeType, inherit);

      /// <inheritdoc/>
      public override IList<CustomAttributeData> GetCustomAttributesData() => m_parameterImpl.GetCustomAttributesData();

      /// <inheritdoc/>
      public override Type[] GetOptionalCustomModifiers() => m_parameterImpl.GetOptionalCustomModifiers();

      /// <inheritdoc/>
      public override Type[] GetRequiredCustomModifiers() => m_parameterImpl.GetRequiredCustomModifiers();

      /// <inheritdoc/>
      public override bool IsDefined(Type attributeType, bool inherit) => m_parameterImpl.IsDefined(attributeType, inherit);

      /// <inheritdoc/>
      public override string ToString() => m_parameterImpl.ToString();

      /// <inheritdoc/>
      public override int GetHashCode() => m_parameterImpl.GetHashCode();

      /// <inheritdoc/>
      public override bool Equals(object obj)
      {
         DelegatingParameterInfo other = obj as DelegatingParameterInfo;
         if (other != null)
            return other.m_parameterImpl.Equals(m_parameterImpl);
         else
            return m_parameterImpl.Equals(obj);
      }

      /// <inheritdoc/>
      public override IEnumerable<CustomAttributeData> CustomAttributes => m_parameterImpl.CustomAttributes;

      /// <inheritdoc/>
      public override bool HasDefaultValue => m_parameterImpl.HasDefaultValue;
   }
}
