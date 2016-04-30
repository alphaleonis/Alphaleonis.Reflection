using System;

namespace CustomAttributeTableTests
{
   class NonAttributedTypes
   {
      public interface IBase1
      {
         void ImplementedMethod1(int a, int b);
         void HiddenMethod1(int a);
      }

      public interface IBase2
      {
         void ImplementedMethod1(int a, int b);
      }

      public interface IComposite : IBase1, IBase2
      {
         new void HiddenMethod1(int a);
      }

      public abstract class Base : IComposite
      {
         public virtual object OverriddenProperty { get; set; }

         public abstract void HiddenMethod1(int a);
         public abstract void ImplementedMethod1(int a, int b);
      }

      public abstract class Derived : Base
      {
         public override object OverriddenProperty { get; set; }
      }

      public abstract class SubDerived : Derived
      {
      }
   }
}
