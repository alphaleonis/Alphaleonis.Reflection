// Copyright (c) Peter Palotas
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Alphaleonis.Reflection
{
   /// <summary>Wraps a <see cref="Type"/> and delegates all methods to that Type.</summary>
   /// <remarks>
   /// This class extends <see cref="TypeDelegator"/> which is missing several overrides to delegate
   /// method calls to the wrapped type.
   /// </remarks>
   public class DelegatingType : TypeDelegator
   {
      /// <summary>Constructor.</summary>
      /// <param name="delegatingType">The <see cref="Type"/> to delegate all calls to.</param>
      public DelegatingType(Type delegatingType)
         : base(delegatingType)
      {
      }

      /// <inheritdoc/>
      public override bool ContainsGenericParameters => typeImpl.ContainsGenericParameters;

      /// <inheritdoc/>
      public override IEnumerable<CustomAttributeData> CustomAttributes => typeImpl.CustomAttributes;

      /// <inheritdoc/>
      public override MethodBase DeclaringMethod => typeImpl.DeclaringMethod;

      /// <inheritdoc/>
      public override Type[] FindInterfaces(TypeFilter filter, object filterCriteria) => typeImpl.FindInterfaces(filter, filterCriteria);

      /// <inheritdoc/>
      public override MemberInfo[] FindMembers(MemberTypes memberType, BindingFlags bindingAttr, MemberFilter filter, object filterCriteria) => typeImpl.FindMembers(memberType, bindingAttr, filter, filterCriteria);

      /// <inheritdoc/>
      public override Type[] GenericTypeArguments => typeImpl.GenericTypeArguments;

      /// <inheritdoc/>
      public override Type GetEnumUnderlyingType() => typeImpl.GetEnumUnderlyingType();

      /// <inheritdoc/>
      public override bool IsEnumDefined(object value) => typeImpl.IsEnumDefined(value);

      /// <inheritdoc/>
      public override bool IsInstanceOfType(object o) => typeImpl.IsInstanceOfType(o);

      /// <inheritdoc/>
      public override bool IsSubclassOf(Type c) => typeImpl.IsSubclassOf(c);

      /// <inheritdoc/>
      public override bool IsEnum => typeImpl.IsEnum;

      /// <inheritdoc/>
      public override Type DeclaringType => typeImpl.DeclaringType;

      /// <inheritdoc/>
      public override GenericParameterAttributes GenericParameterAttributes => typeImpl.GenericParameterAttributes;

      /// <inheritdoc/>
      public override int GenericParameterPosition => typeImpl.GenericParameterPosition;

      /// <inheritdoc/>
      public override bool IsConstructedGenericType => typeImpl.IsConstructedGenericType;

      /// <inheritdoc/>
      public override bool IsGenericParameter => typeImpl.IsGenericParameter;

      /// <inheritdoc/>
      public override bool IsGenericType => typeImpl.IsGenericType;

      /// <inheritdoc/>
      public override bool IsGenericTypeDefinition => typeImpl.IsGenericTypeDefinition;

      /// <inheritdoc/>
      public override bool IsSecurityCritical => typeImpl.IsSecurityCritical;

      /// <inheritdoc/>
      public override bool IsSecuritySafeCritical => typeImpl.IsSecuritySafeCritical;

      /// <inheritdoc/>
      public override bool IsSecurityTransparent => typeImpl.IsSecurityTransparent;

      /// <inheritdoc/>
      public override Type ReflectedType => typeImpl.ReflectedType;

      /// <inheritdoc/>
      public override StructLayoutAttribute StructLayoutAttribute => typeImpl.StructLayoutAttribute;

      /// <inheritdoc/>
      public override RuntimeTypeHandle TypeHandle => typeImpl.TypeHandle;

      /// <inheritdoc/>
      public override int GetArrayRank() => typeImpl.GetArrayRank();

      /// <inheritdoc/>
      public override IList<CustomAttributeData> GetCustomAttributesData() => typeImpl.GetCustomAttributesData();

      /// <inheritdoc/>
      public override MemberInfo[] GetDefaultMembers() => typeImpl.GetDefaultMembers();

      /// <inheritdoc/>
      public override string GetEnumName(object value) => typeImpl.GetEnumName(value);

      /// <inheritdoc/>
      public override string[] GetEnumNames() => typeImpl.GetEnumNames();

      /// <inheritdoc/>
      public override Array GetEnumValues() => typeImpl.GetEnumValues();

      /// <inheritdoc/>
      public override Type[] GetGenericArguments() => typeImpl.GetGenericArguments();

      /// <inheritdoc/>
      public override Type[] GetGenericParameterConstraints() => typeImpl.GetGenericParameterConstraints();

      /// <inheritdoc/>
      public override Type GetGenericTypeDefinition() => typeImpl.GetGenericTypeDefinition();

      /// <inheritdoc/>
      public override int GetHashCode() => typeImpl.GetHashCode();

      /// <inheritdoc/>
      public override InterfaceMapping GetInterfaceMap(Type interfaceType) => typeImpl.GetInterfaceMap(interfaceType);

      /// <inheritdoc/>
      public override MemberInfo[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr) => typeImpl.GetMember(name, type, bindingAttr);

      /// <inheritdoc/>
      public override Type MakeArrayType() => typeImpl.MakeArrayType();

      /// <inheritdoc/>
      public override Type MakeArrayType(int rank) => typeImpl.MakeArrayType(rank);

      /// <inheritdoc/>
      public override Type MakeByRefType() => typeImpl.MakeByRefType();

      /// <inheritdoc/>
      public override Type MakeGenericType(params Type[] typeArguments) => typeImpl.MakeGenericType(typeArguments);

      /// <inheritdoc/>
      public override Type MakePointerType() => typeImpl.MakePointerType();

      /// <inheritdoc/>
      public override bool IsSerializable => typeImpl.IsSerializable;

      /// <inheritdoc/>
      public override string ToString() => typeImpl.ToString();

      /// <inheritdoc/>
      public override MemberTypes MemberType => typeImpl.MemberType;

      /// <inheritdoc/>
      public override bool Equals(object o)
      {
         DelegatingType other = o as DelegatingType;
         if (other != null)
            return other.typeImpl.Equals(typeImpl);
         else
            return typeImpl.Equals(o);
      }

      /// <inheritdoc/>
      public override bool Equals(Type o)
      {
         DelegatingType other = o as DelegatingType;
         if (other != null)
            return other.typeImpl.Equals(typeImpl);
         else
            return typeImpl.Equals(o);
      }

      /// <inheritdoc/>
      public override bool IsEquivalentTo(Type other)
      {
         DelegatingType del = other as DelegatingType;
         if (del != null)
            return del.typeImpl.IsEquivalentTo(typeImpl);
         else
            return typeImpl.IsEquivalentTo(other);
      }      
   }
}
