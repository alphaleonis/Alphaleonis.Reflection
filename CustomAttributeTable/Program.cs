using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CustomAttributeTable
{
   class TestClass<T>
   {
      public void Test(T a)
      {

      }

      public void Test<U>(U a, int val)
      {

      }

      public void Test<U>(U a, T val)
      {

      }

      public void MyMehtod(string inp)
      {
      }
   }

   

   class Program
   {
      [NonSerialized]
      int m_field;

      static void Main(string[] args)
      {

         HashSet<CustomAttributeTableBuilder.MethodKey> keys = new HashSet<CustomAttributeTableBuilder.MethodKey>();

         for (int i = 0; i < 2; i++)
         {
            foreach (var meth in typeof(TestClass<int>).GetMethods().Where(m => m.DeclaringType.Equals(typeof(TestClass<int>))))
            {
               Console.WriteLine(meth.Name + ": " + keys.Add(new CustomAttributeTableBuilder.MethodKey(meth)));
            }
         }

         Console.WriteLine(keys.Count);
         //FieldInfo field = typeof(Program).GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance).First();
         //var cattr = field.FieldHandle;
         //FieldInfoDelegator del = new FieldInfoDelegator(field);
         //var cattr2 = del.FieldHandle;

         //Console.WriteLine(cattr.Value);
         //Console.WriteLine(cattr2.Value);


      }

      

      
   }
   



}
