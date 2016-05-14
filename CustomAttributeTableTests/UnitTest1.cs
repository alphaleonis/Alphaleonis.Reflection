using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CustomAttributeTable;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

namespace CustomAttributeTableTests
{
   [TestClass]
   public class AttributeTableReflectionContextTests
   {
      private IEnumerable<TestAttribute> CreateTestAttributes<T>()
      {
         return CreateTestAttributes(typeof(T).Name);
      }

      private IEnumerable<TestAttribute> CreateTestAttributes(string name)
      {
         return new TestAttribute[] {
               new InheritedMultiAttribute(name),
               new InheritedSingleAttribute(name),
               new NonInheritedMultiAttribute(name),
               new NonInheritedSingleAttribute(name)
         };
      }

      private ICustomAttributeTable CreateTable()
      {
         CustomAttributeTableBuilder builder = new CustomAttributeTableBuilder();
         builder
            .AddTypeAttributes<NonAttributedTypes.IBase1>(CreateTestAttributes<NonAttributedTypes.IBase1>())
               .AddPropertyAttributes<NonAttributedTypes.IBase1>(nameof(NonAttributedTypes.IBase1.ImplementedProperty), CreateTestAttributes<NonAttributedTypes.IBase1>())
               .AddPropertyAttributes<NonAttributedTypes.IBase1>(nameof(NonAttributedTypes.IBase1.HiddenProperty), CreateTestAttributes<NonAttributedTypes.IBase1>())
               .AddMemberAttributes<NonAttributedTypes.IBase1>(c => c.ImplementedMethod1(0, 0), CreateTestAttributes<NonAttributedTypes.IBase1>())
               .AddMemberAttributes<NonAttributedTypes.IBase1>(c => c.HiddenMethod1(0), CreateTestAttributes<NonAttributedTypes.IBase1>())
               .AddMemberAttributes<NonAttributedTypes.IBase1>(c => c.OverloadedMethod(default(int)), CreateTestAttributes(nameof(NonAttributedTypes.IBase1) + "int"))
               .AddMemberAttributes<NonAttributedTypes.IBase1>(c => c.OverloadedMethod(default(long)), CreateTestAttributes(nameof(NonAttributedTypes.IBase1) + "long"))

            .AddTypeAttributes<NonAttributedTypes.IBase2>(CreateTestAttributes<NonAttributedTypes.IBase2>())
               .AddMemberAttributes<NonAttributedTypes.IBase2>(c => c.ImplementedMethod1(0, 0), CreateTestAttributes<NonAttributedTypes.IBase2>())

            .AddTypeAttributes<NonAttributedTypes.IComposite>(CreateTestAttributes<NonAttributedTypes.IComposite>())
               .AddPropertyAttributes<NonAttributedTypes.IComposite>(nameof(NonAttributedTypes.IComposite.HiddenProperty), CreateTestAttributes<NonAttributedTypes.IComposite>())
               .AddMemberAttributes<NonAttributedTypes.IComposite>(c => c.HiddenMethod1(0), CreateTestAttributes<NonAttributedTypes.IComposite>())

            .AddTypeAttributes<NonAttributedTypes.Base>(CreateTestAttributes<NonAttributedTypes.Base>())
               .AddMemberAttributes<NonAttributedTypes.Base>(c => c.HiddenProperty, CreateTestAttributes<NonAttributedTypes.Base>())
               .AddMemberAttributes<NonAttributedTypes.Base>(c => c.ImplementedProperty, CreateTestAttributes<NonAttributedTypes.Base>())
               .AddMemberAttributes<NonAttributedTypes.Base>(c => c.OverriddenProperty, CreateTestAttributes<NonAttributedTypes.Base>())
               .AddMemberAttributes<NonAttributedTypes.Base>(c => c.OverriddenProperty2, CreateTestAttributes<NonAttributedTypes.Base>())
               .AddMemberAttributes<NonAttributedTypes.Base>(c => c.HiddenMethod1(0), CreateTestAttributes<NonAttributedTypes.Base>())
               .AddMemberAttributes<NonAttributedTypes.Base>(c => c.ImplementedMethod1(0, 0), CreateTestAttributes<NonAttributedTypes.Base>())
               .AddMemberAttributes<NonAttributedTypes.Base>(c => c.OverriddenMethod(0, 0), CreateTestAttributes<NonAttributedTypes.Base>())
               .AddMemberAttributes<NonAttributedTypes.Base>(c => c.OverloadedMethod(default(int)), CreateTestAttributes(nameof(NonAttributedTypes.Base) + "int"))
               .AddMemberAttributes<NonAttributedTypes.Base>(c => c.OverloadedMethod(default(long)), CreateTestAttributes(nameof(NonAttributedTypes.Base) + "long"))

            .AddTypeAttributes<NonAttributedTypes.Derived>(CreateTestAttributes<NonAttributedTypes.Derived>())
               .AddMemberAttributes<NonAttributedTypes.Derived>(der => der.HiddenProperty, CreateTestAttributes<NonAttributedTypes.Derived>())
               .AddMemberAttributes<NonAttributedTypes.Derived>(der => der.OverriddenProperty2, CreateTestAttributes<NonAttributedTypes.Derived>())

            .AddTypeAttributes<NonAttributedTypes.SubDerived>(CreateTestAttributes<NonAttributedTypes.SubDerived>())
               .AddMemberAttributes<NonAttributedTypes.SubDerived>(der => der.OverriddenProperty, CreateTestAttributes<NonAttributedTypes.SubDerived>())
               .AddMemberAttributes<NonAttributedTypes.SubDerived>(der => der.OverriddenProperty2, CreateTestAttributes<NonAttributedTypes.SubDerived>())
               .AddMemberAttributes<NonAttributedTypes.SubDerived>(c => c.OverloadedMethod(default(int)), CreateTestAttributes(nameof(NonAttributedTypes.SubDerived) + "int"))
               .AddMemberAttributes<NonAttributedTypes.SubDerived>(c => c.OverloadedMethod(default(long)), CreateTestAttributes(nameof(NonAttributedTypes.SubDerived) + "long"));
         builder
               .AddMemberAttributes<NonAttributedTypes.SubDerived>(c => c.OverriddenMethod(0,0), CreateTestAttributes(nameof(NonAttributedTypes.SubDerived)))


         ;
         return builder.CreateTable();
      }

