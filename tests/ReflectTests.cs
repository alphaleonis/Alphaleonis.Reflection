using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reflection;
using Alphaleonis.Reflection;

namespace Tests.Alphaleonis.Reflection
{
#pragma warning disable 0649,0067

   [TestClass]
   public class ReflectTests
   {
      #region Supporting Reflection Types

      public static object StaticProperty { get; set; }

      public object InstanceProperty { get; set; }

      public static object StaticField;

      public object InstanceField;

      public event EventHandler InstanceEvent;

      public static event EventHandler StaticEvent;

      class BaseClass
      {
         public event EventHandler MyInstanceEvent;

         public virtual event EventHandler MyVirtualInstanceEvent;

         public virtual event EventHandler MyHiddenVirtualInstanceEvent;

         public event EventHandler MyHiddenInstanceEvent;

         public static event EventHandler MyStaticEvent;

         public static object StaticField;

         public object MyHiddenInstanceField;

         public static object MyStaticProperty { get; set; }   
         
         public virtual object VirtualProperty { get; set; }
         
         public virtual object HiddenVirtualProperty { get; set; }

         public void VoidInstanceMethod() => throw new NotImplementedException();

         public virtual void VoidVirtualMethod() => throw new NotImplementedException();

         public int IntOverloadedMethod(string arg1) => 0;

         public int IntOverloadedMethod(int arg1) => 1;

         public void HiddenMethod() => throw new NotImplementedException();

         public virtual void GenericVoidMethod<T>(T a) => throw new NotImplementedException();

         public virtual void GenericVoidMethod<T,U>(T a, U b) => throw new NotImplementedException();

         public virtual T GenericMethod<T>(T a) => throw new NotImplementedException();

         public virtual U GenericMethod<T, U>(T a, U b) => throw new NotImplementedException();
      }

      class SubClass : BaseClass
      {
         public new event EventHandler MyHiddenVirtualInstanceEvent;

         public new event EventHandler MyHiddenInstanceEvent;
         
         public new object MyHiddenInstanceField;

         public object MyProperty { get; set; }

         public new object HiddenVirtualProperty { get; set; }
      }

      class SubSubClass : SubClass
      {
         public override object VirtualProperty { get; set; }

         public override event EventHandler MyVirtualInstanceEvent;

         public override T GenericMethod<T>(T a) => base.GenericMethod(a);
         public override U GenericMethod<T, U>(T a, U b) => base.GenericMethod<T, U>(a, b);
         public override void GenericVoidMethod<T>(T a) => base.GenericVoidMethod(a);
         public override void GenericVoidMethod<T, U>(T a, U b) => base.GenericVoidMethod<T, U>(a, b);         
      }

      class SubSubSubClass : SubSubClass
      {
      }

      class GenericBase<T> 
      {
         public static T MyStaticField;

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
      public void Reflect_GetProperty_NonOverriddenVirtualPropertyFromSubClass_ReturnsExpectedProperty()
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
      public void Reflect_GetProperty_NoArg_NonOverriddenVirtualPropertyFromSubClass_ReturnsExpectedProperty()
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
      public void ReflectT_GetProperty_NonOverriddenVirtualPropertyFromSubClass_ReturnsExpectedProperty()
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

      [TestMethod]
      [ExpectedException(typeof(ArgumentException))]
      public void Reflect_GetField_StaticPropertyFromThisClass_ThrowsArgumentException()
      {
         Reflect.GetField(() => StaticProperty);
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentException))]
      public void Reflect_GetField_InstancePropertyFromThisClass_ThrowsArgumentException()
      {
         Reflect.GetField(() => InstanceProperty);
      }

