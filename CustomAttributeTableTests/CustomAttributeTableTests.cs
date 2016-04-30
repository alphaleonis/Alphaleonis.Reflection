using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CustomAttributeTable;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;

namespace CustomAttributeTableTests
{
   

   [InheritedSingle(Value = "BI1")]
   [InheritedMulti(Value = "BI1-A")]
   [InheritedMulti(Value = "BI1-B")]
   [NonInheritedSingle(Value = "BI1")]
   [NonInheritedMulti(Value = "BI1-A")]
   [NonInheritedMulti(Value = "BI1-B")]
   public interface IDecoratedBaseInterface1
   {
      [InheritedSingle(Value = "BI1-Add")]
      void Add(int a, int b);
   }

   [InheritedSingle(Value = "A")]
   [InheritedMulti(Value = "MA")]
   [InheritedMulti(Value = "MB")]
   public interface IDecoratedSubInterface1 : IDecoratedBaseInterface1
   {
      [InheritedSingle(Value = "SI1-Add")]
      [InheritedMulti(Value = "SI1-Add")]
      new void Add(int a, int b);
   }


   [InheritedSingle(Value = "BC1")]
   [InheritedMulti(Value = "BC1-A")]
   [InheritedMulti(Value = "BC1-B")]
   [NonInheritedSingle(Value = "BC1")]
   [NonInheritedMulti(Value = "BC1-A")]
   [NonInheritedMulti(Value = "BC1-B")]
   public abstract class DecoratedBaseClass1 : IDecoratedSubInterface1
   {
      [InheritedSingle(Value = "BC1-Add")]
      [InheritedMulti(Value = "BC1-Add")]
      public abstract void Add(int a, int b);

      [InheritedSingle(Value = "BC1-HideMe")]
      [InheritedMulti(Value = "BC1-HideMe")]
      public void HideMe() { }
   }

   [InheritedSingle(Value = "SC1")]
   [InheritedMulti(Value = "SC1-A")]
   [InheritedMulti(Value = "SC1-B")]
   [NonInheritedSingle(Value = "SC1")]
   [NonInheritedMulti(Value = "SC1-A")]
   [NonInheritedMulti(Value = "SC1-B")]
   public class DecoratedSubClass1 : DecoratedBaseClass1
   {
      [InheritedSingle(Value = "SC1-Add")]
      [InheritedMulti(Value = "SC1-Add")]
      public override void Add(int a, int b) { }

      [InheritedSingle(Value = "SC1-HideMe")]
      [InheritedMulti(Value = "SC1-HideMe")]
      public new void HideMe() { }
   }

   [InheritedSingle(Value = "SC2")]
   [InheritedMulti(Value = "SC2-A")]
   [InheritedMulti(Value = "SC2-B")]
   [NonInheritedSingle(Value = "SC2")]
   [NonInheritedMulti(Value = "SC2-A")]
   [NonInheritedMulti(Value = "SC2-B")]
   public abstract class DecoratedSubClass2 : DecoratedSubClass1
   {
      [InheritedSingle(Value = "SC2-Add")]
      [InheritedMulti(Value = "SC2-Add")]
      public override void Add(int a, int b) { }
   }


   public interface INonDecoratedBaseInterface1
   {
      void Add(int a, int b);
   }

   public interface INonDecoratedSubInterface1 : INonDecoratedBaseInterface1
   {
      new void Add(int a, int b);
   }

   
   public abstract class NonDecoratedBaseClass1 : IDecoratedSubInterface1
   {
      public abstract void Add(int a, int b);

      public void HideMe() { }
   }
   
   public abstract class NonDecoratedSubClass1 : NonDecoratedBaseClass1
   {
      public override void Add(int a, int b) { }

      public new void HideMe() { }
   }

   public abstract class NonDecoratedSubClass2 : NonDecoratedSubClass1
   {
      public override void Add(int a, int b) { }
   }

