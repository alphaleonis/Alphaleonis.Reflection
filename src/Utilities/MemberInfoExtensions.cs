using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Alphaleonis.Reflection
{
   public static class MemberInfoExtensions
   {
      #region IsOverride

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

      #endregion

      #region GetBaseDefinition

      /// <summary>
      /// When overridden in a derived class, returns the <see cref="propertyInfo"/> object for the 
      /// method on the direct or indirect base class in which the property represented 
      /// by this instance was first declared. 
      /// </summary>
      /// <returns>A <see cref="PropertyInfo"/> object for the first implementation of this property.</returns>
      public static PropertyInfo GetBaseDefinition(this PropertyInfo propertyInfo)
      {
         var method = propertyInfo.GetAccessors(true)[0];
         if (method == null)
            return null;

         var baseMethod = method.GetBaseDefinition();

         if (baseMethod == method)
            return propertyInfo;

         return baseMethod.GetPropertyFromDeclaringType(propertyInfo);
      }

      /// <summary>
      /// Returns the <see cref="EventInfo"/> object for the 
      /// method on the direct or indirect base class in which the event represented 
      /// by this instance was first declared. 
      /// </summary>
      /// <returns>A <see cref="propertyInfo"/> object for the first implementation of this event.</returns>
      public static EventInfo GetBaseDefinition(this EventInfo eventInfo)
      {
         var method = eventInfo.GetAddMethod(true) ?? eventInfo.GetRemoveMethod(true);
         if (method == null)
            return null;

         var baseMethod = method.GetBaseDefinition();

         if (baseMethod == method)
            return eventInfo;

         return baseMethod.DeclaringType.GetEvent(eventInfo.Name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
      }

      public static MemberInfo GetBaseDefinition(this MemberInfo memberInfo)
      {
         switch (memberInfo.MemberType)
         {
            case MemberTypes.Method:
               return ((MethodInfo)memberInfo).GetBaseDefinition();

            case MemberTypes.Constructor:
               return ((ConstructorInfo)memberInfo).GetBaseDefinition();

            case MemberTypes.Property:
               return GetBaseDefinition((PropertyInfo)memberInfo);
               
            case MemberTypes.Event:
               return GetBaseDefinition((EventInfo)memberInfo);

            case MemberTypes.TypeInfo:
            case MemberTypes.NestedType:
               return ((Type)memberInfo).BaseType ?? memberInfo;

            case MemberTypes.Field:
            case MemberTypes.Custom:
            case MemberTypes.All:
            default:
               return memberInfo;
         }
      }

      #endregion

      #region GetParentDefinition

      public static EventInfo GetParentDefinition(this EventInfo eventInfo)
      {
         var eventMethod = eventInfo.AddMethod ?? eventInfo.RemoveMethod;

         if (eventMethod != null)
         {
            eventMethod = eventMethod.GetParentDefinition();
            if (eventMethod != null)
            {
               return eventMethod.GetEventFromDeclaringType(eventInfo);
            }
         }

         return null;
      }

      public static PropertyInfo GetParentDefinition(this PropertyInfo property)
      {
         var propertyMethod = property.GetMethod ?? property.SetMethod;

         if (propertyMethod != null)
         {
            propertyMethod = propertyMethod.GetParentDefinition();
            if (propertyMethod != null)
            {
               return propertyMethod.GetPropertyFromDeclaringType(property);
            }
         }

         return null;
      }

      public static MethodInfo GetParentDefinition(this MethodInfo method)
      {
         var baseDefinition = method.GetBaseDefinition();
         var type = method.DeclaringType.BaseType;       
            
         while (type != null)
         {
            var parent = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly)
               .FirstOrDefault(m => m.GetBaseDefinition().Equals(baseDefinition));

            if (parent != null)
               return parent;

            type = type.BaseType;
         }

         return null;
      }

      public static MemberInfo GetParentDefinition(this MemberInfo member)
      {
         if (member == null)
            return null;

         switch (member.MemberType)
         {
            case MemberTypes.Event:
               return GetParentDefinition((EventInfo)member);
               
            case MemberTypes.Method:
               return GetParentDefinition((MethodInfo)member);

            case MemberTypes.Property:
               return GetParentDefinition((PropertyInfo)member);

            case MemberTypes.TypeInfo:
            case MemberTypes.NestedType:
               return ((Type)member).BaseType;

            default:
               return null;
         }
      }

      #endregion

      #region Private Utilities

      private static PropertyInfo GetPropertyFromDeclaringType(this MemberInfo member, PropertyInfo propertyInfo)
      {
         var allProperties = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

         var arguments = propertyInfo.GetIndexParameters().Select(p => p.ParameterType).ToArray();

         return member.DeclaringType.GetProperty(propertyInfo.Name, allProperties, null, propertyInfo.PropertyType, arguments, null);
      }

      private static EventInfo GetEventFromDeclaringType(this MemberInfo member, EventInfo propertyInfo)
      {
         var bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

         return member.DeclaringType.GetEvent(propertyInfo.Name, bindingFlags);
      }

      #endregion
   }
}


