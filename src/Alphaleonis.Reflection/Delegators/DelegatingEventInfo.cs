using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Alphaleonis.Reflection
{
   /// <summary>
   /// Wraps a <see cref="EventInfo"/> instance and delegates all method calls to that
   /// <see cref="EventInfo" />.
   /// </summary>
   /// <remarks>
   /// <para>Derive from this type and override only those members you have to provide customization
   /// in.</para>
   /// <para><see cref="DelegatingEventInfo"/> derives from <see cref="EventInfo"/> and implements most
   /// of the properties and methods of <see cref="EventInfo"/>. For
   ///       each member it implements, <see cref="DelegatingEventInfo"/> automatically delegates to
   ///       the corresponding member of the internal <see cref="EventInfo"/>
   ///       object which was supplied as the argument to the constructor.  This internal
   ///       <see cref="EventInfo"/> is exposed to deriving classes by the <see langword="protected" />
   ///       <see cref="m_eventImpl"/> field.</para>
   /// </remarks>
   public class DelegatingEventInfo : EventInfo
   {
      private readonly EventInfo m_eventImpl;

      public DelegatingEventInfo(EventInfo eventInfo)
      {
         if (eventInfo == null)
            throw new ArgumentNullException(nameof(eventInfo), $"{nameof(eventInfo)} is null.");

         m_eventImpl = eventInfo;
      }

      public override void AddEventHandler(object target, Delegate handler)
      {
         m_eventImpl.AddEventHandler(target, handler);
      }

      public override MethodInfo AddMethod => m_eventImpl.AddMethod;

      public override EventAttributes Attributes => m_eventImpl.Attributes;

      public override IEnumerable<CustomAttributeData> CustomAttributes => m_eventImpl.CustomAttributes;

      public override Type DeclaringType => m_eventImpl.DeclaringType;

      public override bool Equals(object obj)
      {
         DelegatingEventInfo other = obj as DelegatingEventInfo;
         if (other != null)
         {
            return m_eventImpl.Equals(other.m_eventImpl);
         }

         return m_eventImpl.Equals(obj);
      }

      public override Type EventHandlerType => m_eventImpl.EventHandlerType;

      public override MethodInfo GetAddMethod(bool nonPublic) => m_eventImpl.GetAddMethod(nonPublic);

      public override object[] GetCustomAttributes(bool inherit) => m_eventImpl.GetCustomAttributes(inherit);

      public override object[] GetCustomAttributes(Type attributeType, bool inherit) => m_eventImpl.GetCustomAttributes(attributeType, inherit);

      public override IList<CustomAttributeData> GetCustomAttributesData() => m_eventImpl.GetCustomAttributesData();

      public override int GetHashCode() => m_eventImpl.GetHashCode();

      public override MethodInfo[] GetOtherMethods(bool nonPublic) => m_eventImpl.GetOtherMethods(nonPublic);

      public override MethodInfo GetRaiseMethod(bool nonPublic) => m_eventImpl.GetRaiseMethod(nonPublic);

      public override MethodInfo GetRemoveMethod(bool nonPublic) => m_eventImpl.GetRemoveMethod(nonPublic);

      public override bool IsDefined(Type attributeType, bool inherit) => m_eventImpl.IsDefined(attributeType, inherit);

      public override bool IsMulticast => m_eventImpl.IsMulticast;

      public override MemberTypes MemberType => m_eventImpl.MemberType;

      public override int MetadataToken => m_eventImpl.MetadataToken;

      public override Module Module => m_eventImpl.Module;

      public override string Name => m_eventImpl.Name;

      public override MethodInfo RaiseMethod => m_eventImpl.RaiseMethod;

      public override Type ReflectedType => m_eventImpl.ReflectedType;

      public override void RemoveEventHandler(object target, Delegate handler)
      {
         m_eventImpl.RemoveEventHandler(target, handler);
      }

      public override MethodInfo RemoveMethod => m_eventImpl.RemoveMethod;

      public override string ToString()
      {
         return m_eventImpl.ToString();
      }
   }
}
