// Copyright (c) Peter Palotas
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Alphaleonis.Reflection;
using Alphaleonis.Reflection.Context;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Tests.Alphaleonis.Reflection
{
   [TestClass]
   public class ReflectionTableBuilderTests
   {
      [TestMethod]
      [ExpectedException(typeof(ArgumentException))]
      public void AddMemberAttributes_MethodFromBaseClassIsNotOverriddenInSpecifiedClass_ThrowsArgumentException()
      {
         ReflectionTableBuilder builder = new ReflectionTableBuilder();
         builder.AddMemberAttributes<UndecoratedTypes.Derived>(m => m.GenericMethod(0, ""), new[] { new NonInheritedSingleAttribute() });
      }

      [TestMethod]
      [ExpectedException(typeof(ArgumentException))]
      public void AddReturnParameterAttributes_MethodFromBaseClassIsNotOverriddenInSpecifiedClass_ThrowsArgumentException()
      {
         ReflectionTableBuilder builder = new ReflectionTableBuilder();
         builder.AddReturnParameterAttributes<UndecoratedTypes.Derived>(m => m.GenericMethod(0, ""), new NonInheritedSingleAttribute());
      }
   }
}
