using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using Alphaleonis.Reflection;

namespace Tests.Alphaleonis.Reflection
{

   [TestClass]
   public class ReflectTests
   {
      #region Supporting Reflection Types

      public static object StaticProperty { get; set; }

      public object InstanceProperty { get; set; }

      public static object StaticField;

      public object InstanceField;

      class BaseClass
      {         
         public static object MyStaticProperty { get; set; }   
         
         public virtual object VirtualProperty { get; set; }
         
         public virtual object HiddenVirtualProperty { get; set; }         
      }

      class SubClass : BaseClass
      {
         public object MyProperty { get; set; }

         public new object HiddenVirtualProperty { get; set; }
      }

      class SubSubClass : SubClass
      {
         public override object VirtualProperty { get; set; }
      }

      class SubSubSubClass : SubSubClass
      {
      }

      class GenericBase<T> 
      {
         public static T MyStaticProperty { get; set; }
      }

      #endregion

      #region GetProperty Tests

      [TestMethod]
      [ExpectedException(typeof(ArgumentException))]
      public void Reflect_GetProperty_StaticFieldFromThisClass_ThrowsArgumentException()
      {
         Reflect.GetProperty(() => StaticField);         
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentException))]
      public void Reflect_GetProperty_InstanceFieldFromThisClass_ThrowsArgumentException()
      {
         Reflect.GetProperty(() => InstanceField);
      }

      [TestMethod]
      public void Reflect_GetProperty_StaticPropertyFromThisClass_ReturnsCorrectProperty()
      {
         PropertyInfo expected = GetType().GetProperty(nameof(StaticProperty));
         PropertyInfo actual = Reflect.GetProperty(() => StaticProperty);
         Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void Reflect_GetProperty_InstancePropertyFromThisClass_ReturnsCorrectProperty()
      {
         PropertyInfo expected = GetType().GetProperty(nameof(InstanceProperty));
         PropertyInfo actual = Reflect.GetProperty(() => InstanceProperty);
         Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void Reflect_GetProperty_StaticPropertyFromDeclaringClass_ReturnsCorrectProperty()
      {
         PropertyInfo expected = typeof(BaseClass).GetProperty(nameof(BaseClass.MyStaticProperty));
         PropertyInfo actual = Reflect.GetProperty(() => SubClass.MyStaticProperty);
         Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void Reflect_GetProperty_StaticPropertyFromDerivedClass_ReturnsPropertyFromDeclaringClass()
      {
         PropertyInfo expected = typeof(BaseClass).GetProperty(nameof(BaseClass.MyStaticProperty));
         PropertyInfo actual = Reflect.GetProperty(() => SubClass.MyStaticProperty);
         Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void Reflect_GetProperty_StaticPropertyFromClosedGenericBaseClass_ReturnsPropertyFromDeclaringClass()
      {
         PropertyInfo expected = typeof(GenericBase<object>).GetProperty(nameof(GenericBase<object>.MyStaticProperty));
         PropertyInfo actual = Reflect.GetProperty(() => GenericBase<object>.MyStaticProperty);
         Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void Reflect_GetProperty_VirtualPropertyFromBaseClass_ReturnsPropertyFromBaseClass()
      {
         PropertyInfo expected = typeof(BaseClass).GetProperty(nameof(BaseClass.VirtualProperty));
         PropertyInfo actual = Reflect.GetProperty((BaseClass cls) => cls.VirtualProperty);
         Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void Reflect_GetProperty_NonOverriddeVirtualPropertyFromSubClass_ReturnsExpectedProperty()
      {
         PropertyInfo expected = typeof(SubClass).GetProperty(nameof(SubClass.VirtualProperty));
         PropertyInfo actual = Reflect.GetProperty((SubClass cls) => cls.VirtualProperty);
         Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void Reflect_GetProperty_OverriddenVirtualPropertyFromSubSubClass_ReturnsExpectedProperty()
      {
         PropertyInfo expected = typeof(SubSubClass).GetProperty(nameof(SubSubClass.VirtualProperty));
         PropertyInfo actual = Reflect.GetProperty((SubSubClass cls) => cls.VirtualProperty);
         Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void Reflect_GetProperty_NonOverriddenVirtualPropertyFromSubSubSubClass_ReturnsExpectedProperty()
      {
         PropertyInfo expected = typeof(SubSubSubClass).GetProperty(nameof(SubSubSubClass.VirtualProperty));
         PropertyInfo actual = Reflect.GetProperty((SubSubSubClass cls) => cls.VirtualProperty);
         Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void Reflect_GetProperty_HiddenVirtualPropertyFromSubClass_ReturnsExpectedProperty()
      {
         PropertyInfo expected = typeof(SubClass).GetProperty(nameof(SubClass.HiddenVirtualProperty));
         PropertyInfo actual = Reflect.GetProperty((SubClass cls) => cls.HiddenVirtualProperty);
         Assert.AreEqual(expected, actual);
      }


      [TestMethod]
      public void Reflect_GetProperty_NoArg_VirtualPropertyFromBaseClass_ReturnsPropertyFromBaseClass()
      {
         PropertyInfo expected = typeof(BaseClass).GetProperty(nameof(BaseClass.VirtualProperty));
         BaseClass cls = new BaseClass();
         PropertyInfo actual = Reflect.GetProperty(() => cls.VirtualProperty);
         Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void Reflect_GetProperty_NoArg_NonOverriddeVirtualPropertyFromSubClass_ReturnsExpectedProperty()
      {
         PropertyInfo expected = typeof(SubClass).GetProperty(nameof(SubClass.VirtualProperty));
         SubClass cls = new SubClass();
         PropertyInfo actual = Reflect.GetProperty(() => cls.VirtualProperty);
         Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void Reflect_GetProperty_NoArg_OverriddenVirtualPropertyFromSubSubClass_ReturnsExpectedProperty()
      {
         PropertyInfo expected = typeof(SubSubClass).GetProperty(nameof(SubSubClass.VirtualProperty));
         SubSubClass cls = new SubSubClass();
         PropertyInfo actual = Reflect.GetProperty(() => cls.VirtualProperty);
         Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void Reflect_GetProperty_NoArg_NonOverriddenVirtualPropertyFromSubSubSubClass_ReturnsExpectedProperty()
      {
         PropertyInfo expected = typeof(SubSubSubClass).GetProperty(nameof(SubSubSubClass.VirtualProperty));
         SubSubSubClass cls = new SubSubSubClass();
         PropertyInfo actual = Reflect.GetProperty(() => cls.VirtualProperty);
         Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void Reflect_GetProperty_NoArg_HiddenVirtualPropertyFromSubClass_ReturnsExpectedProperty()
      {
         PropertyInfo expected = typeof(SubClass).GetProperty(nameof(SubClass.HiddenVirtualProperty));
         SubClass cls = new SubClass();
         PropertyInfo actual = Reflect.GetProperty(() => cls.HiddenVirtualProperty);
         Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void ReflectT_GetProperty_VirtualPropertyFromBaseClass_ReturnsPropertyFromBaseClass()
      {
         PropertyInfo expected = typeof(BaseClass).GetProperty(nameof(BaseClass.VirtualProperty));
         PropertyInfo actual = Reflect<BaseClass>.GetProperty(cls => cls.VirtualProperty);
         Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void ReflectT_GetProperty_NonOverriddeVirtualPropertyFromSubClass_ReturnsExpectedProperty()
      {
         PropertyInfo expected = typeof(SubClass).GetProperty(nameof(SubClass.VirtualProperty));
         PropertyInfo actual = Reflect<SubClass>.GetProperty(cls => cls.VirtualProperty);
         Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void ReflectT_GetProperty_OverriddenVirtualPropertyFromSubSubClass_ReturnsExpectedProperty()
      {
         PropertyInfo expected = typeof(SubSubClass).GetProperty(nameof(SubSubClass.VirtualProperty));
         PropertyInfo actual = Reflect<SubSubClass>.GetProperty(cls => cls.VirtualProperty);
         Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void ReflectT_GetProperty_NonOverriddenVirtualPropertyFromSubSubSubClass_ReturnsExpectedProperty()
      {
         PropertyInfo expected = typeof(SubSubSubClass).GetProperty(nameof(SubSubSubClass.VirtualProperty));
         PropertyInfo actual = Reflect<SubSubSubClass>.GetProperty(cls => cls.VirtualProperty);
         Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void ReflectT_GetProperty_HiddenVirtualPropertyFromSubClass_ReturnsExpectedProperty()
      {
         PropertyInfo expected = typeof(SubClass).GetProperty(nameof(SubClass.HiddenVirtualProperty));
         PropertyInfo actual = Reflect<SubClass>.GetProperty(cls => cls.HiddenVirtualProperty);
         Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentException))]
      public void ReflectT_GetProperty_InstanceFieldFromThisClass_ThrowsArgumentException()
      {
         Reflect<ReflectTests>.GetProperty(cls => cls.InstanceField);
      }

      #endregion

      #region GetField Tests

      // TODO PP: Implement

      #endregion

      #region GetEvent Tests

      // TODO PP: Implement

      #endregion

      #region GetMethod Tests

      [TestMethod]
      public void Test3()
      {         
         var mm = Reflect.GetMethod<DecoratedTypes.Derived>(m => m.GenericMethod<string>(1, ""));
      }

      #endregion

      #region GetMember Tests

      // TODO PP: Implement

      #endregion
      // TODO: Add a lot of tests here, esp. with regards to generics/non-generics.


      [TestMethod]
      public void Test()
      {

         var member = Reflect.GetMember<DecoratedTypes.Derived>(c => c.ImplementedMethod1(1, 2));
         Assert.IsNotNull(member);
         Assert.AreEqual(member.DeclaringType, typeof(DecoratedTypes.Base));
         Assert.IsInstanceOfType(member, typeof(MethodInfo));

         member = Reflect.GetMember<DecoratedTypes.Derived>(c => c.OverriddenMethod(1, 2));
         Assert.IsNotNull(member);
         Assert.AreEqual(member.DeclaringType, typeof(DecoratedTypes.Base));
         Assert.IsInstanceOfType(member, typeof(MethodInfo));

         member = Reflect.GetMember<DecoratedTypes.SubDerived>(c => c.OverriddenMethod(1, 2));
         Assert.IsNotNull(member);
         Assert.AreEqual(member.DeclaringType, typeof(DecoratedTypes.SubDerived ));
         Assert.IsInstanceOfType(member, typeof(MethodInfo));

         member = Reflect.GetMember<DecoratedTypes.Derived>(c => c.m_derivedField);
         Assert.IsNotNull(member);
         Assert.AreEqual(member.DeclaringType, typeof(DecoratedTypes.Derived));
         Assert.IsInstanceOfType(member, typeof(FieldInfo));
      }
   }
}
