// Copyright (c) Peter Palotas
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using Alphaleonis.Reflection;

namespace Tests.Alphaleonis.Reflection
{
#pragma warning disable 0649, 0067

   [TestClass]
   public class MemberInfoExtensionTests
   {
      #region EventInfo.IsOverride() Tests

      [TestMethod]
      public void IsOverride_EventInfo_NonOverridden_ReturnsTrue()
      {
         Assert.IsFalse(typeof(Base).GetEvent(nameof(Base.OverriddenEvent)).IsOverride());
      }

      [TestMethod]
      public void IsOverride_EventInfo_Overridden_ReturnsTrue()
      {
         Assert.IsTrue(typeof(Derived).GetEvent(nameof(Derived.OverriddenEvent)).IsOverride());
      }

      [TestMethod]
      public void IsOverride_EventInfo_Inherited_ReturnsFalse()
      {
         Assert.IsFalse(typeof(Derived).GetEvent(nameof(Derived.InheritedEvent)).IsOverride());
      }

      [TestMethod]
      public void IsOverride_EventInfo_Hidden_ReturnsFalse()
      {
         Assert.IsFalse(typeof(Derived).GetEvent(nameof(Derived.HiddenEvent)).IsOverride());
      }


      #endregion

      #region PropertyInfo.IsOverride() Tests

      [TestMethod]
      public void IsOverride_PropertyInfo_NonOverridden_ReturnsFalse()
      {
         Assert.IsFalse(typeof(Base).GetProperty(nameof(Derived.InheritedProperty)).IsOverride());
      }

      [TestMethod]
      public void IsOverride_PropertyInfo_Overridden_ReturnsTrue()
      {
         Assert.IsTrue(typeof(Derived).GetProperty(nameof(Derived.OverriddenProperty)).IsOverride());
      }

      [TestMethod]
      public void IsOverride_PropertyInfo_Inherited_ReturnsFalse()
      {
         Assert.IsFalse(typeof(Derived).GetProperty(nameof(Derived.InheritedProperty)).IsOverride());
      }

      [TestMethod]
      public void IsOverride_PropertyInfo_Hidden_ReturnsFalse()
      {
         Assert.IsFalse(typeof(NonVirtualDerived).GetProperty(nameof(NonVirtualDerived.NonVirtualProperty)).IsOverride());
      }

      [TestMethod]
      public void IsOverride_PropertyInfo_InterfaceImplementation_ReturnsFalse()
      {
         Assert.IsFalse(typeof(Implementation).GetProperty(nameof(Implementation.InterfaceProperty)).IsOverride());
      }

      [TestMethod]
      public void IsOverride_PropertyInfo_AbstractOverridden_ReturnsTrue()
      {
         Assert.IsTrue(typeof(Derived).GetProperty(nameof(Derived.AbstractOverriddenProperty)).IsOverride());
      }

      [TestMethod]
      public void IsOverride_PropertyInfo_AbstractInherited_ReturnsFalse()
      {
         Assert.IsFalse(typeof(Derived).GetProperty(nameof(Derived.AbstractNonOverriddenProperty)).IsOverride());
      }

      [TestMethod]
      public void IsOverride_PropertyInfo_SealedOverridden_ReturnsTrue()
      {
         Assert.IsTrue(typeof(Derived).GetProperty(nameof(Derived.SealedOverriddenProperty)).IsOverride());
      }

      #endregion

      #region MethodInfo.IsOverride() tests

      [TestMethod]
      public void IsOverride_MethodInfo_Overridden_ReturnsTrue()
      {
         Assert.IsTrue(typeof(Derived).GetMethod(nameof(Derived.OverriddenMethod)).IsOverride());
      }

      [TestMethod]
      public void IsOverride_MethodInfo_SealedOverridden_ReturnsTrue()
      {
         Assert.IsTrue(typeof(Derived).GetMethod(nameof(Derived.SealedOverriddenMethod)).IsOverride());
      }

      [TestMethod]
      public void IsOverride_MethodInfo_Inherited_ReturnsFalse()
      {
         Assert.IsFalse(typeof(Derived).GetMethod(nameof(Derived.InheritedMethod)).IsOverride());
      }

      [TestMethod]
      public void IsOverride_MethodInfo_Hidden_ReturnsFalse()
      {
         Assert.IsFalse(typeof(NonVirtualDerived).GetMethod(nameof(NonVirtualDerived.F)).IsOverride());
      }

      [TestMethod]
      public void IsOverride_MethodInfo_InterfaceImplementation_ReturnsFalse()
      {
         Assert.IsFalse(typeof(Implementation).GetMethod(nameof(NonVirtualDerived.F)).IsOverride());
      }

      #endregion

      #region EventInfo.GetParentDefinition() Tests

