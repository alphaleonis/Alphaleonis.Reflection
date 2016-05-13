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
         var member = Reflect.GetMember<AttributedTypes.Derived>(c => c.ImplementedMethod1(1, 2));
         Assert.IsNotNull(member);
         Assert.AreEqual(member.DeclaringType, typeof(AttributedTypes.Base));
         Assert.IsInstanceOfType(member, typeof(MethodInfo));

         member = Reflect.GetMember<AttributedTypes.Derived>(c => c.OverriddenMethod(1, 2));
         Assert.IsNotNull(member);
         Assert.AreEqual(member.DeclaringType, typeof(AttributedTypes.Base));
         Assert.IsInstanceOfType(member, typeof(MethodInfo));

         member = Reflect.GetMember<AttributedTypes.SubDerived>(c => c.OverriddenMethod(1, 2));
         Assert.IsNotNull(member);
         Assert.AreEqual(member.DeclaringType, typeof(AttributedTypes.SubDerived ));
         Assert.IsInstanceOfType(member, typeof(MethodInfo));

         member = Reflect.GetMember<AttributedTypes.Derived>(c => (string)c.Field);
         Assert.IsNotNull(member);
         Assert.AreEqual(member.DeclaringType, typeof(AttributedTypes.Derived));
         Assert.IsInstanceOfType(member, typeof(FieldInfo));
      }
   }
}
