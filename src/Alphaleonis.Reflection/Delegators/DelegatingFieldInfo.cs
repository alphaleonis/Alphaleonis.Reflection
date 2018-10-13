// Copyright (c) Peter Palotas
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

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
   /// <para><see cref="DelegatingFieldInfo"/> derives from <see cref="FieldInfo"/> and implements
   /// most
   /// of the properties and methods of <see cref="FieldInfo"/>. For
   ///       each member it implements, <see cref="DelegatingFieldInfo"/> automatically delegates to
   ///       the corresponding member of the internal <see cref="FieldInfo"/>
   ///       object which was supplied as the argument to the constructor.  This internal
   ///       <see cref="FieldInfo"/> is exposed to deriving classes by the
   ///       <see cref="UnderlyingField"/> property.</para>
   /// </remarks>
   public class DelegatingFieldInfo : FieldInfo
   {
      private readonly FieldInfo m_fieldImpl;

      /// <summary>Constructor.</summary>
      /// <param name="field">The field to delegate all calls to.</param>
      public DelegatingFieldInfo(FieldInfo field)
      {
         m_fieldImpl = field;
      }

      /// <inheritdoc/>
      public override FieldAttributes Attributes => m_fieldImpl.Attributes;

      /// <inheritdoc/>
      public override Type DeclaringType => m_fieldImpl.DeclaringType;

      /// <inheritdoc/>
      public override RuntimeFieldHandle FieldHandle => m_fieldImpl.FieldHandle;

      /// <inheritdoc/>
      public override Type FieldType => m_fieldImpl.FieldType;

      /// <inheritdoc/>
      public override bool IsSecurityCritical => m_fieldImpl.IsSecurityCritical;

      /// <inheritdoc/>
      public override bool IsSecuritySafeCritical => m_fieldImpl.IsSecuritySafeCritical;

      /// <inheritdoc/>
      public override bool IsSecurityTransparent => m_fieldImpl.IsSecurityTransparent;

      /// <inheritdoc/>
      public override int MetadataToken => m_fieldImpl.MetadataToken;

      /// <inheritdoc/>
      public override Module Module => m_fieldImpl.Module;

      /// <inheritdoc/>
      public override string Name => m_fieldImpl.Name;

      /// <inheritdoc/>
      public override Type ReflectedType => m_fieldImpl.ReflectedType;

      /// <summary>The underlying field passed to the constructor.</summary>
      public FieldInfo UnderlyingField => m_fieldImpl;

      /// <inheritdoc/>
      public override object[] GetCustomAttributes(bool inherit) => m_fieldImpl.GetCustomAttributes(inherit);

      /// <inheritdoc/>
      public override object[] GetCustomAttributes(Type attributeType, bool inherit) => m_fieldImpl.GetCustomAttributes(attributeType, inherit);

      /// <inheritdoc/>
      public override IList<CustomAttributeData> GetCustomAttributesData() => m_fieldImpl.GetCustomAttributesData();

      /// <inheritdoc/>
      public override Type[] GetOptionalCustomModifiers() => m_fieldImpl.GetOptionalCustomModifiers();

      /// <inheritdoc/>
      public override object GetRawConstantValue() => m_fieldImpl.GetRawConstantValue();

      /// <inheritdoc/>
      public override Type[] GetRequiredCustomModifiers() => m_fieldImpl.GetRequiredCustomModifiers();

      /// <inheritdoc/>
      public override object GetValue(object obj) => m_fieldImpl.GetValue(obj);

      /// <inheritdoc/>
      public override object GetValueDirect(TypedReference obj) => m_fieldImpl.GetValueDirect(obj);

      /// <inheritdoc/>
      public override bool IsDefined(Type attributeType, bool inherit) => m_fieldImpl.IsDefined(attributeType, inherit);

      /// <inheritdoc/>
      public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, CultureInfo culture) => m_fieldImpl.SetValue(obj, value, invokeAttr, binder, culture);

      /// <inheritdoc/>
      public override void SetValueDirect(TypedReference obj, object value) => m_fieldImpl.SetValueDirect(obj, value);

      /// <inheritdoc/>
      public override string ToString() => m_fieldImpl.ToString();

      /// <inheritdoc/>
      public override int GetHashCode() => m_fieldImpl.GetHashCode();

      /// <inheritdoc/>
      public override bool Equals(object obj)
      {
         DelegatingFieldInfo delegatingFieldInfo = obj as DelegatingFieldInfo;
         if (delegatingFieldInfo != null)
            return delegatingFieldInfo.m_fieldImpl.Equals(m_fieldImpl);
         else
            return m_fieldImpl.Equals(obj);
      }

      /// <inheritdoc/>
      public override IEnumerable<CustomAttributeData> CustomAttributes => m_fieldImpl.CustomAttributes;
   }

   
}
