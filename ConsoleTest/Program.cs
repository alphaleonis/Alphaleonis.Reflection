using CustomAttributeTable;
using CustomAttributeTableTests;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest
{

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
         builder.AddParameterAttributes(() => MyClass.MyMethod(1, Decorate.Parameter<int>(new Attribute[] { new BrowsableAttribute(false), new DisplayNameAttribute("Hello") })));
         builder.AddParameterAttributes<MyClass>(c => c.MyMethod(1, 2, 
            Decorate.Parameter<int>(new Attribute[] { new BrowsableAttribute(false), new DisplayNameAttribute("Hello") })));

         var table = builder.CreateTable();
         AttributeTableReflectionContext ctx = new AttributeTableReflectionContext(table, AttributeTableReflectionContextOptions.Default);
         var type = ctx.MapType(typeof(MyClass));
         foreach (var method in type.GetMethods())
         {
            Console.WriteLine(method.Name);
            foreach (var parameter in method.GetParameters())
            {
               Console.WriteLine("  Parameter: {0}", parameter.Name);
               foreach (var attr in parameter.GetCustomAttributes())
               {
                  Console.WriteLine($"    Attribute {attr.GetType().Name}");
               }
            }
         }
      }
      

      


      
   }
}