      [TestMethod]
      public void TestMethod1()
      {
         //var props = typeof(AttributedTypes.SubDerived).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
         //var attrs = typeof(AttributedTypes.SubDerived).GetProperty("OverriddenProeprty").GetCustomAttributes(true);
         AttributeTableReflectionContext context = new AttributeTableReflectionContext(CreateTable(), AttributeTableReflectionContextOptions.Default);

         //Check<AttributedTypes.IBase1, NonAttributedTypes.IBase1>(context, false);
         //Check<AttributedTypes.IBase2, NonAttributedTypes.IBase2>(context, false);
         //Check<AttributedTypes.IComposite, NonAttributedTypes.IComposite>(context, false);
         //Check<AttributedTypes.Base, NonAttributedTypes.Base>(context, false);
         //Check<AttributedTypes.Derived, NonAttributedTypes.Derived>(context, false);
         //Check<AttributedTypes.SubDerived, NonAttributedTypes.SubDerived>(context, false);

         context = new AttributeTableReflectionContext(CreateTable(), AttributeTableReflectionContextOptions.HonorPropertyAttributeInheritance | AttributeTableReflectionContextOptions.HonorEventAttributeInheritance);
         Check<AttributedTypes.IBase1, NonAttributedTypes.IBase1>(context, true);
         Check<AttributedTypes.IBase2, NonAttributedTypes.IBase2>(context, true);
         Check<AttributedTypes.IComposite, NonAttributedTypes.IComposite>(context, true);
         Check<AttributedTypes.Base, NonAttributedTypes.Base>(context, true);
         Check<AttributedTypes.Derived, NonAttributedTypes.Derived>(context, true);
         Check<AttributedTypes.SubDerived, NonAttributedTypes.SubDerived>(context, true);
      }


