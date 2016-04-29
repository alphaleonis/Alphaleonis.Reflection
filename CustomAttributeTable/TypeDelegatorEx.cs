using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace CustomAttributeTable
{

   public class TypeDelegatorEx : TypeDelegator
   {
      public TypeDelegatorEx(Type delegatingType)
         : base(delegatingType)
      {
      }

      public override int GetArrayRank()
      {
         return typeImpl.GetArrayRank();
      }

      public override MemberInfo[] GetDefaultMembers()
      {
         return typeImpl.GetDefaultMembers();
      }

      public override bool IsGenericType => typeImpl.IsGenericType;

      public override bool IsGenericTypeDefinition => typeImpl.IsGenericTypeDefinition;

      public override string GetEnumName(object value)
      {
         return typeImpl.GetEnumName(value);
      }

      public override string[] GetEnumNames()
      {
         return typeImpl.GetEnumNames();
      }

      public override Array GetEnumValues()
      {
         return typeImpl.GetEnumValues();
      }

      public override Type[] GetGenericArguments()
      {
         return typeImpl.GetGenericArguments();
      }

      public override Type[] GetGenericParameterConstraints()
      {
         return typeImpl.GetGenericParameterConstraints();
      }

      public override Type GetGenericTypeDefinition()
      {
         return typeImpl.GetGenericTypeDefinition();
      }

      public override int GetHashCode()
      {
         return typeImpl.GetHashCode();
      }

      public override InterfaceMapping GetInterfaceMap(Type interfaceType)
      {
         return typeImpl.GetInterfaceMap(interfaceType);
      }

      public override MemberInfo[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr)
      {
         return typeImpl.GetMember(name, type, bindingAttr);
      }

      public override Type MakeArrayType()
      {
         return typeImpl.MakeArrayType();
      }

      public override Type MakeArrayType(int rank)
      {
         return typeImpl.MakeArrayType(rank);
      }

      public override Type MakeByRefType()
      {
         return typeImpl.MakeByRefType();
      }

      public override Type MakeGenericType(params Type[] typeArguments)
      {
         return typeImpl.MakeGenericType(typeArguments);
      }

      public override Type MakePointerType()
      {
         return typeImpl.MakePointerType();
      }

      public override string ToString()
      {
         return typeImpl.ToString();
      }

      public override Type DeclaringType
      {
         get
         {
            return typeImpl.DeclaringType;
         }
      }

      public override GenericParameterAttributes GenericParameterAttributes
      {
         get
         {
            return typeImpl.GenericParameterAttributes;
         }
      }

      public override int GenericParameterPosition
      {
         get
         {
            return typeImpl.GenericParameterPosition;
         }
      }

      public override bool IsConstructedGenericType
      {
         get
         {
            return typeImpl.IsConstructedGenericType;
         }
      }

      public override bool IsGenericParameter
      {
         get
         {
            return typeImpl.IsGenericParameter;
         }
      }


      public override bool IsSecurityCritical
      {
         get
         {
            return typeImpl.IsSecurityCritical;
         }
      }

      public override bool IsSecuritySafeCritical
      {
         get
         {
            return typeImpl.IsSecuritySafeCritical;
         }
      }

      public override bool IsSecurityTransparent
      {
         get
         {
            return typeImpl.IsSecurityTransparent;
         }
      }

      public override Type ReflectedType
      {
         get
         {
            return typeImpl.ReflectedType;
         }
      }

      public override StructLayoutAttribute StructLayoutAttribute
      {
         get
         {
            return typeImpl.StructLayoutAttribute;
         }
      }

      public override RuntimeTypeHandle TypeHandle
      {
         get
         {
            return typeImpl.TypeHandle;
         }
      }

      public override IList<CustomAttributeData> GetCustomAttributesData()
      {
         return typeImpl.GetCustomAttributesData();
      }      
   }


      

}