      [TestMethod]
      public void Reflect_GetField_StaticFieldFromThisClass_ReturnsCorrectField()
      {
         FieldInfo expected = GetType().GetField(nameof(StaticField));
         FieldInfo actual = Reflect.GetField(() => StaticField);
         Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void Reflect_GetField_InstanceFieldFromThisClass_ReturnsCorrectField()
      {
         FieldInfo expected = GetType().GetField(nameof(InstanceField));
         FieldInfo actual = Reflect.GetField(() => InstanceField);
         Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void Reflect_GetField_StaticFieldFromDeclaringClass_ReturnsCorrectField()
      {
         FieldInfo expected = typeof(BaseClass).GetField(nameof(BaseClass.StaticField));
         FieldInfo actual = Reflect.GetField(() => BaseClass.StaticField);
         Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void Reflect_GetField_StaticFieldFromDerivedClass_ReturnsFieldFromDeclaringClass()
      {
         FieldInfo expected = typeof(BaseClass).GetField(nameof(BaseClass.StaticField));
         FieldInfo actual = Reflect.GetField(() => SubClass.StaticField);
         Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void Reflect_GetField_StaticFieldFromClosedGenericBaseClass_ReturnsFieldFromDeclaringClass()
      {
         FieldInfo expected = typeof(GenericBase<object>).GetField(nameof(GenericBase<object>.MyStaticField));
         FieldInfo actual = Reflect.GetField(() => GenericBase<object>.MyStaticField);
         Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentException))]
      public void ReflectT_GetField_InstancePropertyFromThisClass_ThrowsArgumentException()
      {
         Reflect<ReflectTests>.GetField(cls => cls.InstanceProperty);
      }

      #endregion

      #region GetMember Tests

      [TestMethod]
      public void Reflect_GetMember_InstancePropertyFromThisClass_ReturnsExpectedResult()
      {
         var expected = typeof(ReflectTests).GetProperty(nameof(ReflectTests.InstanceProperty));
         var actual = Reflect.GetMember(() => InstanceProperty);
         Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void Reflect_GetMember_StaticPropertyFromThisClass_ReturnsExpectedResult()
      {
         var expected = typeof(ReflectTests).GetProperty(nameof(ReflectTests.StaticProperty));
         var actual = Reflect.GetMember(() => StaticProperty);
         Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void Reflect_GetMember_InstanceFieldFromThisClass_ReturnsExpectedResult()
      {
         var expected = typeof(ReflectTests).GetField(nameof(ReflectTests.InstanceField));
         var actual = Reflect.GetMember(() => InstanceField);
         Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void Reflect_GetMember_StaticFieldFromThisClass_ReturnsExpectedResult()
      {
         var expected = typeof(ReflectTests).GetField(nameof(ReflectTests.StaticField));
         var actual = Reflect.GetMember(() => StaticField);
         Assert.AreEqual(expected, actual);
      }

      #endregion

      #region GetMethod Tests

      [TestMethod]
      public void Reflect_GetMethod_GenericMethodFromBaseClass_ReturnsExpectedValue()
      {
         var expected = typeof(BaseClass).GetMethods()
            .Single(m => m.Name == nameof(BaseClass.GenericMethod) && m.GetGenericArguments().Length == 1);
         var actual = Reflect.GetMethod<BaseClass>(b => b.GenericMethod<int>(0));
         Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void Reflect_GetMethod_GenericMethodWithTwoTypeArgumentsFromBaseClass_ReturnsExpectedValue()
      {
         var expected = typeof(BaseClass).GetMethods()
            .Single(m => m.Name == nameof(BaseClass.GenericMethod) && m.GetGenericArguments().Length == 2);
         var actual = Reflect.GetMethod<BaseClass>(b => b.GenericMethod<int,string>(0,""));
         Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void Reflect_GetMethod_GenericVoidMethodFromBaseClass_ReturnsExpectedValue()
      {
         var expected = typeof(BaseClass).GetMethods()
            .Single(m => m.Name == nameof(BaseClass.GenericVoidMethod) && m.GetGenericArguments().Length == 1);
         var actual = Reflect.GetMethod<BaseClass>(b => b.GenericVoidMethod<int>(0));
         Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void Reflect_GetMethod_GenericVoidMethodWithTwoTypeArgumentsFromBaseClass_ReturnsExpectedValue()
      {
         var expected = typeof(BaseClass).GetMethods()
            .Single(m => m.Name == nameof(BaseClass.GenericVoidMethod) && m.GetGenericArguments().Length == 2);
         var actual = Reflect.GetMethod<BaseClass>(b => b.GenericVoidMethod<int, string>(0, ""));
         Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void Reflect_GetMethod_GenericMethodFromChildClass_ReturnsExpectedValue()
      {
         var expected = typeof(SubClass).GetMethods()
            .Single(m => m.Name == nameof(SubClass.GenericMethod) && m.GetGenericArguments().Length == 1);
         var actual = Reflect.GetMethod<SubClass>(b => b.GenericMethod<int>(0));
         Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void Reflect_GetMethod_GenericMethodWithTwoTypeArgumentsFromChildClass_ReturnsExpectedValue()
      {
         var expected = typeof(SubClass).GetMethods()
            .Single(m => m.Name == nameof(SubClass.GenericMethod) && m.GetGenericArguments().Length == 2);
         var actual = Reflect.GetMethod<SubClass>(b => b.GenericMethod<int, string>(0, ""));
         Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void Reflect_GetMethod_GenericVoidMethodFromChildClass_ReturnsExpectedValue()
      {
         var expected = typeof(SubClass).GetMethods()
            .Single(m => m.Name == nameof(SubClass.GenericVoidMethod) && m.GetGenericArguments().Length == 1);
         var actual = Reflect.GetMethod<SubClass>(b => b.GenericVoidMethod<int>(0));
         Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void Reflect_GetMethod_GenericVoidMethodWithTwoTypeArgumentsFromChildClass_ReturnsExpectedValue()
      {
         var expected = typeof(SubClass).GetMethods()
            .Single(m => m.Name == nameof(SubClass.GenericVoidMethod) && m.GetGenericArguments().Length == 2);
         var actual = Reflect.GetMethod<SubClass>(b => b.GenericVoidMethod<int, string>(0, ""));
         Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void Reflect_GetMethod_OverriddenGenericMethodFromChildClass_ReturnsExpectedValue()
      {
         var expected = typeof(SubSubClass).GetMethods()
            .Single(m => m.Name == nameof(SubSubClass.GenericMethod) && m.GetGenericArguments().Length == 1);
         var actual = Reflect.GetMethod<SubSubClass>(b => b.GenericMethod<int>(0));
         Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void Reflect_GetMethod_OverriddenGenericMethodWithTwoTypeArgumentsFromChildClass_ReturnsExpectedValue()
      {
         var expected = typeof(SubSubClass).GetMethods()
            .Single(m => m.Name == nameof(SubSubClass.GenericMethod) && m.GetGenericArguments().Length == 2);
         var actual = Reflect.GetMethod<SubSubClass>(b => b.GenericMethod<int, string>(0, ""));
         Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void Reflect_GetMethod_OverriddenGenericVoidMethodFromChildClass_ReturnsExpectedValue()
      {
         var expected = typeof(SubSubClass).GetMethods()
            .Single(m => m.Name == nameof(SubSubClass.GenericVoidMethod) && m.GetGenericArguments().Length == 1);
         var actual = Reflect.GetMethod<SubSubClass>(b => b.GenericVoidMethod<int>(0));
         Assert.AreEqual(expected, actual);
      }

      [TestMethod]
      public void Reflect_GetMethod_OverriddenGenericVoidMethodWithTwoTypeArgumentsFromChildClass_ReturnsExpectedValue()
      {
         var expected = typeof(SubSubClass).GetMethods()
            .Single(m => m.Name == nameof(SubSubClass.GenericVoidMethod) && m.GetGenericArguments().Length == 2);
         var actual = Reflect.GetMethod<SubSubClass>(b => b.GenericVoidMethod<int, string>(0, ""));
         Assert.AreEqual(expected, actual);
      }

      #endregion
   }
#pragma warning restore 0649,0067

}
