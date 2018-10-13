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
   /// Wraps a <see cref="ConstructorInfo"/> instance and delegates all method calls to that
   /// <see cref="ConstructorInfo" />.
   /// </summary>
   /// <remarks>
   /// <para>Derive from this type and override only those members you have to provide customization
   /// in.</para>
   /// <para><see cref="DelegatingConstructorInfo"/> derives from <see cref="ConstructorInfo"/> and
   /// implements most
   /// of the properties and methods of <see cref="ConstructorInfo"/>. For
   ///       each member it implements, <see cref="DelegatingConstructorInfo"/> automatically
   ///       delegates to the corresponding member of the internal <see cref="ConstructorInfo"/>
   ///       object which was supplied as the argument to the constructor.  This internal
   ///       <see cref="ConstructorInfo"/> is exposed to deriving classes by the
   ///       <see cref="UnderlyingConstructor"/> property.</para>  
   /// </remarks>
   public class DelegatingConstructorInfo : ConstructorInfo
   {
      private readonly ConstructorInfo m_constructorImpl;

      /// <summary>Constructor.</summary>
      /// <param name="constructor">The <see cref="ConstructorInfo"/> to delegate all calls to.</param>
      public DelegatingConstructorInfo(ConstructorInfo constructor)
      {
         m_constructorImpl = constructor;
      }

      /// <inheritdoc/>
      public override MethodAttributes Attributes => m_constructorImpl.Attributes;

      /// <inheritdoc/>
      public override CallingConventions CallingConvention => m_constructorImpl.CallingConvention;

      /// <inheritdoc/>
      public override bool ContainsGenericParameters => m_constructorImpl.ContainsGenericParameters;

      /// <inheritdoc/>
      public override Type DeclaringType => m_constructorImpl.DeclaringType;

      /// <inheritdoc/>
      public override bool IsGenericMethod => m_constructorImpl.IsGenericMethod;

      /// <inheritdoc/>
      public override bool IsGenericMethodDefinition => m_constructorImpl.IsGenericMethodDefinition;

      /// <inheritdoc/>
      public override bool IsSecurityCritical => m_constructorImpl.IsSecurityCritical;

      /// <inheritdoc/>
      public override bool IsSecuritySafeCritical => m_constructorImpl.IsSecuritySafeCritical;

      /// <inheritdoc/>
      public override bool IsSecurityTransparent => m_constructorImpl.IsSecurityTransparent;

      /// <inheritdoc/>
      public override int MetadataToken => m_constructorImpl.MetadataToken;

      /// <inheritdoc/>
      public override RuntimeMethodHandle MethodHandle => m_constructorImpl.MethodHandle;

      /// <inheritdoc/>
      public override Module Module => m_constructorImpl.Module;

      /// <inheritdoc/>
      public override string Name => m_constructorImpl.Name;

      /// <inheritdoc/>
      public override Type ReflectedType => m_constructorImpl.ReflectedType;

      /// <summary>The underlying <see cref="ConstructorInfo"/> that was passed to the constructor.</summary>
      public ConstructorInfo UnderlyingConstructor => m_constructorImpl;

      /// <inheritdoc/>
      public override object[] GetCustomAttributes(bool inherit) => m_constructorImpl.GetCustomAttributes(inherit);

      /// <inheritdoc/>
      public override object[] GetCustomAttributes(Type attributeType, bool inherit) => m_constructorImpl.GetCustomAttributes(attributeType, inherit);

      /// <inheritdoc/>
      public override IList<CustomAttributeData> GetCustomAttributesData() => m_constructorImpl.GetCustomAttributesData();

      /// <inheritdoc/>
      public override Type[] GetGenericArguments() => m_constructorImpl.GetGenericArguments();

      /// <inheritdoc/>
      public override MethodBody GetMethodBody() => m_constructorImpl.GetMethodBody();

      /// <inheritdoc/>
      public override MethodImplAttributes GetMethodImplementationFlags() => m_constructorImpl.GetMethodImplementationFlags();

      /// <inheritdoc/>
      public override ParameterInfo[] GetParameters() => m_constructorImpl.GetParameters();

      /// <inheritdoc/>
      public override object Invoke(BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture) => m_constructorImpl.Invoke(invokeAttr, binder, parameters, culture);

      /// <inheritdoc/>
      public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture) => m_constructorImpl.Invoke(obj, invokeAttr, binder, parameters, culture);

      /// <inheritdoc/>
      public override bool IsDefined(Type attributeType, bool inherit) => m_constructorImpl.IsDefined(attributeType, inherit);

      /// <inheritdoc/>
      public override string ToString() => m_constructorImpl.ToString();

      /// <inheritdoc/>
      public override IEnumerable<CustomAttributeData> CustomAttributes => m_constructorImpl.CustomAttributes;

      /// <inheritdoc/>
      public override bool Equals(object obj)
      {
         DelegatingConstructorInfo other = obj as DelegatingConstructorInfo;
         if (other != null)
            return m_constructorImpl.Equals(other.m_constructorImpl);
         else
            return m_constructorImpl.Equals(obj);
      }

      /// <inheritdoc/>
      public override int GetHashCode() => m_constructorImpl.GetHashCode();

      /// <inheritdoc/>
      public override MemberTypes MemberType => m_constructorImpl.MemberType;

      /// <inheritdoc/>
      public override MethodImplAttributes MethodImplementationFlags => m_constructorImpl.MethodImplementationFlags;
   }
}
