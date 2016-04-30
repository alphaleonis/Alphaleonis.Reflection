using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CustomAttributeTable;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

namespace CustomAttributeTableTests
{
   [TestClass]
   public class UnitTest1
   {
      private IEnumerable<TestAttribute> CreateTestAttributes<T>()
      {
         return CreateTestAttributes(typeof(T).Name);
      }

      private IEnumerable<TestAttribute> CreateTestAttributes(string name)
      {
         return new TestAttribute[] {
               new InheritedMultiAttribute(nameof(NonAttributedTypes.IBase1)),
               new InheritedSingleAttribute(nameof(NonAttributedTypes.IBase1)),
               new NonInheritedMultiAttribute(nameof(NonAttributedTypes.IBase1)),
               new NonInheritedSingleAttribute(nameof(NonAttributedTypes.IBase1))
         };
      }

      private ICustomAttributeTable CreateTable()
      {
         CustomAttributeTableBuilder builder = new CustomAttributeTableBuilder();
         builder
            .AddTypeAttributes<NonAttributedTypes.IBase1>(CreateTestAttributes<NonAttributedTypes.IBase1>())
            
            .AddTypeAttributes<NonAttributedTypes.IBase2>(
               new InheritedMultiAttribute(nameof(NonAttributedTypes.IBase2)),
               new InheritedSingleAttribute(nameof(NonAttributedTypes.IBase2)),
               new NonInheritedMultiAttribute(nameof(NonAttributedTypes.IBase2)),
               new NonInheritedSingleAttribute(nameof(NonAttributedTypes.IBase2)))

            .AddTypeAttributes<NonAttributedTypes.IComposite>(
               new InheritedMultiAttribute(nameof(NonAttributedTypes.IComposite)),
               new InheritedSingleAttribute(nameof(NonAttributedTypes.IComposite)),
               new NonInheritedMultiAttribute(nameof(NonAttributedTypes.IComposite)),
               new NonInheritedSingleAttribute(nameof(NonAttributedTypes.IComposite)))

            .AddTypeAttributes<NonAttributedTypes.Base>(
               new InheritedMultiAttribute(nameof(NonAttributedTypes.Base)),
               new InheritedSingleAttribute(nameof(NonAttributedTypes.Base)),
               new NonInheritedMultiAttribute(nameof(NonAttributedTypes.Base)),
               new NonInheritedSingleAttribute(nameof(NonAttributedTypes.Base)))

            .AddMemberAttributes(Reflect<NonAttributedTypes.Base>.GetProperty(p => p.OverriddenProperty), CreateTestAttributes<NonAttributedTypes.Base>())

            .AddTypeAttributes<NonAttributedTypes.Derived>(
               new InheritedMultiAttribute(nameof(NonAttributedTypes.Derived)),
               new InheritedSingleAttribute(nameof(NonAttributedTypes.Derived)),
               new NonInheritedMultiAttribute(nameof(NonAttributedTypes.Derived)),
               new NonInheritedSingleAttribute(nameof(NonAttributedTypes.Derived)))

            .AddTypeAttributes<NonAttributedTypes.SubDerived>(
               new InheritedMultiAttribute(nameof(NonAttributedTypes.SubDerived)),
               new InheritedSingleAttribute(nameof(NonAttributedTypes.SubDerived)),
               new NonInheritedMultiAttribute(nameof(NonAttributedTypes.SubDerived)),
               new NonInheritedSingleAttribute(nameof(NonAttributedTypes.SubDerived)));

         return builder.CreateTable();
      }

      [TestMethod]
      public void TestMethod1()
      {
         AttributeTableReflectionContext context = new AttributeTableReflectionContext(CreateTable());

         Check<AttributedTypes.IBase1, NonAttributedTypes.IBase1>(context);
         Check<AttributedTypes.IBase2, NonAttributedTypes.IBase2>(context);
         Check<AttributedTypes.IComposite, NonAttributedTypes.IComposite>(context);
         Check<AttributedTypes.Base, NonAttributedTypes.Base>(context);
         Check<AttributedTypes.Derived, NonAttributedTypes.Derived>(context);
         Check<AttributedTypes.SubDerived, NonAttributedTypes.SubDerived>(context);
      }


      [TestMethod]
      public void TestMethod2()
      {
         MethodInfo m = Reflect<NonAttributedTypes.Base>.GetMethod((NonAttributedTypes.Base b) => b.ImplementedMethod1(1, 2));
         //m = Reflect<MyClass>.GetProperty(c => c.Name);
         m = Reflect<List<int>>.GetMethod(s => s.AddRange(new int[] { }));

         Console.WriteLine(m);
      }

      private void Check<SourceType, TargetType>(ReflectionContext context)
      {
         Type sourceType = typeof(SourceType);
         Type targetType = context.MapType(typeof(TargetType).GetTypeInfo());

         SequenceAssert.AreEquivalent(sourceType.GetCustomAttributes(true), targetType.GetCustomAttributes(true).Where(attr => !(attr is AttributeTableReflectionContextIdentifierAttribute)), $"Attribute mismatch on type {sourceType.Name} (inherit=true)");
         SequenceAssert.AreEquivalent(sourceType.GetCustomAttributes(false), targetType.GetCustomAttributes(false).Where(attr => !(attr is AttributeTableReflectionContextIdentifierAttribute)), $"Attribute mismatch on type {sourceType.Name} (inherit=false)");
      }
   }
}
