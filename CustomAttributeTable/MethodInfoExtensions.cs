using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CustomAttributeTableTests
{
   public static class MemberInfoExtensions
   {
      public static bool IsOverride(this MethodInfo method)
      {
         return method.GetBaseDefinition().DeclaringType != method.DeclaringType;
      }

      public static bool IsOverride(this PropertyInfo property)
      {
         return property.GetGetMethod(true)?.IsOverride() == true || property.GetSetMethod(true)?.IsOverride() == true;
      }

      public static bool IsOverride(this EventInfo eventInfo)
      {
         return eventInfo.GetAddMethod(true)?.IsOverride() == true ||
                eventInfo.GetRemoveMethod(true)?.IsOverride() == true;
      }

      /// <summary>
      /// When overridden in a derived class, returns the <see cref="propertyInfo"/> object for the 
      /// method on the direct or indirect base class in which the property represented 
      /// by this instance was first declared. 
      /// </summary>
      /// <returns>A <see cref="propertyInfo"/> object for the first implementation of this property.</returns>
      public static PropertyInfo GetBaseDefinition(this PropertyInfo propertyInfo)
      {
         var method = propertyInfo.GetAccessors(true)[0];
         if (method == null)
            return null;

         var baseMethod = method.GetBaseDefinition();

         if (baseMethod == method)
            return propertyInfo;

         var allProperties = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

         var arguments = propertyInfo.GetIndexParameters().Select(p => p.ParameterType).ToArray();

         return baseMethod.DeclaringType.GetProperty(propertyInfo.Name, allProperties,
             null, propertyInfo.PropertyType, arguments, null);
      }
   }

   public class MyClass
   {
      public string Name { get; set; }
      public MyClass()
      {
      }
   }
}


