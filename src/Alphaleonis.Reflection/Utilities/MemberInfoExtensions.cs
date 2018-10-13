// Copyright (c) Peter Palotas
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Alphaleonis.Reflection
{
   /// <summary>Defines useful extension methods, used for reflection.</summary>
   public static class MemberInfoExtensions
   {
      #region IsOverride

      /// <summary>Determines if this method is an override of a method declared in a base class.</summary>
      /// <param name="method">The method to check.</param>
      /// <returns><see langword="true"/> if <paramref name="method"/> refers to a method overriding a method from a base class, <see langword="false"/> otherwise.</returns>
      public static bool IsOverride(this MethodInfo method)
      {
         return method.GetBaseDefinition().DeclaringType != method.DeclaringType;
      }

      /// <summary>Determines if the specified property is an override of a property declared in a base class.</summary>
      /// <param name="property">The property to check.</param>
      /// <returns><see langword="true"/> if <paramref name="property"/> refers to a property overriding a property from a base class, <see langword="false"/> otherwise.</returns>
      public static bool IsOverride(this PropertyInfo property)
      {
         return property.GetGetMethod(true)?.IsOverride() == true || property.GetSetMethod(true)?.IsOverride() == true;
      }

      /// <summary>Determines if the specified event is an override of an event declared in a base class.</summary>
      /// <param name="eventInfo">The event to check.</param>
      /// <returns><see langword="true"/> if <paramref name="eventInfo"/> refers to an event overriding an event from a base class, <see langword="false"/> otherwise.</returns>
      public static bool IsOverride(this EventInfo eventInfo)
      {
         return eventInfo.GetAddMethod(true)?.IsOverride() == true ||
                eventInfo.GetRemoveMethod(true)?.IsOverride() == true;
      }

      #endregion

      #region GetBaseDefinition

      /// <summary>
      /// When overridden in a derived class, returns the <see cref="PropertyInfo"/> object for the 
      /// method on the direct or indirect base class in which the property represented 
      /// by this instance was first declared. 
      /// </summary>
      /// <returns>A <see cref="PropertyInfo"/> object for the top-most declaration of this property in the inheritance chain.</returns>
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
      /// event on the direct or indirect base class in which the event represented 
      /// by this instance was first declared. 
      /// </summary>
      /// <returns>An <see cref="EventInfo"/> object for the top-most declaration of this event in the inheritance chain.</returns>
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

      /// <summary>
      /// Returns the <see cref="MemberInfo"/> object for the 
      /// member on the direct or indirect base class in which the member represented 
      /// by this instance was first declared. 
      /// </summary>
      /// <returns>A <see cref="MemberInfo"/> object for the top-most declaration of this member in the inheritance chain.</returns>
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

      /// <summary>Gets the most derived override of this event up the inheritance chain.</summary>
      /// <param name="eventInfo">The event to check.</param>
      /// <remarks>Returns override of the specified event in the most derived class that is a base class of the declaring type
      ///          of the specified <paramref name="eventInfo"/> if it exists. If no override exists prior to this one in 
      ///          the inheritance chain, this method will return the same as <see cref="GetBaseDefinition(EventInfo)"/>.
      ///          </remarks>
      /// <returns>The most derived definition of the specified <paramref name="eventInfo"/> in a base class.</returns>
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

      /// <summary>Gets the most derived override of this property up the inheritance chain.</summary>
      /// <param name="property">The property to check.</param>
      /// <remarks>Returns override of the specified property in the most derived class that is a base class of the declaring type
      ///          of the specified <paramref name="property"/> if it exists. If no override exists prior to this one in 
      ///          the inheritance chain, this method will return the same as <see cref="GetBaseDefinition(PropertyInfo)"/>.
      ///          </remarks>
      /// <returns>The most derived definition of the specified <paramref name="property"/> in a base class.</returns>
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

      // TODO PP (2018-06-11): This documentation does not seem correct. We return null if there is no parent definition?

      /// <summary>Gets the most derived override of this method up the inheritance chain.</summary>
      /// <param name="method">The method to check.</param>
      /// <remarks>Returns override of the specified method in the most derived class that is a base class of the declaring type
      ///          of the specified <paramref name="method"/> if it exists. If no override exists prior to this one in
      ///          the inheritance chain, this method will return the same as <see cref="GetBaseDefinition(MemberInfo)" />.
      ///          </remarks>
      /// <returns>The most derived definition of the specified <paramref name="method"/> in a base class.</returns>
      public static MethodInfo GetParentDefinition(this MethodInfo method)
      {         
         var baseDefinition = method.GetBaseDefinition();

         if (baseDefinition == method)
            return null;

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

      /// <summary>Gets the most derived override of this member up the inheritance chain.</summary>
      /// <param name="member">The member to check.</param>
      /// <remarks>Returns override of the specified member in the most derived class that is a base class of the declaring type
      ///          of the specified <paramref name="member"/> if it exists. If no override exists prior to this one in
      ///          the inheritance chain, this member will return the same as <see cref="GetBaseDefinition(MemberInfo)"/>.
      ///          </remarks>
      /// <returns>The most derived definition of the specified <paramref name="member"/> in a base class.</returns>
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


