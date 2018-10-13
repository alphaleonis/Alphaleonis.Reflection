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
   /// Wraps a <see cref="PropertyInfo"/> instance and delegates all method calls to that
   /// <see cref="PropertyInfo" />.
   /// </summary>
   /// <remarks>
   /// <para>Derive from this type and override only those members you have to provide customization
   /// in.</para>
   /// <para><see cref="DelegatingPropertyInfo"/> derives from <see cref="PropertyInfo"/> and
   /// implements most
   /// of the properties and methods of <see cref="PropertyInfo"/>. For
   ///       each member it implements, <see cref="DelegatingPropertyInfo"/> automatically delegates
   ///       to the corresponding member of the internal <see cref="PropertyInfo"/>
   ///       object which was supplied as the argument to the constructor.  This internal
   ///       <see cref="PropertyInfo"/> is exposed to deriving classes by the
   ///       <see cref="UnderlyingProperty"/> property.</para>
   /// </remarks>
   public class DelegatingPropertyInfo : PropertyInfo
   {
      private readonly PropertyInfo m_propertyImpl;

      /// <summary>Constructor.</summary>
      /// <param name="property">The property to delegate all operations to.</param>
      public DelegatingPropertyInfo(PropertyInfo property)
      {
         m_propertyImpl = property;
      }

      /// <inheritdoc/>
      public override PropertyAttributes Attributes => m_propertyImpl.Attributes;

      /// <inheritdoc/>
      public override bool CanRead => m_propertyImpl.CanRead;

      /// <inheritdoc/>
      public override bool CanWrite => m_propertyImpl.CanWrite;

      /// <inheritdoc/>
      public override Type DeclaringType => m_propertyImpl.DeclaringType;

      /// <inheritdoc/>
      public override int MetadataToken => m_propertyImpl.MetadataToken;

      /// <inheritdoc/>
      public override Module Module => m_propertyImpl.Module;

      /// <inheritdoc/>
      public override string Name => m_propertyImpl.Name;

      /// <inheritdoc/>
      public override Type PropertyType => m_propertyImpl.PropertyType;

      /// <inheritdoc/>
      public override Type ReflectedType => m_propertyImpl.ReflectedType;

      
      /// <summary>The underlying property that was passed to the constructor.</summary>
      public PropertyInfo UnderlyingProperty => m_propertyImpl;

      /// <inheritdoc/>
      public override MethodInfo[] GetAccessors(bool nonPublic) => m_propertyImpl.GetAccessors(nonPublic);

      /// <inheritdoc/>
      public override object GetConstantValue() => m_propertyImpl.GetConstantValue();

      /// <inheritdoc/>
      public override object[] GetCustomAttributes(bool inherit) => m_propertyImpl.GetCustomAttributes(inherit);

      /// <inheritdoc/>
      public override object[] GetCustomAttributes(Type attributeType, bool inherit) => m_propertyImpl.GetCustomAttributes(attributeType, inherit);

      /// <inheritdoc/>
      public override IList<CustomAttributeData> GetCustomAttributesData() => m_propertyImpl.GetCustomAttributesData();

      /// <inheritdoc/>
      public override MethodInfo GetGetMethod(bool nonPublic) => m_propertyImpl.GetGetMethod(nonPublic);

      /// <inheritdoc/>
      public override ParameterInfo[] GetIndexParameters() => m_propertyImpl.GetIndexParameters();

      /// <inheritdoc/>
      public override Type[] GetOptionalCustomModifiers() => m_propertyImpl.GetOptionalCustomModifiers();

      /// <inheritdoc/>
      public override object GetRawConstantValue() => m_propertyImpl.GetRawConstantValue();

      /// <inheritdoc/>
      public override Type[] GetRequiredCustomModifiers() => m_propertyImpl.GetRequiredCustomModifiers();

      /// <inheritdoc/>
      public override MethodInfo GetSetMethod(bool nonPublic) => m_propertyImpl.GetSetMethod(nonPublic);

      /// <inheritdoc/>
      public override object GetValue(object obj, object[] index) => m_propertyImpl.GetValue(obj, index);

      /// <inheritdoc/>
      public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture) => m_propertyImpl.GetValue(obj, invokeAttr, binder, index, culture);

      /// <inheritdoc/>
      public override bool IsDefined(Type attributeType, bool inherit) => m_propertyImpl.IsDefined(attributeType, inherit);

      /// <inheritdoc/>
      public override void SetValue(object obj, object value, object[] index) => m_propertyImpl.SetValue(obj, value, index);

      /// <inheritdoc/>
      public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, CultureInfo culture) => m_propertyImpl.SetValue(obj, value, invokeAttr, binder, index, culture);

      /// <inheritdoc/>
      public override string ToString() => m_propertyImpl.ToString();

      /// <inheritdoc/>
      public override int GetHashCode() => m_propertyImpl.GetHashCode();

      /// <inheritdoc/>
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
