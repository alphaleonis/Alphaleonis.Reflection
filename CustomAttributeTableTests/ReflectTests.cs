using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CustomAttributeTable;
using System.Reflection;

namespace CustomAttributeTableTests
{
   [TestClass]
   public class ReflectTests
   {
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
