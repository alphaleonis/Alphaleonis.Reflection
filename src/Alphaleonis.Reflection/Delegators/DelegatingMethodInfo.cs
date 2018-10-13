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
   /// Wraps a <see cref="MethodInfo"/> instance and delegates all method calls to that
   /// <see cref="MethodInfo" />.
   /// </summary>
   /// <remarks>
   /// <para>Derive from this type and override only those members you have to provide customization
   /// in.</para>
   /// <para><see cref="DelegatingMethodInfo"/> derives from <see cref="MethodInfo"/> and implements
   /// most
   /// of the properties and methods of <see cref="MethodInfo"/>. For
   ///       each member it implements, <see cref="DelegatingMethodInfo"/> automatically delegates to
   ///       the corresponding member of the internal <see cref="MethodInfo"/>
   ///       object which was supplied as the argument to the constructor.  This internal
   ///       <see cref="MethodInfo"/> is exposed to deriving classes by the
   ///       <see cref="UnderlyingMethod"/> property.</para>
   /// </remarks>
   public class DelegatingMethodInfo : MethodInfo
   {
      private readonly MethodInfo m_methodImpl;

      /// <summary>Constructor.</summary>
      /// <param name="method">The method to delegate all calls to.</param>
      public DelegatingMethodInfo(MethodInfo method)
      {
         m_methodImpl = method;
      }

      /// <inheritdoc/>
      public override MethodAttributes Attributes => m_methodImpl.Attributes;

      /// <inheritdoc/>
      public override CallingConventions CallingConvention => m_methodImpl.CallingConvention;

      /// <inheritdoc/>
      public override bool ContainsGenericParameters => m_methodImpl.ContainsGenericParameters;

      /// <inheritdoc/>
      public override Type DeclaringType => m_methodImpl.DeclaringType;

      /// <inheritdoc/>
      public override bool IsGenericMethod => m_methodImpl.IsGenericMethod;

      /// <inheritdoc/>
      public override bool IsGenericMethodDefinition => m_methodImpl.IsGenericMethodDefinition;

      /// <inheritdoc/>
      public override bool IsSecurityCritical => m_methodImpl.IsSecurityCritical;

      /// <inheritdoc/>
      public override bool IsSecuritySafeCritical => m_methodImpl.IsSecuritySafeCritical;

      /// <inheritdoc/>
      public override bool IsSecurityTransparent => m_methodImpl.IsSecurityTransparent;

      /// <inheritdoc/>
      public override int MetadataToken => m_methodImpl.MetadataToken;

      /// <inheritdoc/>
      public override RuntimeMethodHandle MethodHandle => m_methodImpl.MethodHandle;

      /// <inheritdoc/>
      public override Module Module => m_methodImpl.Module;

      /// <inheritdoc/>
      public override string Name => m_methodImpl.Name;

      /// <inheritdoc/>
      public override Type ReflectedType => m_methodImpl.ReflectedType;

      /// <inheritdoc/>
      public override ParameterInfo ReturnParameter => m_methodImpl.ReturnParameter;

      /// <inheritdoc/>
      public override Type ReturnType => m_methodImpl.ReturnType;

      /// <inheritdoc/>
      public override ICustomAttributeProvider ReturnTypeCustomAttributes => m_methodImpl.ReturnTypeCustomAttributes;

      /// <inheritdoc/>
      public MethodInfo UnderlyingMethod => m_methodImpl;

      /// <inheritdoc/>
      public override Delegate CreateDelegate(Type delegateType) => m_methodImpl.CreateDelegate(delegateType);

      /// <inheritdoc/>
      public override Delegate CreateDelegate(Type delegateType, object target) => m_methodImpl.CreateDelegate(delegateType, target);

      /// <inheritdoc/>
      public override MethodInfo GetBaseDefinition() => m_methodImpl.GetBaseDefinition();

      /// <inheritdoc/>
      public override object[] GetCustomAttributes(bool inherit) => m_methodImpl.GetCustomAttributes(inherit);

      /// <inheritdoc/>
      public override object[] GetCustomAttributes(Type attributeType, bool inherit) => m_methodImpl.GetCustomAttributes(attributeType, inherit);

      /// <inheritdoc/>
      public override IList<CustomAttributeData> GetCustomAttributesData() => m_methodImpl.GetCustomAttributesData();

      /// <inheritdoc/>
      public override Type[] GetGenericArguments() => m_methodImpl.GetGenericArguments();

      /// <inheritdoc/>
      public override MethodInfo GetGenericMethodDefinition() => m_methodImpl.GetGenericMethodDefinition();

      /// <inheritdoc/>
      public override MethodBody GetMethodBody() => m_methodImpl.GetMethodBody();

      /// <inheritdoc/>
      public override MethodImplAttributes GetMethodImplementationFlags() => m_methodImpl.GetMethodImplementationFlags();

      /// <inheritdoc/>
      public override ParameterInfo[] GetParameters() => m_methodImpl.GetParameters();

      /// <inheritdoc/>
      public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture) => m_methodImpl.Invoke(obj, invokeAttr, binder, parameters, culture);

      /// <inheritdoc/>
      public override bool IsDefined(Type attributeType, bool inherit) => m_methodImpl.IsDefined(attributeType, inherit);

      /// <inheritdoc/>
      public override MethodInfo MakeGenericMethod(params Type[] typeArguments) => m_methodImpl.MakeGenericMethod(typeArguments);

      /// <inheritdoc/>
      public override string ToString() => m_methodImpl.ToString();

      /// <inheritdoc/>
      public override int GetHashCode() => m_methodImpl.GetHashCode();

      /// <inheritdoc/>
      public override bool Equals(object obj) 
      {
         DelegatingMethodInfo other = obj as DelegatingMethodInfo;
         if (other != null)
            return other.m_methodImpl.Equals(m_methodImpl);
         else
            return m_methodImpl.Equals(obj);
      }

      /// <inheritdoc/>
      public override IEnumerable<CustomAttributeData> CustomAttributes => m_methodImpl.CustomAttributes;

      /// <inheritdoc/>
      public override MethodImplAttributes MethodImplementationFlags => m_methodImpl.MethodImplementationFlags;
   }

   
}