   [TestClass]
   public class CustomAttributeTableTests
   {
      private void CopyTypeAttributes<SourceType, TargetType>(CustomAttributeTableBuilder builder)
      {
         var sourceType = typeof(SourceType);
         var targetType = typeof(TargetType);
         builder.AddMemberAttributes(targetType, sourceType.GetCustomAttributes(false).OfType<Attribute>());

         foreach (var sourceMethod in sourceType.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).Where(m => !m.IsGenericMethod && m.DeclaringType.Equals(sourceType)))
         {
            var targetMethod = targetType.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)
               .Single(m => m.Name.Equals(sourceMethod.Name) &&
                       m.DeclaringType.Equals(targetType) &&
                                 m.GetParameters().Select(p => p.ParameterType).SequenceEqual(
                                 sourceMethod.GetParameters().Select(p => p.ParameterType)));

            builder.AddMemberAttributes(targetMethod, sourceMethod.GetCustomAttributes(false).OfType<Attribute>());
         }
      }

      class A
      {
         public virtual bool Name
         {
            get { return false; }
         }
      }

      class B : A
      {
         new public bool Name
         {
             get
            {
               return base.Name;
            }

            
         }
      }

      [TestMethod]
      public void IsOverride_PropertyInfo()
      {
         
      }
      [TestMethod]
      public void TestMethod1()
      {
         var iattrs = typeof(DecoratedSubClass2).GetMethod("HideMe").GetCustomAttributes(true);

         CustomAttributeTableBuilder builder = new CustomAttributeTableBuilder();
         // Copy all type attributes
         CopyTypeAttributes<IDecoratedBaseInterface1, INonDecoratedBaseInterface1>(builder);
         CopyTypeAttributes<IDecoratedSubInterface1, INonDecoratedSubInterface1>(builder);
         CopyTypeAttributes<DecoratedBaseClass1, NonDecoratedBaseClass1>(builder);
         CopyTypeAttributes<DecoratedSubClass1, NonDecoratedSubClass1>(builder);
         CopyTypeAttributes<DecoratedSubClass2, NonDecoratedSubClass2>(builder);

         var table = builder.CreateTable();

         AttributeTableReflectionContext context = new AttributeTableReflectionContext(table);


         //Check<DecoratedBaseClass1, NonDecoratedBaseClass1>(context);
         Check<DecoratedSubClass1, NonDecoratedSubClass1>(context);
         //Check<DecoratedSubClass2, NonDecoratedSubClass2>(context);

         //SequenceAssert.AreEquivalent(typeof(IDecoratedBaseInterface1).GetCustomAttributes(true), table.GetCustomAttributes(typeof(INonDecoratedBaseInterface1)));
         //SequenceAssert.AreEquivalent(typeof(IDecoratedSubInterface1).GetCustomAttributes(true), table.GetCustomAttributes(typeof(INonDecoratedSubInterface1)));
         //SequenceAssert.AreEquivalent(typeof(DecoratedBaseClass1).GetCustomAttributes(true), table.GetCustomAttributes(typeof(NonDecoratedBaseClass1)));
         //SequenceAssert.AreEquivalent(typeof(DecoratedSubClass1).GetCustomAttributes(true), table.GetCustomAttributes(typeof(NonDecoratedSubClass1)));
         //SequenceAssert.AreEquivalent(typeof(DecoratedSubClass2).GetCustomAttributes(true), table.GetCustomAttributes(typeof(NonDecoratedSubClass2)));
     } 

      private void Check<SourceType, TargetType>(ReflectionContext context)
      {
         Type sourceType = typeof(SourceType);
         Type targetType = context.MapType(typeof(TargetType).GetTypeInfo());

         SequenceAssert.AreEquivalent(sourceType.GetCustomAttributes(true), targetType.GetCustomAttributes(true).Where(attr => !(attr is AttributeTableReflectionContextIdentifierAttribute)));
         //SequenceAssert.AreEquivalent(sourceType.GetCustomAttributes(false), targetType.GetCustomAttributes(false));

      }
   }

   
}
