using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace CustomAttributeTable
{


   /// <summary>
   /// Wraps a <see cref="MethodInfo"/> object and delegates methods to that <c>PropertyInfo</c>. 
   /// Similar to what <see cref="TypeDelegator"/> does for <see cref="System.Type"/>.
   /// </summary>
   public class MethodInfoDelegator : MethodInfo
   {
      private readonly MethodInfo m_parent;

      public MethodInfoDelegator(MethodInfo parent)
      {
         m_parent = parent;
      }

      public override object[] GetCustomAttributes(bool inherit)
      {
         return m_parent.GetCustomAttributes(inherit);
      }

      public override bool IsDefined(Type attributeType, bool inherit)
      {
         return m_parent.IsDefined(attributeType, inherit);
      }

      public override ParameterInfo[] GetParameters()
      {
         return m_parent.GetParameters();
      }

      public override MethodImplAttributes GetMethodImplementationFlags()
      {
         return m_parent.GetMethodImplementationFlags();
      }

      public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture)
      {
         return m_parent.Invoke(obj, invokeAttr, binder, parameters, culture);
      }

      public override MethodInfo GetBaseDefinition()
      {
         return m_parent.GetBaseDefinition();
      }

      public override ICustomAttributeProvider ReturnTypeCustomAttributes
      {
         get { return m_parent.ReturnTypeCustomAttributes; }
      }

      public override string Name
      {
         get { return m_parent.Name; }
      }

      public override Type DeclaringType
      {
         get { return m_parent.DeclaringType; }
      }

      public override Type ReflectedType
      {
         get { return m_parent.ReflectedType; }
      }

      public override RuntimeMethodHandle MethodHandle
      {
         get { return m_parent.MethodHandle; }
      }

      public override MethodAttributes Attributes
      {
         get { return m_parent.Attributes; }
      }

      public override object[] GetCustomAttributes(Type attributeType, bool inherit)
      {
         return m_parent.GetCustomAttributes(attributeType, inherit);
      }

      public override Type ReturnType => m_parent.ReturnType;
      public override CallingConventions CallingConvention => m_parent.CallingConvention;
      public override bool ContainsGenericParameters => m_parent.ContainsGenericParameters;
      public override Type[] GetGenericArguments() => m_parent.GetGenericArguments();
      public override bool IsGenericMethod => m_parent.IsGenericMethod;
      public override IList<CustomAttributeData> GetCustomAttributesData() => m_parent.GetCustomAttributesData();
      public override MethodInfo GetGenericMethodDefinition() => m_parent.GetGenericMethodDefinition();
      public override bool IsGenericMethodDefinition => m_parent.IsGenericMethodDefinition;
      public override bool IsSecurityCritical => m_parent.IsSecurityCritical;
      public override bool IsSecuritySafeCritical => m_parent.IsSecuritySafeCritical;
      public override bool IsSecurityTransparent => m_parent.IsSecurityTransparent;
      public override MethodInfo MakeGenericMethod(params Type[] typeArguments) => m_parent.MakeGenericMethod(typeArguments);
      public override MemberTypes MemberType => m_parent.MemberType;
      public override MethodImplAttributes MethodImplementationFlags => m_parent.MethodImplementationFlags;
      public override Module Module => m_parent.Module;
      public override ParameterInfo ReturnParameter => m_parent.ReturnParameter;
   }

   
}
