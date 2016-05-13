using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CustomAttributeTableTests
{
   

   [TestClass]
   public class MemberInfoExtensionTests
   {
      #region EventInfo.IsOverride() Tests

      [TestMethod]
      public void EventInfo_NonOverriden_ReturnsTrue()
      {
         Assert.IsFalse(typeof(Base).GetEvent(nameof(Base.OverriddenEvent)).IsOverride());
      }

      [TestMethod]
      public void EventInfo_Overriden_ReturnsTrue()
      {
         Assert.IsTrue(typeof(Derived).GetEvent(nameof(Derived.OverriddenEvent)).IsOverride());
      }

      [TestMethod]
      public void EventInfo_Inherited_ReturnsFalse()
      {
         Assert.IsFalse(typeof(Derived).GetEvent(nameof(Derived.InheritedEvent)).IsOverride());
      }

      [TestMethod]
      public void EventInfo_Hidden_ReturnsFalse()
      {
         Assert.IsFalse(typeof(Derived).GetEvent(nameof(Derived.HiddenEvent)).IsOverride());
      }


      #endregion

      #region PropertyInfo.IsOverride() Tests

      [TestMethod]
      public void PropertyInfo_NonOverridden_ReturnsFalse()
      {
         Assert.IsFalse(typeof(Base).GetProperty(nameof(Derived.InheritedProperty)).IsOverride());
      }

      [TestMethod]
      public void PropertyInfo_Overriden_ReturnsTrue()
      {
         Assert.IsTrue(typeof(Derived).GetProperty(nameof(Derived.OverriddenProperty)).IsOverride());
      }

      [TestMethod]
      public void PropertyInfo_Inherited_ReturnsFalse()
      {
         Assert.IsFalse(typeof(Derived).GetProperty(nameof(Derived.InheritedProperty)).IsOverride());
      }

      [TestMethod]
      public void PropertyInfo_Hidden_ReturnsFalse()
      {
         Assert.IsFalse(typeof(NonVirtualDerived).GetProperty(nameof(NonVirtualDerived.NonVirtualProperty)).IsOverride());
      }

      [TestMethod]
      public void PropertyInfo_InterfaceImplementation_ReturnsFalse()
      {
         Assert.IsFalse(typeof(Implementation).GetProperty(nameof(Implementation.InterfaceProperty)).IsOverride());
      }

      [TestMethod]
      public void PropertyInfo_AbstractOverriden_ReturnsTrue()
      {
         Assert.IsTrue(typeof(Derived).GetProperty(nameof(Derived.AbstractOverriddenProperty)).IsOverride());
      }

      [TestMethod]
      public void PropertyInfo_AbstractInherited_ReturnsFalse()
      {
         Assert.IsFalse(typeof(Derived).GetProperty(nameof(Derived.AbstractNonOverriddenProperty)).IsOverride());
      }

      [TestMethod]
      public void PropertyInfo_SelaedOverriden_ReturnsTrue()
      {
         Assert.IsTrue(typeof(Derived).GetProperty(nameof(Derived.SealedOverriddenProperty)).IsOverride());
      }

      #endregion

      #region MethodInfo.IsOverride() tests

      [TestMethod]
      public void MethodInfo_Overriden_ReturnsTrue()
      {
         Assert.IsTrue(typeof(Derived).GetMethod(nameof(Derived.OverriddenMethod)).IsOverride());
      }

      [TestMethod]
      public void MethodInfo_SealedOverriden_ReturnsTrue()
      {
         Assert.IsTrue(typeof(Derived).GetMethod(nameof(Derived.SealedOverriddenMethod)).IsOverride());
      }

      [TestMethod]
      public void MethodInfo_Inherited_ReturnsFalse()
      {
         Assert.IsFalse(typeof(Derived).GetMethod(nameof(Derived.InheritedMethod)).IsOverride());
      }

      [TestMethod]
      public void MethodInfo_Hidden_ReturnsFalse()
      {
         Assert.IsFalse(typeof(NonVirtualDerived).GetMethod(nameof(NonVirtualDerived.F)).IsOverride());
      }

      [TestMethod]
      public void MethodInfo_InterfaceImplementation_ReturnsFalse()
      {
         Assert.IsFalse(typeof(Implementation).GetMethod(nameof(NonVirtualDerived.F)).IsOverride());
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
}
