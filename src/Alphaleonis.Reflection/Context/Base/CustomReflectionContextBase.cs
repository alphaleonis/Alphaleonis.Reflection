// Copyright (c) Peter Palotas
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Alphaleonis.Reflection.Context
{
   /// <summary>Represents a customizable reflection context.</summary>
   public abstract partial class CustomReflectionContextBase : ReflectionContext
   {
      /// <summary>Constructor.</summary>
      protected CustomReflectionContextBase()
      {
         Id = Guid.NewGuid();
         ContextIdentifierAttribute = new CustomReflectionContextIdAttribute(Id);
      }

      /// <summary>Gets the unique identifier of this reflection context instance.</summary>
      /// <value>The identifier.</value>
      /// <remarks>A new unique ID is generated in the constructor of this class.</remarks>
      protected Guid Id { get; }

      private CustomReflectionContextIdAttribute ContextIdentifierAttribute { get; }

      /// <summary>
      /// Gets the representation, in this reflection context, of a collection types represented by an
      /// types from another reflection context.
      /// </summary>
      /// <remarks>
      /// If any of the types was already mapped in this reflection context, they will not be mapped
      /// again.
      /// </remarks>
      /// <param name="types">The types to map.</param>
      /// <returns>
      /// The representation of the <paramref name="types"/> in this reflection context.
      /// </returns>
      public IEnumerable<TypeInfo> MapTypes(IEnumerable<Type> types)
      {
         return types.Select(type => MapType(type));
      }

      /// <summary>
      /// Gets the representation, in this reflection context, of an assembly that is represented by an
      /// object from another reflection context.
      /// </summary>
      /// <remarks>
      /// If the assembly was already mapped in this reflection context, it will not be mapped
      /// again.
      /// </remarks>
      /// <param name="assembly">The assembly to map.</param>
      /// <returns>The representation of the assembly in this reflection context.</returns>
      public sealed override Assembly MapAssembly(Assembly assembly)
      {
         if (IsMapped(assembly))
            return assembly;

         return MapAssemblyCore(assembly);

      }

      /// <summary>
      /// Gets the representation of a type in this reflection context that is represented by an object
      /// from another reflection context.
      /// </summary>
      /// <remarks>
      /// If the type was already mapped in this reflection context, it will not be mapped
      /// again.
      /// </remarks>
      /// <param name="type">The type to map.</param>
      /// <returns>A TypeInfo representation of the type in this reflection context.</returns>
      public TypeInfo MapType(Type type)
      {
         if (type == null)
            return null;

         if (IsMapped(type))
            return type.GetTypeInfo();

         return MapTypeCore(type).GetTypeInfo();
      }

      /// <summary>
      /// Gets the representation of a type in this reflection context that is represented by an object
      /// from another reflection context.
      /// </summary>
      /// <remarks>
      /// If the type was already mapped in this reflection context, it will not be mapped
      /// again.
      /// </remarks>
      /// <param name="type">The type to map.</param>
      /// <returns>A TypeInfo representation of the type in this reflection context.</returns>
      public sealed override TypeInfo MapType(TypeInfo type)
      {
         return MapType((Type)type).GetTypeInfo();
      }

      #region Map Member

      #region Private MapMember methods

      /// <summary>Maps a <see cref="MemberInfo"/> in this reflection context.</summary>
      /// <remarks>
      /// <para>
      /// This method simply delegates the call to <see cref="MapMember(ConstructorInfo)"/>,
      /// <see cref="MapMember(EventInfo)"/>,
      /// <see cref="MapMember(FieldInfo)"/> etc depending on the member type.
      /// </para>
      /// <para>
      /// If the specified member was already mapped in this reflection context it will be returned as
      /// is.
      /// </para>
      /// </remarks>
      /// <exception cref="NotSupportedException">Thrown if the member type is not supported.</exception>
      /// <param name="member">The member to map.</param>
      /// <returns>
      /// The representation of the member in this reflection context, or <see langword="null"/> if
      /// <paramref name="member"/> was <see langword="null"/>.
      /// </returns>
      protected MemberInfo MapMember(MemberInfo member)
      {
         if (member == null)
            return null;

         switch (member.MemberType)
         {
            case MemberTypes.Constructor:
               return MapMember((ConstructorInfo)member);

            case MemberTypes.Event:
               return MapMember((EventInfo)member);

            case MemberTypes.Field:
               return MapMember((FieldInfo)member);

            case MemberTypes.Method:
               return MapMember((MethodInfo)member);

            case MemberTypes.Property:
               return MapMember((PropertyInfo)member);

            case MemberTypes.TypeInfo:
            case MemberTypes.NestedType:
               return MapType((Type)member);

            case MemberTypes.Custom:
            case MemberTypes.All:
            default:
               throw new NotSupportedException($"Cannot map a member of type {member.MemberType}");
         }
      }

      /// <summary>Maps an array of types in this reflection context.</summary>
      /// <remarks>
      /// Any of the types specified that were already mapped in this reflection context will not be
      /// mapped again.
      /// </remarks>
      /// <param name="types">The types to map.</param>
      /// <returns>
      /// An array containing the representation of the types in this reflection context, or <see langword="null"/>
      /// if <paramref name="types"/> was <see langword="null"/>.
      /// </returns>
      protected Type[] MapTypes(Type[] types)
      {
         if (types == null || types.Length == 0)
            return types;

         TypeInfo[] result = new TypeInfo[types.Length];

         for (int i = 0; i < types.Length; i++)
         {
            result[i] = MapType(types[i]);
         }

         return result;
      }

      /// <summary>Maps an array of fields in this reflection context.</summary>
      /// <remarks>
      /// Any of the types specified that were already mapped in this reflection context will not be
      /// mapped again.
      /// </remarks>
      /// <param name="members">The fields to map.</param>
      /// <returns>
      /// An array containing the representation of the fields in this reflection context, or
      /// <see langword="null"/>
      /// if <paramref name="members"/> was <see langword="null"/>.
      /// </returns>
      /// <seealso cref="MapMember(FieldInfo)"/>
      protected FieldInfo[] MapMembers(FieldInfo[] members)
      {
         if (members == null || members.Length == 0)
            return members;

         FieldInfo[] result = new FieldInfo[members.Length];

         for (int i = 0; i < members.Length; i++)
         {
            result[i] = MapMember(members[i]);
         }

         return result;
      }
      /// <summary>Maps an array of properties in this reflection context.</summary>
      /// <remarks>
      /// Any of the properties specified that were already mapped in this reflection context will not be
      /// mapped again.
      /// </remarks>
      /// <param name="members">The properties to map.</param>
      /// <returns>
      /// An array containing the representation of the properties in this reflection context, or
      /// <see langword="null"/>
      /// if <paramref name="members"/> was <see langword="null"/>.
      /// </returns>
      /// <seealso cref="MapMember(PropertyInfo)"/>
      protected PropertyInfo[] MapMembers(PropertyInfo[] members)
      {
         if (members == null || members.Length == 0)
            return members;

         PropertyInfo[] result = new PropertyInfo[members.Length];

         for (int i = 0; i < members.Length; i++)
         {
            result[i] = MapMember(members[i]);
         }

         return result;
      }

      /// <summary>Maps an array of events in this reflection context.</summary>
      /// <remarks>
      /// Any of the events specified that were already mapped in this reflection context will not be
      /// mapped again.
      /// </remarks>
      /// <param name="members">The events to map.</param>
      /// <returns>
      /// An array containing the representation of the events in this reflection context, or
      /// <see langword="null"/>
      /// if <paramref name="members"/> was <see langword="null"/>.
      /// </returns>
      /// <seealso cref="MapMember(EventInfo)"/>
      protected EventInfo[] MapMembers(EventInfo[] members)
      {
         if (members == null || members.Length == 0)
            return members;

         EventInfo[] result = new EventInfo[members.Length];

         for (int i = 0; i < members.Length; i++)
         {
            result[i] = MapMember(members[i]);
         }

         return result;
      }

      /// <summary>Maps an array of constructors in this reflection context.</summary>
      /// <remarks>
      /// Any of the constructors specified that were already mapped in this reflection context will not be
      /// mapped again.
      /// </remarks>
      /// <param name="members">The constructors to map.</param>
      /// <returns>
      /// An array containing the representation of the constructors in this reflection context, or
      /// <see langword="null"/>
      /// if <paramref name="members"/> was <see langword="null"/>.
      /// </returns>
      ///  <seealso cref="MapMember(ConstructorInfo)"/>
      protected ConstructorInfo[] MapMembers(ConstructorInfo[] members)
      {
         if (members == null || members.Length == 0)
            return members;

         ConstructorInfo[] result = new ConstructorInfo[members.Length];

         for (int i = 0; i < members.Length; i++)
         {
            result[i] = MapMember(members[i]);
         }

         return result;
      }

      /// <summary>Maps an array of methods in this reflection context.</summary>
      /// <remarks>
      /// Any of the methods specified that were already mapped in this reflection context will not be
      /// mapped again.
      /// </remarks>
      /// <param name="members">The methods to map.</param>
      /// <returns>
      /// An array containing the representation of the methods in this reflection context, or
      /// <see langword="null"/>
      /// if <paramref name="members"/> was <see langword="null"/>.
      /// </returns>
      /// <seealso cref="MapMember(MethodInfo)"/>
      protected MethodInfo[] MapMembers(MethodInfo[] members)
      {
         if (members == null || members.Length == 0)
            return members;

         MethodInfo[] result = new MethodInfo[members.Length];

         for (int i = 0; i < members.Length; i++)
         {
            result[i] = MapMember(members[i]);
         }

         return result;
      }

      /// <summary>Maps an array of members in this reflection context.</summary>
      /// <remarks>
      /// Any of the members specified that were already mapped in this reflection context will not be
      /// mapped again.
      /// </remarks>
      /// <param name="members">The members to map.</param>
      /// <returns>
      /// An array containing the representation of the members in this reflection context, or
      /// <see langword="null"/>
      /// if <paramref name="members"/> was <see langword="null"/>.
      /// </returns>
      /// <seealso cref="MapMember(MemberInfo)"/>
      protected MemberInfo[] MapMembers(MemberInfo[] members)
      {
         if (members == null || members.Length == 0)
            return members;

         MemberInfo[] result = new MemberInfo[members.Length];

         for (int i = 0; i < members.Length; i++)
         {
            result[i] = MapMember(members[i]);
         }

         return result;
      }

      /// <summary>Maps an array of parameters in this reflection context.</summary>
      /// <remarks>
      /// Any of the parameters specified that were already mapped in this reflection context will not be
      /// mapped again.
      /// </remarks>
      /// <param name="parameters">The parameters to map.</param>
      /// <returns>
      /// An array containing the representation of the parameters in this reflection context, or
      /// <see langword="null"/>
      /// if <paramref name="parameters"/> was <see langword="null"/>.
      /// </returns>
      /// <seealso cref="MapParameter(ParameterInfo)"/>
      protected ParameterInfo[] MapParameters(ParameterInfo[] parameters)
      {
         if (parameters == null || parameters.Length == 0)
            return parameters;

         ParameterInfo[] result = new ParameterInfo[parameters.Length];

         for (int i = 0; i < parameters.Length; i++)
         {
            result[i] = MapParameter(parameters[i]);
         }

         return result;
      }

      /// <summary>Maps a <see cref="FieldInfo"/> in this reflection context.</summary>
      /// <remarks>If the specified <paramref name="field"/> was already mapped in this reflection
      /// context it will not be mapped again.</remarks>
      /// <remarks>To override the actual mapping of a field, override
      /// <see cref="MapFieldCore(FieldInfo)"/> which is called by this method to perform the actual
      /// mapping for a field that is not already mapped.</remarks>
      /// <param name="field">The <see cref="FieldInfo"/> to map.</param>
      /// <returns>
      /// The representation of the specified <paramref name="field"/> in this reflection context, or
      /// <see langword="null"/> if the
      /// <paramref name="field"/> specified was <see langword="null"/>.
      /// </returns>
      protected FieldInfo MapMember(FieldInfo field)
      {
         if (field == null)
            return null;

         if (IsMapped(field))
            return field;

         return MapFieldCore(field);
      }

      /// <summary>Maps an <see cref="EventInfo"/> in this reflection context.</summary>
      /// <remarks>If the specified <paramref name="member"/> was already mapped in this reflection
      /// context it will not be mapped again.</remarks>
      /// <remarks>To override the actual mapping of a member, override
      /// <see cref="MapEventCore(EventInfo)"/> which is called by this method to perform the actual
      /// mapping for an event that is not already mapped.</remarks>
      /// <param name="member">The <see cref="EventInfo"/> to map.</param>
      /// <returns>
      /// The representation of the specified <paramref name="member"/> in this reflection context, or
      /// <see langword="null"/> if the
      /// <paramref name="member"/> specified was <see langword="null"/>.
      /// </returns>
      protected EventInfo MapMember(EventInfo member)
      {
         if (member == null)
            return null;

         if (IsMapped(member))
            return member;

         return MapEventCore(member);
      }

      /// <summary>Maps a <see cref="ConstructorInfo"/> in this reflection context.</summary>
      /// <remarks>If the specified <paramref name="constructor"/> was already mapped in this reflection
      /// context it will not be mapped again.</remarks>
      /// <remarks>To override the actual mapping of a constructor, override
      /// <see cref="MapConstructorCore(ConstructorInfo)"/> which is called by this method to perform the actual
      /// mapping for a constructor that is not already mapped.</remarks>
      /// <param name="constructor">The <see cref="ConstructorInfo"/> to map.</param>
      /// <returns>
      /// The representation of the specified <paramref name="constructor"/> in this reflection context, or
      /// <see langword="null"/> if the
      /// <paramref name="constructor"/> specified was <see langword="null"/>.
      /// </returns>
      protected ConstructorInfo MapMember(ConstructorInfo constructor)
      {
         if (constructor == null)
            return null;

         if (IsMapped(constructor))
            return constructor;

         return MapConstructorCore(constructor);
      }

      /// <summary>Maps a <see cref="ParameterInfo"/> in this reflection context.</summary>
      /// <remarks>If the specified <paramref name="parameter"/> was already mapped in this reflection
      /// context it will not be mapped again.</remarks>
      /// <remarks>To override the actual mapping of a parameter, override
      /// <see cref="MapParameterCore(ParameterInfo)"/> which is called by this method to perform the actual
      /// mapping for a parameter that is not already mapped.</remarks>
      /// <param name="parameter">The <see cref="ParameterInfo"/> to map.</param>
      /// <returns>
      /// The representation of the specified <paramref name="parameter"/> in this reflection context, or
      /// <see langword="null"/> if the
      /// <paramref name="parameter"/> specified was <see langword="null"/>.
      /// </returns>
      protected ParameterInfo MapParameter(ParameterInfo parameter)
      {
         if (parameter == null)
            return null;

         if (IsMapped(parameter))
            return parameter;

         return MapParameterCore(parameter);
      }


      /// <summary>Maps a <see cref="PropertyInfo"/> in this reflection context.</summary>
      /// <remarks>If the specified <paramref name="property"/> was already mapped in this reflection
      /// context it will not be mapped again.</remarks>
      /// <remarks>To override the actual mapping of a property, override
      /// <see cref="MapPropertyCore(PropertyInfo)"/> which is called by this method to perform the actual
      /// mapping for a property that is not already mapped.</remarks>
      /// <param name="property">The <see cref="PropertyInfo"/> to map.</param>
      /// <returns>
      /// The representation of the specified <paramref name="property"/> in this reflection context, or
      /// <see langword="null"/> if the
      /// <paramref name="property"/> specified was <see langword="null"/>.
      /// </returns>
      protected PropertyInfo MapMember(PropertyInfo property)
      {
         if (property == null)
            return null;

         if (IsMapped(property))
            return property;

         return MapPropertyCore(property);
      }

      /// <summary>Maps a <see cref="MethodInfo"/> in this reflection context.</summary>
      /// <remarks>If the specified <paramref name="method"/> was already mapped in this reflection
      /// context it will not be mapped again.</remarks>
      /// <remarks>To override the actual mapping of a method, override
      /// <see cref="MapMethodCore(MethodInfo)"/> which is called by this method to perform the actual
      /// mapping for a method that is not already mapped.</remarks>
      /// <param name="method">The <see cref="MethodInfo"/> to map.</param>
      /// <returns>
      /// The representation of the specified <paramref name="method"/> in this reflection context, or
      /// <see langword="null"/> if the
      /// <paramref name="method"/> specified was <see langword="null"/>.
      /// </returns>
      protected MethodInfo MapMember(MethodInfo method)
      {
         if (method == null)
            return null;

         if (IsMapped(method))
            return method;

         return MapMethodCore(method);
      }

      #endregion

      #endregion

      #region MapCore

      /// <summary>
      /// Performs projection of a parameter that has not already been mapped in this reflection context.
      /// </summary>
      /// <remarks>
      /// <para>
      /// Override this method to customize how a parameter is mapped in this reflection context.  This method is called
      /// by all other methods mapping a <see cref="ParameterInfo"/>, and it is only called if the parameter has not already
      /// been mapped in this reflection context. 
      /// </para>
      /// <para>The default implementation returns a
      /// <see cref="ProjectedParameterInfo{CustomReflectionContextBase}"/>
      /// that simply delegates all operations to the mapped member.</para>
      /// </remarks>
      /// <param name="parameter">The <see cref="ParameterInfo"/> to map.</param>
      /// <returns>The representation of the <paramref name="parameter"/> in this reflection context.</returns>
      protected virtual ParameterInfo MapParameterCore(ParameterInfo parameter)
      {
         return new ProjectedParameterInfo<CustomReflectionContextBase>(parameter, this);
      }

      /// <summary>
      /// Performs projection of a constructor that has not already been mapped in this reflection context.
      /// </summary>
      /// <remarks>
      /// <para>
      /// Override this method to customize how a constructor is mapped in this reflection context.  This method is called
      /// by all other methods mapping a <see cref="ConstructorInfo"/>, and it is only called if the constructor has not already
      /// been mapped in this reflection context. 
      /// </para>
      /// <para>The default implementation returns a
      /// <see cref="ProjectedConstructorInfo{CustomReflectionContextBase}"/>
      /// that simply delegates all operations to the mapped member.</para>
      /// </remarks>
      /// <param name="constructor">The <see cref="ConstructorInfo"/> to map.</param>
      /// <returns>The representation of the <paramref name="constructor"/> in this reflection context.</returns>
      protected virtual ConstructorInfo MapConstructorCore(ConstructorInfo constructor)
      {
         return new ProjectedConstructorInfo<CustomReflectionContextBase>(constructor, this);
      }

      /// <summary>
      /// Performs projection of a property that has not already been mapped in this reflection context.
      /// </summary>
      /// <remarks>
      /// <para>
      /// Override this method to customize how a property is mapped in this reflection context.  This method is called
      /// by all other methods mapping a <see cref="PropertyInfo"/>, and it is only called if the property has not already
      /// been mapped in this reflection context. 
      /// </para>
      /// <para>The default implementation returns a
      /// <see cref="ProjectedPropertyInfo{CustomReflectionContextBase}"/>
      /// that simply delegates all operations to the mapped member.</para>
      /// </remarks>
      /// <param name="property">The <see cref="PropertyInfo"/> to map.</param>
      /// <returns>The representation of the <paramref name="property"/> in this reflection context.</returns>
      protected virtual PropertyInfo MapPropertyCore(PropertyInfo property)
      {
         //return (PropertyInfo)m_tbl.GetValue(property, p => new ProjectedPropertyInfo<CustomReflectionContextBase>((PropertyInfo)p, this));
         return new ProjectedPropertyInfo<CustomReflectionContextBase>(property, this);
      }

      /// <summary>
      /// Performs projection of an event that has not already been mapped in this reflection context.
      /// </summary>
      /// <remarks>
      /// <para>
      /// Override this method to customize how an event is mapped in this reflection context.  This method is called
      /// by all other methods mapping a <see cref="EventInfo"/>, and it is only called if the event has not already
      /// been mapped in this reflection context. 
      /// </para>
      /// <para>The default implementation returns a
      /// <see cref="ProjectedEventInfo{CustomReflectionContextBase}"/>
      /// that simply delegates all operations to the mapped member.</para>
      /// </remarks>
      /// <param name="eventInfo">The <see cref="EventInfo"/> to map.</param>
      /// <returns>The representation of the <paramref name="eventInfo"/> in this reflection context.</returns>
      protected virtual EventInfo MapEventCore(EventInfo eventInfo)
      {
         return new ProjectedEventInfo<CustomReflectionContextBase>(eventInfo, this);
      }

      /// <summary>
      /// Performs projection of a field that has not already been mapped in this reflection context.
      /// </summary>
      /// <remarks>
      /// <para>
      /// Override this method to customize how a field is mapped in this reflection context.  This method is called
      /// by all other methods mapping a <see cref="FieldInfo"/>, and it is only called if the field has not already
      /// been mapped in this reflection context. 
      /// </para>
      /// <para>The default implementation returns a
      /// <see cref="ProjectedFieldInfo{CustomReflectionContextBase}"/>
      /// that simply delegates all operations to the mapped member.</para>
      /// </remarks>
      /// <param name="field">The <see cref="FieldInfo"/> to map.</param>
      /// <returns>The representation of the <paramref name="field"/> in this reflection context.</returns>
      protected virtual FieldInfo MapFieldCore(FieldInfo field)
      {
         return new ProjectedFieldInfo<CustomReflectionContextBase>(field, this);
      }

      /// <summary>
      /// Performs projection of an assembly that has not already been mapped in this reflection context.
      /// </summary>
      /// <remarks>
      /// <para>
      /// Override this method to customize how an assembly is mapped in this reflection context.  This method is called
      /// by all other methods mapping an <see cref="Assembly"/>, and it is only called if the assembly has not already
      /// been mapped in this reflection context. 
      /// </para>
      /// <para>The default implementation returns a
      /// <see cref="ProjectedAssembly{CustomReflectionContextBase}"/>
      /// that simply delegates all operations to the mapped member.</para>
      /// </remarks>
      /// <param name="assembly">The <see cref="Assembly"/> to map.</param>
      /// <returns>The representation of the <paramref name="assembly"/> in this reflection context.</returns>
      protected virtual Assembly MapAssemblyCore(Assembly assembly)
      {
         return new ProjectedAssembly<CustomReflectionContextBase>(assembly, this);
      }

      /// <summary>
      /// Performs projection of a method that has not already been mapped in this reflection context.
      /// </summary>
      /// <remarks>
      /// <para>
      /// Override this method to customize how a method is mapped in this reflection context.  This method is called
      /// by all other methods mapping a <see cref="MethodInfo"/>, and it is only called if the method has not already
      /// been mapped in this reflection context. 
      /// </para>
      /// <para>The default implementation returns a
      /// <see cref="ProjectedMethodInfo{CustomReflectionContextBase}"/>
      /// that simply delegates all operations to the mapped member.</para>
      /// </remarks>
      /// <param name="method">The <see cref="MethodInfo"/> to map.</param>
      /// <returns>The representation of the <paramref name="method"/> in this reflection context.</returns>
      protected virtual MethodInfo MapMethodCore(MethodInfo method)
      {
         return new ProjectedMethodInfo<CustomReflectionContextBase>(method, this);
      }

      /// <summary>
      /// Performs projection of a type that has not already been mapped in this reflection context.
      /// </summary>
      /// <remarks>
      /// <para>
      /// Override this method to customize how a type is mapped in this reflection context.  This method is called
      /// by all other methods mapping a <see cref="Type"/>, and it is only called if the type has not already
      /// been mapped in this reflection context. 
      /// </para>
      /// <para>The default implementation returns a
      /// <see cref="ProjectedType{CustomReflectionContextBase}"/>
      /// that simply delegates all operations to the mapped type.</para>
      /// </remarks>
      /// <param name="type">The <see cref="Type"/> to map.</param>
      /// <returns>The representation of the <paramref name="type"/> in this reflection context.</returns>
      protected virtual Type MapTypeCore(Type type)
      {
         return new ProjectedType<CustomReflectionContextBase>(type, this);
      }
      #endregion

      private object[] AddContextIdentifierAttribute(object[] attributes)
      {
         Type elementType = attributes.GetType().GetElementType();
         object[] newArray = (object[])Array.CreateInstance(elementType, attributes.Length + 1);
         Array.Copy(attributes, newArray, attributes.Length);
         newArray[attributes.Length] = ContextIdentifierAttribute;
         return newArray;
      }

      private object[] AddContextIdentifierAttributeIfRequested(Type requestedAttributeType, object[] attributes)
      {
         if (!requestedAttributeType.IsAssignableFrom(typeof(CustomReflectionContextIdAttribute)))
            return attributes;

         return AddContextIdentifierAttribute(attributes);
      }

      private static readonly HashSet<string> m_systemMemberTypeNames = new HashSet<string>()
      {
         "System.Reflection.RuntimePropertyInfo",
         "System.Reflection.RuntimeFieldInfo",
         "System.Reflection.RuntimeMethodInfo",
         "System.Reflection.RuntimeParameterInfo",
         "System.Reflection.RuntimeConstructorInfo",
         "System.Reflection.RuntimeAssembly",
         "System.Reflection.RuntimeEventInfo",
         "System.RuntimeType",
      };

      private bool IsSystemMemberInfo(ICustomAttributeProvider member)
      {
         string typeName = member.GetType().FullName;
         return m_systemMemberTypeNames.Contains(typeName);
      }

      /// <summary>
      /// Determines if the specified <paramref name="member"/> has previously been mapped in this
      /// reflection context.
      /// </summary>
      /// <remarks>
      /// This works by examining if the member has been decorated with the
      /// <see cref="CustomReflectionContextIdAttribute"/> with the <see cref="Id"/> of this reflection
      /// context, so it will work even if the member was mapped in other reflection contexts after
      /// being mapped in this.
      /// </remarks>
      /// <param name="member">The member to examine.</param>
      /// <returns>
      /// <see langword="true"/> if the member has been mapped in this reflection context, or
      /// <see langword="false"/> otherwise.
      /// </returns>
      protected bool IsMapped(ICustomAttributeProvider member)
      {
         // Shortcut which is faster than using reflection when not necessary.
         if (member is IProjector projector && projector.ReflectionContext.Id == Id)
            return true;

         if (IsSystemMemberInfo(member))
            return false;

         return member.GetCustomAttributes(typeof(CustomReflectionContextIdAttribute), false)
                                     .OfType<CustomReflectionContextIdAttribute>()
                                     .Any(attr => attr.ContextId == ContextIdentifierAttribute.ContextId);
      }
   }



}
