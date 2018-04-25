using Alphaleonis.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Tests.Alphaleonis.Reflection
{
   [TestClass]
   public class AttributeTableBuilderTests
   {
      [TestMethod]
      [ExpectedException(typeof(ArgumentException))]
      public void AddMemberAttributes_MethodFromBaseClassIsNotOverriddenInSpecifiedClass_ThrowsArgumentException()
      {
         AttributeTableBuilder builder = new AttributeTableBuilder();
         builder.AddMemberAttributes<UndecoratedTypes.Derived>(m => m.GenericMethod(0, ""), new[] { new NonInheritedSingleAttribute() });
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentException))]
      public void AddReturnParameterAttributes_MethodFromBaseClassIsNotOverriddenInSpecifiedClass_ThrowsArgumentException()
      {
         AttributeTableBuilder builder = new AttributeTableBuilder();
         builder.AddReturnParameterAttributes<UndecoratedTypes.Derived>(m => m.GenericMethod(0, ""), new NonInheritedSingleAttribute());
      }
   }
}
