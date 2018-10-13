// Copyright (c) Peter Palotas
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

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
   /// <para><see cref="DelegatingEventInfo"/> derives from <see cref="EventInfo"/> and implements
   /// most
   /// of the properties and methods of <see cref="EventInfo"/>. For
   ///       each member it implements, <see cref="DelegatingEventInfo"/> automatically delegates to
   ///       the corresponding member of the internal <see cref="EventInfo"/>
   ///       object which was supplied as the argument to the constructor.  This internal
   ///       <see cref="EventInfo"/> is exposed to deriving classes by the
   ///       <see cref="UnderlyingEventInfo"/> property.</para>
   /// </remarks>
   public class DelegatingEventInfo : EventInfo
   {
      private readonly EventInfo m_eventImpl;

      /// <summary>Constructor.</summary>
      /// <exception cref="ArgumentNullException">Thrown when one or more required arguments are null.</exception>
      /// <param name="eventInfo">The <see cref="EventInfo"/> to delegate all calls to.</param>
      public DelegatingEventInfo(EventInfo eventInfo)
      {
         if (eventInfo == null)
            throw new ArgumentNullException(nameof(eventInfo), $"{nameof(eventInfo)} is null.");

         m_eventImpl = eventInfo;
      }

      /// <summary>The underlying <see cref="EventInfo"/> that was passed to the constructor.</summary>
      public EventInfo UnderlyingEventInfo => m_eventImpl;

      /// <inheritdoc/>
      public override void AddEventHandler(object target, Delegate handler)
      {
         m_eventImpl.AddEventHandler(target, handler);
      }

      /// <inheritdoc/>
      public override MethodInfo AddMethod => m_eventImpl.AddMethod;

      /// <inheritdoc/>
      public override EventAttributes Attributes => m_eventImpl.Attributes;

      /// <inheritdoc/>
      public override IEnumerable<CustomAttributeData> CustomAttributes => m_eventImpl.CustomAttributes;

      /// <inheritdoc/>
      public override Type DeclaringType => m_eventImpl.DeclaringType;

      /// <inheritdoc/>
      public override bool Equals(object obj)
      {
         DelegatingEventInfo other = obj as DelegatingEventInfo;
         if (other != null)
         {
            return m_eventImpl.Equals(other.m_eventImpl);
         }

         return m_eventImpl.Equals(obj);
      }

      /// <inheritdoc/>
      public override Type EventHandlerType => m_eventImpl.EventHandlerType;

      /// <inheritdoc/>
      public override MethodInfo GetAddMethod(bool nonPublic) => m_eventImpl.GetAddMethod(nonPublic);

      /// <inheritdoc/>
      public override object[] GetCustomAttributes(bool inherit) => m_eventImpl.GetCustomAttributes(inherit);

      /// <inheritdoc/>
      public override object[] GetCustomAttributes(Type attributeType, bool inherit) => m_eventImpl.GetCustomAttributes(attributeType, inherit);

      /// <inheritdoc/>
      public override IList<CustomAttributeData> GetCustomAttributesData() => m_eventImpl.GetCustomAttributesData();

      /// <inheritdoc/>
      public override int GetHashCode() => m_eventImpl.GetHashCode();

      /// <inheritdoc/>
      public override MethodInfo[] GetOtherMethods(bool nonPublic) => m_eventImpl.GetOtherMethods(nonPublic);

      /// <inheritdoc/>
      public override MethodInfo GetRaiseMethod(bool nonPublic) => m_eventImpl.GetRaiseMethod(nonPublic);

      /// <inheritdoc/>
      public override MethodInfo GetRemoveMethod(bool nonPublic) => m_eventImpl.GetRemoveMethod(nonPublic);

      /// <inheritdoc/>
      public override bool IsDefined(Type attributeType, bool inherit) => m_eventImpl.IsDefined(attributeType, inherit);

      /// <inheritdoc/>
      public override bool IsMulticast => m_eventImpl.IsMulticast;

      /// <inheritdoc/>
      public override MemberTypes MemberType => m_eventImpl.MemberType;

      /// <inheritdoc/>
      public override int MetadataToken => m_eventImpl.MetadataToken;

      /// <inheritdoc/>
      public override Module Module => m_eventImpl.Module;

      /// <inheritdoc/>
      public override string Name => m_eventImpl.Name;

      /// <inheritdoc/>
      public override MethodInfo RaiseMethod => m_eventImpl.RaiseMethod;

      /// <inheritdoc/>
      public override Type ReflectedType => m_eventImpl.ReflectedType;

      /// <inheritdoc/>
      public override void RemoveEventHandler(object target, Delegate handler) => m_eventImpl.RemoveEventHandler(target, handler);

      /// <inheritdoc/>
      public override MethodInfo RemoveMethod => m_eventImpl.RemoveMethod;

      /// <inheritdoc/>
      public override string ToString() => m_eventImpl.ToString();
   }
}
