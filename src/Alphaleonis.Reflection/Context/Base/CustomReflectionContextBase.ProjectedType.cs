// Copyright (c) Peter Palotas
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Alphaleonis.Reflection.Context
{
   public partial class CustomReflectionContextBase
   {
      /// <summary>
      /// A default projector that wraps a <see cref="Type"/> for a reflection context.
      /// </summary>
      /// <remarks>
      /// This projector is intended to be used as a base class for projectors in derived reflection contexts. It 
      /// adds no functionality to the underlying object, other than ensuring that all
      /// types and members returned from the object are also mapped in the same reflection context.
      /// </remarks>
      /// <typeparam name="TContext">The type of the reflection context. Must inherit from
      /// <see cref="CustomReflectionContextBase"/>.</typeparam>
      protected class ProjectedType<TContext> : DelegatingType, IProjector<TContext> where TContext : CustomReflectionContextBase
      {
         #region Constructor

         /// <summary>Constructor.</summary>
         /// <param name="type">The type to wrap.</param>
         /// <param name="context">The owner reflection context.</param>
         public ProjectedType(Type type, TContext context)
            : base(type)
         {
            ReflectionContext = context;
         }

         #endregion

         /// <summary>The owner reflection context.</summary>
         public TContext ReflectionContext { get; }

         /// <inheritdoc/> 
         CustomReflectionContextBase IProjector.ReflectionContext => ReflectionContext;

         /// <inheritdoc/> 
         public override Type BaseType => ReflectionContext.MapType(base.BaseType?.GetTypeInfo());

         /// <inheritdoc/> 
         public override Assembly Assembly => ReflectionContext.MapAssembly(base.Assembly);

         /// <inheritdoc/> 
         public override object[] GetCustomAttributes(bool inherit) => GetCustomAttributes(typeof(Attribute), inherit);

         /// <inheritdoc/> 
         public override object[] GetCustomAttributes(Type attributeType, bool inherit) => ReflectionContext.AddContextIdentifierAttributeIfRequested(attributeType, base.GetCustomAttributes(attributeType, inherit));

         /// <inheritdoc/> 
         public override bool IsDefined(Type attributeType, bool inherit)
         {
            if (attributeType.IsAssignableFrom(typeof(CustomReflectionContextIdAttribute)))
               return true;

            return base.IsDefined(attributeType, inherit);
         }

         /// <inheritdoc/> 
         protected override PropertyInfo GetPropertyImpl(string name, BindingFlags bindingAttr, Binder binder, Type returnType, Type[] types, ParameterModifier[] modifiers)
         {
            return ReflectionContext.MapMember(base.GetPropertyImpl(name, bindingAttr, binder, returnType, types, modifiers));
         }

         /// <inheritdoc/> 
         public override PropertyInfo[] GetProperties(BindingFlags bindingAttr) => ReflectionContext.MapMembers(base.GetProperties(bindingAttr));

         /// <inheritdoc/> 
         public override MethodInfo[] GetMethods(BindingFlags bindingAttr) => ReflectionContext.MapMembers(base.GetMethods(bindingAttr));

         /// <inheritdoc/> 
         public override Type[] FindInterfaces(TypeFilter filter, object filterCriteria) 
            => ReflectionContext.MapTypes(base.FindInterfaces(filter, filterCriteria));

         /// <inheritdoc/> 
         public override MemberInfo[] FindMembers(MemberTypes memberType, BindingFlags bindingAttr, MemberFilter filter, object filterCriteria) 
            => ReflectionContext.MapMembers(base.FindMembers(memberType, bindingAttr, filter, filterCriteria));

         /// <inheritdoc/> 
         protected override ConstructorInfo GetConstructorImpl(BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers)
            => ReflectionContext.MapMember(base.GetConstructorImpl(bindingAttr, binder, callConvention, types, modifiers));

         /// <inheritdoc/> 
         public override ConstructorInfo[] GetConstructors(BindingFlags bindingAttr) => ReflectionContext.MapMembers(base.GetConstructors(bindingAttr));

         /// <inheritdoc/> 
         public override MemberInfo[] GetDefaultMembers() => ReflectionContext.MapMembers(base.GetDefaultMembers());

         /// <inheritdoc/> 
         public override Type GetElementType() => ReflectionContext.MapType(base.GetElementType());

         /// <inheritdoc/> 
         public override Type GetEnumUnderlyingType() => ReflectionContext.MapType(base.GetEnumUnderlyingType());

         /// <inheritdoc/> 
         public override EventInfo GetEvent(string name, BindingFlags bindingAttr) => ReflectionContext.MapMember(base.GetEvent(name, bindingAttr));

         /// <inheritdoc/> 
         public override EventInfo[] GetEvents(BindingFlags bindingAttr) => ReflectionContext.MapMembers(base.GetEvents(bindingAttr));

         /// <inheritdoc/> 
         public override EventInfo[] GetEvents() => ReflectionContext.MapMembers(base.GetEvents());

         /// <inheritdoc/> 
         public override FieldInfo GetField(string name, BindingFlags bindingAttr) => ReflectionContext.MapMember(base.GetField(name, bindingAttr));

         /// <inheritdoc/> 
         public override FieldInfo[] GetFields(BindingFlags bindingAttr) => ReflectionContext.MapMembers(base.GetFields(bindingAttr));

         /// <inheritdoc/> 
         public override Type[] GetGenericArguments() => ReflectionContext.MapTypes(base.GetGenericArguments());

         /// <inheritdoc/> 
         public override Type[] GetGenericParameterConstraints() => ReflectionContext.MapTypes(base.GetGenericParameterConstraints());

         /// <inheritdoc/> 
         public override Type GetGenericTypeDefinition() => ReflectionContext.MapType(base.GetGenericTypeDefinition());

         /// <inheritdoc/> 
         public override Type GetInterface(string name, bool ignoreCase) => ReflectionContext.MapType(base.GetInterface(name, ignoreCase));

         /// <inheritdoc/> 
         public override InterfaceMapping GetInterfaceMap(Type interfaceType)
         {            
            var map = base.GetInterfaceMap(interfaceType);
            map.InterfaceMethods = ReflectionContext.MapMembers(map.InterfaceMethods);
            map.InterfaceType = ReflectionContext.MapType(map.InterfaceType);
            map.TargetMethods = ReflectionContext.MapMembers(map.TargetMethods);
            map.TargetType = ReflectionContext.MapType(map.TargetType);
            return map;
         }

         /// <inheritdoc/> 
         public override Type[] GetInterfaces() => ReflectionContext.MapTypes(base.GetInterfaces());

         /// <inheritdoc/> 
         public override MemberInfo[] GetMember(string name, BindingFlags bindingAttr) => ReflectionContext.MapMembers(base.GetMember(name, bindingAttr));

         /// <inheritdoc/> 
         public override MemberInfo[] GetMember(string name, MemberTypes type, BindingFlags bindingAttr) => ReflectionContext.MapMembers(base.GetMember(name, type, bindingAttr));

         /// <inheritdoc/> 
         public override MemberInfo[] GetMembers(BindingFlags bindingAttr) => ReflectionContext.MapMembers(base.GetMembers(bindingAttr));

         /// <inheritdoc/> 
         protected override MethodInfo GetMethodImpl(string name, BindingFlags bindingAttr, Binder binder, CallingConventions callConvention, Type[] types, ParameterModifier[] modifiers) => ReflectionContext.MapMember(base.GetMethodImpl(name, bindingAttr, binder, callConvention, types, modifiers));

         /// <inheritdoc/> 
         public override Type GetNestedType(string name, BindingFlags bindingAttr) => ReflectionContext.MapType(base.GetNestedType(name, bindingAttr));

         /// <inheritdoc/> 
         public override Type[] GetNestedTypes(BindingFlags bindingAttr) => ReflectionContext.MapTypes(base.GetNestedTypes(bindingAttr));

         /// <inheritdoc/> 
         public override Type MakeArrayType() => ReflectionContext.MapType(base.MakeArrayType());

         /// <inheritdoc/> 
         public override Type MakeArrayType(int rank) => ReflectionContext.MapType(base.MakeArrayType(rank));

         /// <inheritdoc/> 
         public override Type MakeByRefType() => ReflectionContext.MapType(base.MakeByRefType());

         /// <inheritdoc/> 
         public override Type MakeGenericType(params Type[] typeArguments) => ReflectionContext.MapType(base.MakeGenericType(typeArguments));

         /// <inheritdoc/> 
         public override Type MakePointerType() => ReflectionContext.MapType(base.MakePointerType());

         /// <inheritdoc/> 
         public override MethodBase DeclaringMethod => ReflectionContext.MapMember(base.DeclaringMethod) as MethodBase;

         /// <inheritdoc/> 
         public override Type DeclaringType => ReflectionContext.MapType(base.DeclaringType);

         /// <inheritdoc/> 
         public override Type[] GenericTypeArguments => ReflectionContext.MapTypes(base.GenericTypeArguments);

         /// <inheritdoc/> 
         public override Type ReflectedType => ReflectionContext.MapType(base.ReflectedType);
      }
   }

   
}
