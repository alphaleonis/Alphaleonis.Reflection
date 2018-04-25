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
   /// <para><see cref="DelegatingMethodInfo"/> derives from <see cref="MethodInfo"/> and implements most
   /// of the properties and methods of <see cref="MethodInfo"/>. For
   ///       each member it implements, <see cref="DelegatingMethodInfo"/> automatically delegates to
   ///       the corresponding member of the internal <see cref="MethodInfo"/>
   ///       object which was supplied as the argument to the constructor.  This internal
   ///       <see cref="MethodInfo"/> is exposed to deriving classes by the <see langword="protected" />
   ///       <see cref="m_methodImpl"/> field.</para>
   /// </remarks>
   public class DelegatingMethodInfo : MethodInfo
   {
      private readonly MethodInfo m_methodImpl;

      public DelegatingMethodInfo(MethodInfo method)
      {
         m_methodImpl = method;
      }

      public override MethodAttributes Attributes => m_methodImpl.Attributes;

      public override CallingConventions CallingConvention => m_methodImpl.CallingConvention;

      public override bool ContainsGenericParameters => m_methodImpl.ContainsGenericParameters;

      public override Type DeclaringType => m_methodImpl.DeclaringType;

      public override bool IsGenericMethod => m_methodImpl.IsGenericMethod;

      public override bool IsGenericMethodDefinition => m_methodImpl.IsGenericMethodDefinition;

      public override bool IsSecurityCritical => m_methodImpl.IsSecurityCritical;

      public override bool IsSecuritySafeCritical => m_methodImpl.IsSecuritySafeCritical;

      public override bool IsSecurityTransparent => m_methodImpl.IsSecurityTransparent;

      public override int MetadataToken => m_methodImpl.MetadataToken;

      public override RuntimeMethodHandle MethodHandle => m_methodImpl.MethodHandle;

      public override Module Module => m_methodImpl.Module;

      public override string Name => m_methodImpl.Name;

      public override Type ReflectedType => m_methodImpl.ReflectedType;

      public override ParameterInfo ReturnParameter => m_methodImpl.ReturnParameter;

      public override Type ReturnType => m_methodImpl.ReturnType;

      public override ICustomAttributeProvider ReturnTypeCustomAttributes => m_methodImpl.ReturnTypeCustomAttributes;

      public MethodInfo UnderlyingMethod => m_methodImpl;

      public override Delegate CreateDelegate(Type delegateType) => m_methodImpl.CreateDelegate(delegateType);

      public override Delegate CreateDelegate(Type delegateType, object target) => m_methodImpl.CreateDelegate(delegateType, target);

      public override MethodInfo GetBaseDefinition() => m_methodImpl.GetBaseDefinition();

      public override object[] GetCustomAttributes(bool inherit) => m_methodImpl.GetCustomAttributes(inherit);

      public override object[] GetCustomAttributes(Type attributeType, bool inherit) => m_methodImpl.GetCustomAttributes(attributeType, inherit);

      public override IList<CustomAttributeData> GetCustomAttributesData() => m_methodImpl.GetCustomAttributesData();

      public override Type[] GetGenericArguments() => m_methodImpl.GetGenericArguments();

      public override MethodInfo GetGenericMethodDefinition() => m_methodImpl.GetGenericMethodDefinition();

      public override MethodBody GetMethodBody() => m_methodImpl.GetMethodBody();

      public override MethodImplAttributes GetMethodImplementationFlags() => m_methodImpl.GetMethodImplementationFlags();

      public override ParameterInfo[] GetParameters() => m_methodImpl.GetParameters();

      public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture) => m_methodImpl.Invoke(obj, invokeAttr, binder, parameters, culture);

      public override bool IsDefined(Type attributeType, bool inherit) => m_methodImpl.IsDefined(attributeType, inherit);

      public override MethodInfo MakeGenericMethod(params Type[] typeArguments) => m_methodImpl.MakeGenericMethod(typeArguments);

      public override string ToString() => m_methodImpl.ToString();

      public override int GetHashCode() => m_methodImpl.GetHashCode();

      public override bool Equals(object obj) 
      {
         DelegatingMethodInfo other = obj as DelegatingMethodInfo;
         if (other != null)
            return other.m_methodImpl.Equals(m_methodImpl);
         else
            return m_methodImpl.Equals(obj);
      }

      public override IEnumerable<CustomAttributeData> CustomAttributes => m_methodImpl.CustomAttributes;

      public override MethodImplAttributes MethodImplementationFlags => m_methodImpl.MethodImplementationFlags;
   }

   
}
