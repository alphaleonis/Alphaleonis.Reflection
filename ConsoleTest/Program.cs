using Alphaleonis.Reflection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tests.Alphaleonis.Reflection;

namespace ConsoleTest
{
   class MyActualBase
   {
      [return: InheritedMulti("Base")]
      [return: NonInheritedMulti("Base")]
      public virtual int MyMethod<T>(T a)
      {
         return 0;
      }
   }
   class MyBase
   {
      public virtual int MyMethod<T>(T a)
      {
         return 0;
      }
   }



   class MyChild : MyBase
   {
   }

   class MySubChild : MyChild
   {
      public override int MyMethod<T>(T a)
      {
         return 1;
      }
   }

   class MyActualChild : MyActualBase
   {
   }

   class MyActualSubChild : MyActualChild
   {
      [return: InheritedMulti("SubChild")]
      [return: NonInheritedMulti("SubChild")]
      public override int MyMethod<T>(T a)
      {
         return 1;
      }
   }

   [Category("My Category")]
   class MyClass
   {
      public static void MyMethod(int a, int b)
      {
      }

      public int MyMethod(int a, [Browsable(false)] int b, int c)
      {
         return 5;
      }
   }

   class Program
   {
      static void Main(string[] args)
      {
         CustomAttributeTableBuilder builder = new CustomAttributeTableBuilder();
         //builder.AddParameterAttributes(() => MyClass.MyMethod(1, Decorate.Parameter<int>(new Attribute[] { new BrowsableAttribute(false), new DisplayNameAttribute("Hello") })));
         //builder.AddParameterAttributes<MyClass>(c => c.MyMethod(1, 2, 
         //   Decorate.Parameter<int>(new Attribute[] { new BrowsableAttribute(false), new DisplayNameAttribute("Hello") })));

         //builder.AddReturnParameterAttributes<MyBase>(b => b.MyMethod<int>(0), new InheritedMultiAttribute("Base"), new NonInheritedMultiAttribute("Base"));
         //builder.AddReturnParameterAttributes<MyChild>(b => b.MyMethod<int>(0), new InheritedMultiAttribute("SubChild"), new NonInheritedMultiAttribute("SubChild"));
         var table = builder.CreateTable();
         
         AttributeTableReflectionContext ctx = new AttributeTableReflectionContext(table, AttributeTableReflectionContextOptions.Default);

         var systemType = typeof(MyActualChild);
         var type = ctx.MapType(typeof(MyChild));

         var ev = type.Assembly.Evidence;
         return;

         var returnParam = type.GetMethod(nameof(MyBase.MyMethod), BindingFlags.Public | BindingFlags.Instance).ReturnParameter;
         var attrs = returnParam.GetCustomAttributes(true);

         Console.WriteLine("Reflection Context");
         foreach (var attr in returnParam.GetCustomAttributes())
            Console.WriteLine(attr);

         Console.WriteLine();
         Console.WriteLine("Default");
         MethodInfo methodInfo = systemType.GetMethod(nameof(MyBase.MyMethod), BindingFlags.Public | BindingFlags.Instance);
         foreach (var attr in methodInfo.ReturnParameter.GetCustomAttributes(true))
            Console.WriteLine(attr);

         //var type = ctx.MapType(typeof(MyClass));
         //foreach (var method in type.GetMethods())
         //{
         //   Console.WriteLine(method.Name);
         //   foreach (var parameter in method.GetParameters())
         //   {
         //      Console.WriteLine("  Parameter: {0}", parameter.Name);
         //      foreach (var attr in parameter.GetCustomAttributes())
         //      {
         //         Console.WriteLine($"    Attribute {attr.GetType().Name}");
         //      }
         //   }
         //}
      }






   }
}
