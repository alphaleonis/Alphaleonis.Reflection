using System;

namespace CustomAttributeTableTests
{
   class AttributedTypes
   {
      [InheritedMulti(nameof(IBase1))]
      [InheritedSingle(nameof(IBase1))]
      [NonInheritedMulti(nameof(IBase1))]
      [NonInheritedSingle(nameof(IBase1))]
      public interface IBase1
      {
         [InheritedMulti(nameof(IBase1))]
         [InheritedSingle(nameof(IBase1))]
         [NonInheritedMulti(nameof(IBase1))]
         [NonInheritedSingle(nameof(IBase1))]
         void ImplementedMethod1(int a, int b);

         [InheritedMulti(nameof(IBase1))]
         [InheritedSingle(nameof(IBase1))]
         [NonInheritedMulti(nameof(IBase1))]
         [NonInheritedSingle(nameof(IBase1))]
         void HiddenMethod1(int a);
      }

      [InheritedMulti(nameof(IBase2))]
      [InheritedSingle(nameof(IBase2))]
      [NonInheritedMulti(nameof(IBase2))]
      [NonInheritedSingle(nameof(IBase2))]
      public interface IBase2
      {
         [InheritedMulti(nameof(IBase2))]
         [InheritedSingle(nameof(IBase2))]
         [NonInheritedMulti(nameof(IBase2))]
         [NonInheritedSingle(nameof(IBase2))]
         void ImplementedMethod1(int a, int b);         
      }

      [InheritedSingle(nameof(IComposite))]
      [InheritedMulti(nameof(IComposite))]
      [NonInheritedSingle(nameof(IComposite))]
      [NonInheritedMulti(nameof(IComposite))]
      public interface IComposite : IBase1, IBase2
      {
         [InheritedMulti(nameof(IComposite))]
         [InheritedSingle(nameof(IComposite))]
         [NonInheritedMulti(nameof(IComposite))]
         [NonInheritedSingle(nameof(IComposite))]
         new void HiddenMethod1(int a);
      }

      [InheritedSingle(nameof(Base))]
      [InheritedMulti(nameof(Base))]
      [NonInheritedSingle(nameof(Base))]
      [NonInheritedMulti(nameof(Base))]
      public abstract class Base : IComposite
      {
         [InheritedMulti(nameof(Base))]
         [InheritedSingle(nameof(Base))]
         [NonInheritedMulti(nameof(Base))]
         [NonInheritedSingle(nameof(Base))]
         public abstract void HiddenMethod1(int a);
         public abstract void ImplementedMethod1(int a, int b);
      }

      [InheritedSingle(nameof(Derived))]
      [InheritedMulti(nameof(Derived))]
      [NonInheritedSingle(nameof(Derived))]
      [NonInheritedMulti(nameof(Derived))]
      public abstract class Derived : Base
      {
      }

      [InheritedSingle(nameof(SubDerived))]
      [InheritedMulti(nameof(SubDerived))]
      [NonInheritedSingle(nameof(SubDerived))]
      [NonInheritedMulti(nameof(SubDerived))]
      public abstract class SubDerived : Derived
      {
      }
   }
}
