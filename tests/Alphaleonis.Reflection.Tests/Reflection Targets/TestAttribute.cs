using System;
using System.Linq;

namespace Tests.Alphaleonis.Reflection
{
   public abstract class TestAttribute : Attribute
   {
      public TestAttribute(string value)
      {
         Value = value;
      }
      public TestAttribute()
      {
      }

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
         return $"{Name}-{Value}";
      }

      public abstract string Name { get; }
   }


   [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = true)]
   public sealed class InheritedMultiAttribute : TestAttribute
   {
      public InheritedMultiAttribute()
      {
      }

      public InheritedMultiAttribute(string value)
         : base(value)
      {
      }
      
      public override string Name => "IM";
   }

   [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
   public sealed class InheritedSingleAttribute : TestAttribute
   {
      public InheritedSingleAttribute()
      {
      }

      public InheritedSingleAttribute(string value)
         : base(value)
      {
      }

      public override string Name => "IS";
   }

   [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
   public sealed class NonInheritedSingleAttribute : TestAttribute
   {
      public NonInheritedSingleAttribute()
      {
      }

      public NonInheritedSingleAttribute(string value) 
         : base(value)
      {
      }

      public override string Name => "NS";
   }

   [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
   public sealed class NonInheritedMultiAttribute : TestAttribute
   {
      public NonInheritedMultiAttribute()
      {
      }

      public NonInheritedMultiAttribute(string value) 
         : base(value)
      {
      }

      public override string Name => "NM";
   }
}
