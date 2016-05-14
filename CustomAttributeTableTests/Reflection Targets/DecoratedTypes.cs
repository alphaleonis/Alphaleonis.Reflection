using System;

namespace CustomAttributeTableTests
{
   class DecoratedTypes
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
         int ImplementedProperty { get; }

         [InheritedMulti(nameof(IBase1))]
         [InheritedSingle(nameof(IBase1))]
         [NonInheritedMulti(nameof(IBase1))]
         [NonInheritedSingle(nameof(IBase1))]
         int HiddenProperty { set; }

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

         [InheritedMulti(nameof(IBase1) + "int")]
         [InheritedSingle(nameof(IBase1) + "int")]
         [NonInheritedMulti(nameof(IBase1) + "int")]
         [NonInheritedSingle(nameof(IBase1) + "int")]
         void OverloadedMethod(int a);

         [InheritedMulti(nameof(IBase1) + "long")]
         [InheritedSingle(nameof(IBase1) + "long")]
         [NonInheritedMulti(nameof(IBase1) + "long")]
         [NonInheritedSingle(nameof(IBase1) + "long")]
         void OverloadedMethod(long a);

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
         new int HiddenProperty { get; set; }

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
         [InheritedSingle(nameof(Base) + "_S")]
         [InheritedMulti(nameof(Base) + "_S")]
         [NonInheritedSingle(nameof(Base) + "_S")]
         [NonInheritedMulti(nameof(Base) + "_S")]
         public static event EventHandler StaticBaseEvent;

         [InheritedSingle(nameof(Base) + "_V")]
         [InheritedMulti(nameof(Base) + "_V")]
         [NonInheritedSingle(nameof(Base) + "_V")]
         [NonInheritedMulti(nameof(Base) + "_V")]
         public event EventHandler NonVirtualBaseEvent;

         [InheritedSingle(nameof(Base) + "_H")]
         [InheritedMulti(nameof(Base) + "_H")]
         [NonInheritedSingle(nameof(Base) + "_H")]
         [NonInheritedMulti(nameof(Base) + "_H")]
         public event EventHandler HiddenBaseEvent;

         [InheritedSingle(nameof(Base) + "_O")]
         [InheritedMulti(nameof(Base) + "_O")]
         [NonInheritedSingle(nameof(Base) + "_O")]
         [NonInheritedMulti(nameof(Base) + "_O")]
         public virtual event EventHandler OverriddenEvent;

         [InheritedSingle(nameof(Base))]
         [InheritedMulti(nameof(Base))]
         [NonInheritedSingle(nameof(Base))]
         [NonInheritedMulti(nameof(Base))]
         public static int s_hiddenBaseField;

         [InheritedSingle(nameof(Base))]
         [InheritedMulti(nameof(Base))]
         [NonInheritedSingle(nameof(Base))]
         [NonInheritedMulti(nameof(Base))]
         public static int s_baseField;

         [InheritedSingle(nameof(Base))]
         [InheritedMulti(nameof(Base))]
         [NonInheritedSingle(nameof(Base))]
         [NonInheritedMulti(nameof(Base))]
         public int m_baseField;

         [InheritedSingle(nameof(Base))]
         [InheritedMulti(nameof(Base))]
         [NonInheritedSingle(nameof(Base))]
         [NonInheritedMulti(nameof(Base))]
         public int m_hiddenBaseField;

         [InheritedSingle(nameof(Base))]
         [InheritedMulti(nameof(Base))]
         [NonInheritedSingle(nameof(Base))]
         [NonInheritedMulti(nameof(Base))]
         public int HiddenProperty { get; set; }

         [InheritedSingle(nameof(Base))]
         [InheritedMulti(nameof(Base))]
         [NonInheritedSingle(nameof(Base))]
         [NonInheritedMulti(nameof(Base))]
         public int ImplementedProperty { get; private set; }

         [InheritedSingle(nameof(Base))]
         [InheritedMulti(nameof(Base))]
         [NonInheritedSingle(nameof(Base))]
         [NonInheritedMulti(nameof(Base))]
         public virtual object OverriddenProperty { get; set; }

         [InheritedSingle(nameof(Base))]
         [InheritedMulti(nameof(Base))]
         [NonInheritedSingle(nameof(Base))]
         [NonInheritedMulti(nameof(Base))]
         public virtual object OverriddenProperty2 { get; set; }

         [InheritedMulti(nameof(Base))]
         [InheritedSingle(nameof(Base))]
         [NonInheritedMulti(nameof(Base))]
         [NonInheritedSingle(nameof(Base))]
         public abstract void HiddenMethod1(int a);

         [InheritedMulti(nameof(Base))]
         [InheritedSingle(nameof(Base))]
         [NonInheritedMulti(nameof(Base))]
         [NonInheritedSingle(nameof(Base))]
         public abstract void ImplementedMethod1(int a, int b);