      [TestMethod]
      public void GetParentDefinition_EventInfo_OverriddenEventWithParentTwoClassesUp_ReturnsExpectedEvent()
      {
         EventInfo actual = typeof(SubSubDerived).GetEvent(nameof(SubSubDerived.OverriddenEvent)).GetParentDefinition();
         EventInfo expected = typeof(Derived).GetEvent(nameof(Derived.OverriddenEvent));
         Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void GetParentDefinition_EventInfo_OverriddenEvent_ReturnsExpectedEvent()
      {
         EventInfo actual = typeof(Derived).GetEvent(nameof(Derived.OverriddenEvent)).GetParentDefinition();
         EventInfo expected = typeof(Base).GetEvent(nameof(Base.OverriddenEvent));
         Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void GetParentDefinition_EventInfo_VirtualEventInBaseClass_ReturnsNull()
      {
         EventInfo actual = typeof(Base).GetEvent(nameof(Base.OverriddenEvent)).GetParentDefinition();
         EventInfo expected = null;
         Assert.AreEqual(expected, actual);
      }

      #endregion

      #region PropertyInfo.GetParentDefinition() Tests

      [TestMethod]
      public void GetParentDefinition_PropertyInfo_OverriddenPropertyWithParentTwoClassesUp_ReturnsExpectedProperty()
      {
         PropertyInfo actual = typeof(SubSubDerived).GetProperty(nameof(SubSubDerived.OverriddenProperty)).GetParentDefinition();
         PropertyInfo expected = typeof(Derived).GetProperty(nameof(Derived.OverriddenProperty));
         Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void GetParentDefinition_PropertyInfo_OverriddenProperty_ReturnsExpectedProperty()
      {
         PropertyInfo actual = typeof(Derived).GetProperty(nameof(Derived.OverriddenProperty)).GetParentDefinition();
         PropertyInfo expected = typeof(Base).GetProperty(nameof(Base.OverriddenProperty));
         Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void GetParentDefinition_PropertyInfo_VirtualPropertyInBaseClass_ReturnsNull()
      {
         PropertyInfo actual = typeof(Base).GetProperty(nameof(Base.OverriddenProperty)).GetParentDefinition();
         PropertyInfo expected = null;
         Assert.AreEqual(expected, actual);
      }

      #endregion

      #region PropertyInfo.GetParentDefinition() Tests

      [TestMethod]
      public void GetParentDefinition_MethodInfo_OverriddenMethodWithParentTwoClassesUp_ReturnsExpectedMethod()
      {
         MethodInfo actual = typeof(SubSubDerived).GetMethod(nameof(SubSubDerived.OverriddenMethod)).GetParentDefinition();
         MethodInfo expected = typeof(Derived).GetMethod(nameof(Derived.OverriddenMethod));
         Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void GetParentDefinition_MethodInfo_OverriddenMethod_ReturnsExpectedMethod()
      {
         MethodInfo actual = typeof(Derived).GetMethod(nameof(Derived.OverriddenMethod)).GetParentDefinition();
         MethodInfo expected = typeof(Base).GetMethod(nameof(Base.OverriddenMethod));
         Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void GetParentDefinition_MethodInfo_VirtualMethodInBaseClass_ReturnsNull()
      {
         MethodInfo actual = typeof(Base).GetMethod(nameof(Base.OverriddenMethod)).GetParentDefinition();
         MethodInfo expected = null;
         Assert.AreEqual(expected, actual);
      }

      #endregion

      #region Nested Types

      abstract class Base
      {
         public virtual event EventHandler InheritedEvent;
         public virtual event EventHandler OverriddenEvent;
         public event EventHandler HiddenEvent;

         public virtual void OverriddenMethod() { }
         public virtual void InheritedMethod() { }
         public virtual void SealedOverriddenMethod() { }

         public virtual string OverriddenProperty { get; set; }
         public virtual string InheritedProperty { get; set; }
         public abstract string AbstractOverriddenProperty { get; set; }
         public abstract string AbstractNonOverriddenProperty { get; set; }
         public virtual string SealedOverriddenProperty { get; set; }
      }

      abstract class Derived : Base
      {
         public override event EventHandler OverriddenEvent;
         public new event EventHandler HiddenEvent;

         public override void OverriddenMethod() { }
         public sealed override void SealedOverriddenMethod() { }

         public override string OverriddenProperty { get; set; }
         public override string AbstractOverriddenProperty { get; set; }
         public sealed override string SealedOverriddenProperty { get; set; }
      }

      abstract class SubDerived : Derived
      {

      }

      abstract class SubSubDerived : SubDerived
      {
         public override event EventHandler OverriddenEvent;

         public override void OverriddenMethod()
         {
            base.OverriddenMethod();
         }

         public override string OverriddenProperty { get; set; }
      }

      interface Interface
      {
         void F();
         string InterfaceProperty { get; }
      }

      class Implementation : Interface
      {
         public string InterfaceProperty => String.Empty;
         public virtual void F() { }
      }

      class NonVirtualBase
      {
         public void F() { }
         public string NonVirtualProperty => String.Empty;
      }

      class NonVirtualDerived : NonVirtualBase
      {
         public new void F() { }
         public new string NonVirtualProperty => String.Empty;
      }

      #endregion


   }
#pragma warning restore 0649, 0067
}
