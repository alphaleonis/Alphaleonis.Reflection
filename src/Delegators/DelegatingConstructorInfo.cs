using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Alphaleonis.Reflection
{
   // TODO PP (2018-04-21): Document
   public class DelegatingConstructorInfo : ConstructorInfo
   {
      private readonly ConstructorInfo m_constructor;

      public DelegatingConstructorInfo(ConstructorInfo constructor)
      {
         m_constructor = constructor;
      }

      public override MethodAttributes Attributes => m_constructor.Attributes;

      public override CallingConventions CallingConvention => m_constructor.CallingConvention;

      public override bool ContainsGenericParameters => m_constructor.ContainsGenericParameters;

      public override Type DeclaringType => m_constructor.DeclaringType;

      public override bool IsGenericMethod => m_constructor.IsGenericMethod;

      public override bool IsGenericMethodDefinition => m_constructor.IsGenericMethodDefinition;

      public override bool IsSecurityCritical => m_constructor.IsSecurityCritical;

      public override bool IsSecuritySafeCritical => m_constructor.IsSecuritySafeCritical;

      public override bool IsSecurityTransparent => m_constructor.IsSecurityTransparent;

      public override int MetadataToken => m_constructor.MetadataToken;

      public override RuntimeMethodHandle MethodHandle => m_constructor.MethodHandle;

      public override Module Module => m_constructor.Module;

      public override string Name => m_constructor.Name;

      public override Type ReflectedType => m_constructor.ReflectedType;

      public ConstructorInfo UnderlyingConstructor => m_constructor;

      public override object[] GetCustomAttributes(bool inherit) => m_constructor.GetCustomAttributes(inherit);

      public override object[] GetCustomAttributes(Type attributeType, bool inherit) => m_constructor.GetCustomAttributes(attributeType, inherit);

      public override IList<CustomAttributeData> GetCustomAttributesData() => m_constructor.GetCustomAttributesData();

      public override Type[] GetGenericArguments() => m_constructor.GetGenericArguments();

      public override MethodBody GetMethodBody() => m_constructor.GetMethodBody();

      public override MethodImplAttributes GetMethodImplementationFlags() => m_constructor.GetMethodImplementationFlags();

      public override ParameterInfo[] GetParameters() => m_constructor.GetParameters();

      public override object Invoke(BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture) => m_constructor.Invoke(invokeAttr, binder, parameters, culture);

      public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture) => m_constructor.Invoke(obj, invokeAttr, binder, parameters, culture);

      public override bool IsDefined(Type attributeType, bool inherit) => m_constructor.IsDefined(attributeType, inherit);

      public override string ToString() => m_constructor.ToString();

      public override IEnumerable<CustomAttributeData> CustomAttributes => m_constructor.CustomAttributes;

      public override bool Equals(object obj)
      {
         DelegatingConstructorInfo other = obj as DelegatingConstructorInfo;
         if (other != null)
            return m_constructor.Equals(other.m_constructor);
         else
            return m_constructor.Equals(obj);
      }

      public override int GetHashCode() => m_constructor.GetHashCode();

      public override MemberTypes MemberType => m_constructor.MemberType;

      public override MethodImplAttributes MethodImplementationFlags => m_constructor.MethodImplementationFlags;
   }
}
