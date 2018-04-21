using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Alphaleonis.Reflection
{
   public class DelegatingMethodInfo : MethodInfo
   {
      private readonly MethodInfo m_method;

      public DelegatingMethodInfo(MethodInfo method)
      {
         m_method = method;
      }

      public override MethodAttributes Attributes => m_method.Attributes;

      public override CallingConventions CallingConvention => m_method.CallingConvention;

      public override bool ContainsGenericParameters => m_method.ContainsGenericParameters;

      public override Type DeclaringType => m_method.DeclaringType;

      public override bool IsGenericMethod => m_method.IsGenericMethod;

      public override bool IsGenericMethodDefinition => m_method.IsGenericMethodDefinition;

      public override bool IsSecurityCritical => m_method.IsSecurityCritical;

      public override bool IsSecuritySafeCritical => m_method.IsSecuritySafeCritical;

      public override bool IsSecurityTransparent => m_method.IsSecurityTransparent;

      public override int MetadataToken => m_method.MetadataToken;

      public override RuntimeMethodHandle MethodHandle => m_method.MethodHandle;

      public override Module Module => m_method.Module;

      public override string Name => m_method.Name;

      public override Type ReflectedType => m_method.ReflectedType;

      public override ParameterInfo ReturnParameter => m_method.ReturnParameter;

      public override Type ReturnType => m_method.ReturnType;

      public override ICustomAttributeProvider ReturnTypeCustomAttributes => m_method.ReturnTypeCustomAttributes;

      public MethodInfo UnderlyingMethod => m_method;

      public override Delegate CreateDelegate(Type delegateType) => m_method.CreateDelegate(delegateType);

      public override Delegate CreateDelegate(Type delegateType, object target) => m_method.CreateDelegate(delegateType, target);

      public override MethodInfo GetBaseDefinition() => m_method.GetBaseDefinition();

      public override object[] GetCustomAttributes(bool inherit) => m_method.GetCustomAttributes(inherit);

      public override object[] GetCustomAttributes(Type attributeType, bool inherit) => m_method.GetCustomAttributes(attributeType, inherit);

      public override IList<CustomAttributeData> GetCustomAttributesData() => m_method.GetCustomAttributesData();

      public override Type[] GetGenericArguments() => m_method.GetGenericArguments();

      public override MethodInfo GetGenericMethodDefinition() => m_method.GetGenericMethodDefinition();

      public override MethodBody GetMethodBody() => m_method.GetMethodBody();

      public override MethodImplAttributes GetMethodImplementationFlags() => m_method.GetMethodImplementationFlags();

      public override ParameterInfo[] GetParameters() => m_method.GetParameters();

      public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture) => m_method.Invoke(obj, invokeAttr, binder, parameters, culture);

      public override bool IsDefined(Type attributeType, bool inherit) => m_method.IsDefined(attributeType, inherit);

      public override MethodInfo MakeGenericMethod(params Type[] typeArguments) => m_method.MakeGenericMethod(typeArguments);

      public override string ToString() => m_method.ToString();

      public override int GetHashCode() => m_method.GetHashCode();

      public override bool Equals(object obj) 
      {
         DelegatingMethodInfo other = obj as DelegatingMethodInfo;
         if (other != null)
            return other.m_method.Equals(m_method);
         else
            return m_method.Equals(obj);
      }

      public override IEnumerable<CustomAttributeData> CustomAttributes => m_method.CustomAttributes;

      public override MethodImplAttributes MethodImplementationFlags => m_method.MethodImplementationFlags;
   }

   
}
