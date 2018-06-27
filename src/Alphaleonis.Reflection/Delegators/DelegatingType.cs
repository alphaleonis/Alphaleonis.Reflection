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
      public DelegatingType(Type delegatingType)
         : base(delegatingType)
      {
      }
      
      public override bool ContainsGenericParameters => typeImpl.ContainsGenericParameters;

      public override IEnumerable<CustomAttributeData> CustomAttributes => typeImpl.CustomAttributes;

      public override MethodBase DeclaringMethod => typeImpl.DeclaringMethod;

      public override Type[] FindInterfaces(TypeFilter filter, object filterCriteria) => typeImpl.FindInterfaces(filter, filterCriteria);

      public override MemberInfo[] FindMembers(MemberTypes memberType, BindingFlags bindingAttr, MemberFilter filter, object filterCriteria) => typeImpl.FindMembers(memberType, bindingAttr, filter, filterCriteria);

      public override Type[] GenericTypeArguments => typeImpl.GenericTypeArguments;

      public override Type GetEnumUnderlyingType() => typeImpl.GetEnumUnderlyingType();

      public override bool IsEnumDefined(object value) => typeImpl.IsEnumDefined(value);

      public override bool IsInstanceOfType(object o) => typeImpl.IsInstanceOfType(o);

      public override bool IsSubclassOf(Type c) => typeImpl.IsSubclassOf(c);

      public override bool IsEnum => typeImpl.IsEnum;

      public override Type DeclaringType => typeImpl.DeclaringType;
      
      public override GenericParameterAttributes GenericParameterAttributes => typeImpl.GenericParameterAttributes;

      public override int GenericParameterPosition => typeImpl.GenericParameterPosition;

      public override bool IsConstructedGenericType => typeImpl.IsConstructedGenericType;

      public override bool IsGenericParameter => typeImpl.IsGenericParameter;

      public override bool IsGenericType => typeImpl.IsGenericType;

      public override bool IsGenericTypeDefinition => typeImpl.IsGenericTypeDefinition;

      public override bool IsSecurityCritical => typeImpl.IsSecurityCritical;

      public override bool IsSecuritySafeCritical => typeImpl.IsSecuritySafeCritical;

      public override bool IsSecurityTransparent => typeImpl.IsSecurityTransparent;

      public override Type ReflectedType => typeImpl.ReflectedType;

      public override StructLayoutAttribute StructLayoutAttribute => typeImpl.StructLayoutAttribute;

      public override RuntimeTypeHandle TypeHandle => typeImpl.TypeHandle;

      public override int GetArrayRank() => typeImpl.GetArrayRank();

      public override IList<CustomAttributeData> GetCustomAttributesData() => typeImpl.GetCustomAttributesData();

      public override MemberInfo[] GetDefaultMembers() => typeImpl.GetDefaultMembers();

      public override string GetEnumName(object value) => typeImpl.GetEnumName(value);

      public override string[] GetEnumNames() => typeImpl.GetEnumNames();

      public override Array GetEnumValues() => typeImpl.GetEnumValues();

      public override Type[] GetGenericArguments() => typeImpl.GetGenericArguments();

      public override Type[] GetGenericParameterConstraints() => typeImpl.GetGenericParameterConstraints();

      public override Type GetGenericTypeDefinition() => typeImpl.GetGenericTypeDefinition();

      public override int GetHashCode() => typeImpl.GetHashCode();

      public override InterfaceMapping GetInterfaceMap(Type interfaceType) => typeImpl.GetInterfaceMap(interfaceType);

      public override MemberInfo[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr) => typeImpl.GetMember(name, type, bindingAttr);

      public override Type MakeArrayType() => typeImpl.MakeArrayType();

      public override Type MakeArrayType(int rank) => typeImpl.MakeArrayType(rank);

      public override Type MakeByRefType() => typeImpl.MakeByRefType();

      public override Type MakeGenericType(params Type[] typeArguments) => typeImpl.MakeGenericType(typeArguments);

      public override Type MakePointerType() => typeImpl.MakePointerType();

      public override bool IsSerializable => typeImpl.IsSerializable;

      public override string ToString() => typeImpl.ToString();

      public override MemberTypes MemberType => typeImpl.MemberType;

      public override bool Equals(object o)
      {
         DelegatingType other = o as DelegatingType;
         if (other != null)
            return other.typeImpl.Equals(typeImpl);
         else
            return typeImpl.Equals(o);
      }

      public override bool Equals(Type o)
      {
         DelegatingType other = o as DelegatingType;
         if (other != null)
            return other.typeImpl.Equals(typeImpl);
         else
            return typeImpl.Equals(o);
      }

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
