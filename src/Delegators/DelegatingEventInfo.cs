using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Alphaleonis.Reflection
{
   // TODO PP (2018-04-21): Document
   public class DelegatingEventInfo : EventInfo
   {
      private readonly EventInfo m_event;

      public DelegatingEventInfo(EventInfo eventInfo)
      {
         if (eventInfo == null)
            throw new ArgumentNullException(nameof(eventInfo), $"{nameof(eventInfo)} is null.");

         m_event = eventInfo;
      }

      public override void AddEventHandler(object target, Delegate handler)
      {
         m_event.AddEventHandler(target, handler);
      }

      public override MethodInfo AddMethod => m_event.AddMethod;

      public override EventAttributes Attributes => m_event.Attributes;

      public override IEnumerable<CustomAttributeData> CustomAttributes => m_event.CustomAttributes;

      public override Type DeclaringType => m_event.DeclaringType;

      public override bool Equals(object obj)
      {
         DelegatingEventInfo other = obj as DelegatingEventInfo;
         if (other != null)
         {
            return m_event.Equals(other.m_event);
         }

         return m_event.Equals(obj);
      }

      public override Type EventHandlerType => m_event.EventHandlerType;

      public override MethodInfo GetAddMethod(bool nonPublic) => m_event.GetAddMethod(nonPublic);

      public override object[] GetCustomAttributes(bool inherit) => m_event.GetCustomAttributes(inherit);

      public override object[] GetCustomAttributes(Type attributeType, bool inherit) => m_event.GetCustomAttributes(attributeType, inherit);

      public override IList<CustomAttributeData> GetCustomAttributesData() => m_event.GetCustomAttributesData();

      public override int GetHashCode() => m_event.GetHashCode();

      public override MethodInfo[] GetOtherMethods(bool nonPublic) => m_event.GetOtherMethods(nonPublic);

      public override MethodInfo GetRaiseMethod(bool nonPublic) => m_event.GetRaiseMethod(nonPublic);

      public override MethodInfo GetRemoveMethod(bool nonPublic) => m_event.GetRemoveMethod(nonPublic);

      public override bool IsDefined(Type attributeType, bool inherit) => m_event.IsDefined(attributeType, inherit);

      public override bool IsMulticast => m_event.IsMulticast;

      public override MemberTypes MemberType => m_event.MemberType;

      public override int MetadataToken => m_event.MetadataToken;

      public override Module Module => m_event.Module;

      public override string Name => m_event.Name;

      public override MethodInfo RaiseMethod => m_event.RaiseMethod;

      public override Type ReflectedType => m_event.ReflectedType;

      public override void RemoveEventHandler(object target, Delegate handler)
      {
         m_event.RemoveEventHandler(target, handler);
      }

      public override MethodInfo RemoveMethod => m_event.RemoveMethod;

      public override string ToString()
      {
         return m_event.ToString();
      }
   }
}