      [TestMethod]
      public void TestMethod2()
      {
         MethodInfo m = Reflect<NonAttributedTypes.Base>.GetMethod((NonAttributedTypes.Base b) => b.ImplementedMethod1(1, 2));
         //m = Reflect<MyClass>.GetProperty(c => c.Name);
         m = Reflect<List<int>>.GetMethod(s => s.AddRange(new int[] { }));

         Console.WriteLine(m);
      }

      private void Check<SourceType, TargetType>(ReflectionContext context, bool honorInheritance)
      {
         Type sourceType = typeof(SourceType);
         Type targetType = context.MapType(typeof(TargetType).GetTypeInfo());

         SequenceAssert.AreEquivalent(sourceType.GetCustomAttributes(true), FilterAttributes(targetType.GetCustomAttributes(true)), $"Attribute mismatch on type {sourceType.Name} (inherit=true)");
         SequenceAssert.AreEquivalent(sourceType.GetCustomAttributes(false), FilterAttributes(targetType.GetCustomAttributes(false)), $"Attribute mismatch on type {sourceType.Name} (inherit=false)");

         foreach (var sourceProperty in sourceType.GetProperties(BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public))
         {
            var targetProperty = targetType.GetProperty(sourceProperty.Name, sourceProperty.PropertyType, sourceProperty.GetIndexParameters().Select(p => p.ParameterType).ToArray());

            Assert.IsNotNull(targetProperty, $"Property {sourceProperty.Name} was not found in class {targetType.Name}");
            SequenceAssert.AreEquivalent(sourceProperty.GetCustomAttributes(false), FilterAttributes(targetProperty.GetCustomAttributes(false)), $"Attribute mismatch och property {sourceProperty.DeclaringType.Name}.{sourceProperty.Name} (inherit=false)");
            object[] expectedInherited = honorInheritance ? Attribute.GetCustomAttributes(sourceProperty, true) : sourceProperty.GetCustomAttributes(true);
            SequenceAssert.AreEquivalent(expectedInherited, FilterAttributes(targetProperty.GetCustomAttributes(true)), $"Attribute mismatch och property {sourceProperty.DeclaringType.Name}.{sourceProperty.Name} (inherit=true)");
         }

         foreach (var sourceMethod in sourceType.GetMethods(BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).Where(m => m.IsSpecialName == false && !m.DeclaringType.Equals(typeof(object))))
         {
            var targetMethod = targetType.GetMethod(sourceMethod.Name, sourceMethod.GetParameters().Select(p => p.ParameterType).ToArray());
            Assert.IsNotNull(targetMethod, $"Method {sourceMethod.Name} was not found in class {targetType.Name}");
            SequenceAssert.AreEquivalent(sourceMethod.GetCustomAttributes(false), FilterAttributes(targetMethod.GetCustomAttributes(false)), $"Attribute mismatch och method {sourceMethod.DeclaringType.Name}.{sourceMethod.Name}({String.Join(",", sourceMethod.GetParameters().Select(p => p.ParameterType.Name))}) (inherit=false)");
            SequenceAssert.AreEquivalent(sourceMethod.GetCustomAttributes(true), FilterAttributes(targetMethod.GetCustomAttributes(true)), $"Attribute mismatch och method {sourceMethod.DeclaringType.Name}.{sourceMethod.Name}({String.Join(",", sourceMethod.GetParameters().Select(p => p.ParameterType.Name))}) (inherit=true)");
         }
      }

      private IEnumerable<T> FilterAttributes<T>(IEnumerable<T> list)
      {
         return list.Where(attr => !(attr is AttributeTableReflectionContextIdentifierAttribute));
      }
   }
}