         [InheritedMulti(nameof(Base))]
         [InheritedSingle(nameof(Base))]
         [NonInheritedMulti(nameof(Base))]
         [NonInheritedSingle(nameof(Base))]
         public abstract void OverriddenMethod(int a, int b);

         [InheritedMulti(nameof(Base) + "int")]
         [InheritedSingle(nameof(Base) + "int")]
         [NonInheritedMulti(nameof(Base) + "int")]
         [NonInheritedSingle(nameof(Base) + "int")]
         public abstract void OverloadedMethod(int a);

         [InheritedMulti(nameof(Base) + "long")]
         [InheritedSingle(nameof(Base) + "long")]
         [NonInheritedMulti(nameof(Base) + "long")]
         [NonInheritedSingle(nameof(Base) + "long")]
         public abstract void OverloadedMethod(long a);
      }

      [InheritedSingle(nameof(Derived))]
      [InheritedMulti(nameof(Derived))]
      [NonInheritedSingle(nameof(Derived))]
      [NonInheritedMulti(nameof(Derived))]
      public abstract class Derived : Base
      {
         [InheritedSingle(nameof(Derived))]
         [InheritedMulti(nameof(Derived))]
         [NonInheritedSingle(nameof(Derived))]
         [NonInheritedMulti(nameof(Derived))]
         public new event EventHandler HiddenBaseEvent;

         [InheritedSingle(nameof(Derived))]
         [InheritedMulti(nameof(Derived))]
         [NonInheritedSingle(nameof(Derived))]
         [NonInheritedMulti(nameof(Derived))]
         public static new int s_hiddenBaseField;

         [InheritedSingle(nameof(Derived))]
         [InheritedMulti(nameof(Derived))]
         [NonInheritedSingle(nameof(Derived))]
         [NonInheritedMulti(nameof(Derived))]
         public new int m_hiddenBaseField;

         [InheritedSingle(nameof(Derived))]
         [InheritedMulti(nameof(Derived))]
         [NonInheritedSingle(nameof(Derived))]
         [NonInheritedMulti(nameof(Derived))]
         public int m_derivedField;

         [InheritedSingle(nameof(Derived))]
         [InheritedMulti(nameof(Derived))]
         [NonInheritedSingle(nameof(Derived))]
         [NonInheritedMulti(nameof(Derived))]
         public new long HiddenProperty { get; set; }

         [InheritedSingle(nameof(Derived))]
         [InheritedMulti(nameof(Derived))]
         [NonInheritedSingle(nameof(Derived))]
         [NonInheritedMulti(nameof(Derived))]
         public override object OverriddenProperty2 { get; set; }
      }

      [InheritedSingle(nameof(SubDerived))]
      [InheritedMulti(nameof(SubDerived))]
      [NonInheritedSingle(nameof(SubDerived))]
      [NonInheritedMulti(nameof(SubDerived))]
      public abstract class SubDerived : Derived
      {
         [InheritedSingle(nameof(SubDerived))]
         [InheritedMulti(nameof(SubDerived))]
         [NonInheritedSingle(nameof(SubDerived))]
         [NonInheritedMulti(nameof(SubDerived))]
         public override event EventHandler OverriddenEvent;

         [InheritedSingle(nameof(SubDerived))]
         [InheritedMulti(nameof(SubDerived))]
         [NonInheritedSingle(nameof(SubDerived))]
         [NonInheritedMulti(nameof(SubDerived))]
         public override object OverriddenProperty { get; set; }

         [InheritedSingle(nameof(SubDerived))]
         [InheritedMulti(nameof(SubDerived))]
         [NonInheritedSingle(nameof(SubDerived))]
         [NonInheritedMulti(nameof(SubDerived))]
         public override object OverriddenProperty2 { get; set; }

         [InheritedMulti(nameof(SubDerived))]
         [InheritedSingle(nameof(SubDerived))]
         [NonInheritedMulti(nameof(SubDerived))]
         [NonInheritedSingle(nameof(SubDerived))]
         public override void OverriddenMethod(int a, int b) { }

         [InheritedMulti(nameof(SubDerived) + "int")]
         [InheritedSingle(nameof(SubDerived) + "int")]
         [NonInheritedMulti(nameof(SubDerived) + "int")]
         [NonInheritedSingle(nameof(SubDerived) + "int")]
         public override void OverloadedMethod(int a) { }

         [InheritedMulti(nameof(SubDerived) + "long")]
         [InheritedSingle(nameof(SubDerived) + "long")]
         [NonInheritedMulti(nameof(SubDerived) + "long")]
         [NonInheritedSingle(nameof(SubDerived) + "long")]
         public override void OverloadedMethod(long a) { }
      }      
   }
}
