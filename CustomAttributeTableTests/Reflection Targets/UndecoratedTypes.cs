using System;

namespace CustomAttributeTableTests
{
   class UndecoratedTypes
   {
      public interface IBase1
      {
         int ImplementedProperty { get; }
         int HiddenProperty { set; }

         void ImplementedMethod1(int a, int b);
         void HiddenMethod1(int a);

         void OverloadedMethod(int a);
         void OverloadedMethod(long a);
      }

      public interface IBase2
      {
         void ImplementedMethod1(int a, int b);
      }

      public interface IComposite : IBase1, IBase2
      {
         new int HiddenProperty { get; set; }
         new void HiddenMethod1(int a);
      }

      public abstract class Base : IComposite
      {
         public static event EventHandler StaticBaseEvent;
         public event EventHandler NonVirtualBaseEvent;
         public event EventHandler HiddenBaseEvent;
         public virtual event EventHandler OverriddenEvent;

         public static int s_hiddenBaseField;
         public static int s_baseField;

         public int m_baseField;
         public int m_hiddenBaseField;

         public int HiddenProperty { get; set; }
         public int ImplementedProperty { get; private set; }
         public virtual object OverriddenProperty { get; set; }
         public virtual object OverriddenProperty2 { get; set; }
         int IBase1.HiddenProperty { set { } }

         public abstract void HiddenMethod1(int a);
         public abstract void ImplementedMethod1(int a, int b);

         public abstract void OverriddenMethod(int a, int b);

         public abstract void OverloadedMethod(long a);
         public abstract void OverloadedMethod(int a);
      }

      public abstract class Derived : Base
      {
         public new event EventHandler HiddenBaseEvent;

         public new int m_hiddenBaseField;
         public int m_derivedField;
         public static new int s_hiddenBaseField;

         public new long HiddenProperty { get; set; }
         public override object OverriddenProperty2 { get; set; }
      }

      public abstract class SubDerived : Derived
      {
         public override event EventHandler OverriddenEvent;

         public override object OverriddenProperty { get; set; }
         public override object OverriddenProperty2 { get; set; }

         public override void OverriddenMethod(int a, int b) { }

         public override void OverloadedMethod(long a) { }
         public override void OverloadedMethod(int a) { }

      }
   }
}
