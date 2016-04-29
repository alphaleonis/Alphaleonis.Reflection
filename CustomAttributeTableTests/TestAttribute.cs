using System;
using System.Linq;

namespace CustomAttributeTableTests
{

   public abstract class TestAttribute : Attribute
   {
      public string Value { get; set; }

      public override bool Equals(object obj)
      {
         TestAttribute other = obj as TestAttribute;
         return other != null && Value == other.Value && GetType().Equals(other.GetType());
      }

      public override int GetHashCode()
      {
         return 0;
      }

      public override string ToString()
      {
         return $"{Name}-{Value}\n";
      }

      public abstract string Name { get; }
   }


   [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
   public sealed class InheritedMultiAttribute : TestAttribute
   {
      public override string Name => "IMA";
   }

   [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
   public sealed class InheritedSingleAttribute : TestAttribute
   {
      public override string Name => "ISA";
   }

   [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
   public sealed class NonInheritedSingleAttribute : TestAttribute
   {
      public override string Name => "NSA";
   }

   [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
   public sealed class NonInheritedMultiAttribute : TestAttribute
   {
      public override string Name => "NMA";
   }
}
