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
   /// <para><see cref="DelegatingConstructorInfo"/> derives from <see cref="ConstructorInfo"/> and implements most
   /// of the properties and methods of <see cref="ConstructorInfo"/>. For
   ///       each member it implements, <see cref="DelegatingConstructorInfo"/> automatically delegates to
   ///       the corresponding member of the internal <see cref="ConstructorInfo"/>
   ///       object which was supplied as the argument to the constructor.  This internal
   ///       <see cref="ConstructorInfo"/> is exposed to deriving classes by the <see langword="protected" />
   ///       <see cref="m_constructorImpl"/> field.</para>
   /// </remarks>
   public class DelegatingConstructorInfo : ConstructorInfo
   {
      private readonly ConstructorInfo m_constructorImpl;

      public DelegatingConstructorInfo(ConstructorInfo constructor)
      {
         m_constructorImpl = constructor;
      }

      public override MethodAttributes Attributes => m_constructorImpl.Attributes;

      public override CallingConventions CallingConvention => m_constructorImpl.CallingConvention;

      public override bool ContainsGenericParameters => m_constructorImpl.ContainsGenericParameters;

      public override Type DeclaringType => m_constructorImpl.DeclaringType;

      public override bool IsGenericMethod => m_constructorImpl.IsGenericMethod;

      public override bool IsGenericMethodDefinition => m_constructorImpl.IsGenericMethodDefinition;

      public override bool IsSecurityCritical => m_constructorImpl.IsSecurityCritical;

      public override bool IsSecuritySafeCritical => m_constructorImpl.IsSecuritySafeCritical;

      public override bool IsSecurityTransparent => m_constructorImpl.IsSecurityTransparent;

      public override int MetadataToken => m_constructorImpl.MetadataToken;

      public override RuntimeMethodHandle MethodHandle => m_constructorImpl.MethodHandle;

      public override Module Module => m_constructorImpl.Module;

      public override string Name => m_constructorImpl.Name;

      public override Type ReflectedType => m_constructorImpl.ReflectedType;

      public ConstructorInfo UnderlyingConstructor => m_constructorImpl;

      public override object[] GetCustomAttributes(bool inherit) => m_constructorImpl.GetCustomAttributes(inherit);

      public override object[] GetCustomAttributes(Type attributeType, bool inherit) => m_constructorImpl.GetCustomAttributes(attributeType, inherit);

      public override IList<CustomAttributeData> GetCustomAttributesData() => m_constructorImpl.GetCustomAttributesData();

      public override Type[] GetGenericArguments() => m_constructorImpl.GetGenericArguments();

      public override MethodBody GetMethodBody() => m_constructorImpl.GetMethodBody();

      public override MethodImplAttributes GetMethodImplementationFlags() => m_constructorImpl.GetMethodImplementationFlags();

      public override ParameterInfo[] GetParameters() => m_constructorImpl.GetParameters();

      public override object Invoke(BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture) => m_constructorImpl.Invoke(invokeAttr, binder, parameters, culture);

      public override object Invoke(object obj, BindingFlags invokeAttr, Binder binder, object[] parameters, CultureInfo culture) => m_constructorImpl.Invoke(obj, invokeAttr, binder, parameters, culture);

      public override bool IsDefined(Type attributeType, bool inherit) => m_constructorImpl.IsDefined(attributeType, inherit);

      public override string ToString() => m_constructorImpl.ToString();

      public override IEnumerable<CustomAttributeData> CustomAttributes => m_constructorImpl.CustomAttributes;

      public override bool Equals(object obj)
      {
         DelegatingConstructorInfo other = obj as DelegatingConstructorInfo;
         if (other != null)
            return m_constructorImpl.Equals(other.m_constructorImpl);
         else
            return m_constructorImpl.Equals(obj);
      }

      public override int GetHashCode() => m_constructorImpl.GetHashCode();

      public override MemberTypes MemberType => m_constructorImpl.MemberType;

      public override MethodImplAttributes MethodImplementationFlags => m_constructorImpl.MethodImplementationFlags;
   }
}
