using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Alphaleonis.Reflection.Context
{
   // TODO PP (2018-04-30): Document
   public abstract partial class CustomReflectionContextBase : ReflectionContext
   {
      //private readonly ConditionalWeakTable<object, object> m_tbl = new ConditionalWeakTable<object, object>();

      public CustomReflectionContextBase()
      {
         Id = Guid.NewGuid();
         ContextIdentifierAttribute = new CustomReflectionContextIdAttribute(Id);
      }

      protected Guid Id { get; }

      protected CustomReflectionContextIdAttribute ContextIdentifierAttribute { get; }

      public IEnumerable<TypeInfo> MapTypes(IEnumerable<Type> types)
      {
         return types.Select(type => MapType(type));
      }

      public sealed override Assembly MapAssembly(Assembly assembly)
      {
         if (IsMapped(assembly))
            return assembly;

         return MapAssemblyCore(assembly);
   
      }

      public TypeInfo MapType(Type type)
      {
         if (type == null)
            return null;

         //return (TypeInfo)m_tbl.GetValue(type, t => {
            if (IsMapped(type))
               return type.GetTypeInfo();

            return MapTypeCore(type).GetTypeInfo();
         //});
            
      }

      public sealed override TypeInfo MapType(TypeInfo type)
      {
         return MapType((Type)type).GetTypeInfo();
      }

      #region Map Member

      #region Private MapMember methods

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

      protected FieldInfo MapMember(FieldInfo field)
      {
         if (field == null)
            return null;

         if (IsMapped(field))
            return field;

         return MapFieldCore(field);
      }

      protected EventInfo MapMember(EventInfo member)
      {
         if (member == null)
            return null;

         if (IsMapped(member))
            return member;

         return MapEventCore(member);
      }

      protected ConstructorInfo MapMember(ConstructorInfo constructor)
      {
         if (constructor == null)
            return null;

         if (IsMapped(constructor))
            return constructor;

         return MapConstructorCore(constructor);
      }

      protected ParameterInfo MapParameter(ParameterInfo parameter)
      {
         if (parameter == null)
            return null;

         if (IsMapped(parameter))
            return parameter;

         return MapParameterCore(parameter);
      }

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



      protected PropertyInfo MapMember(PropertyInfo property)
      {
         if (property == null)
            return null;

         if (IsMapped(property))
            return property;

         return MapPropertyCore(property);
      }

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

      protected virtual ParameterInfo MapParameterCore(ParameterInfo parameter)
      {
         return new ProjectedParameterInfo<CustomReflectionContextBase>(parameter, this);
      }

      protected virtual ConstructorInfo MapConstructorCore(ConstructorInfo constructor)
      {
         return new ProjectedConstructorInfo<CustomReflectionContextBase>(constructor, this);
      }

      protected virtual PropertyInfo MapPropertyCore(PropertyInfo property)
      {
         //return (PropertyInfo)m_tbl.GetValue(property, p => new ProjectedPropertyInfo<CustomReflectionContextBase>((PropertyInfo)p, this));
         return new ProjectedPropertyInfo<CustomReflectionContextBase>(property, this);
      }

      protected virtual EventInfo MapEventCore(EventInfo eventInfo)
      {
         return new ProjectedEventInfo<CustomReflectionContextBase>(eventInfo, this);
      }

      protected virtual FieldInfo MapFieldCore(FieldInfo field)
      {
         return new ProjectedFieldInfo<CustomReflectionContextBase>(field, this);
      }

      protected virtual Assembly MapAssemblyCore(Assembly assembly)
      {
         return new ProjectedAssembly<CustomReflectionContextBase>(assembly, this);
      }

      protected virtual MethodInfo MapMethodCore(MethodInfo method)
      {
         return new ProjectedMethodInfo<CustomReflectionContextBase>(method, this);
      }

      protected virtual Type MapTypeCore(Type type)
      {
         return new ProjectedType<CustomReflectionContextBase>(type, this);
      }

      protected virtual object[] AddContextIdentifierAttribute(object[] attributes)
      {
         Type elementType = attributes.GetType().GetElementType();
         object[] newArray = (object[])Array.CreateInstance(elementType, attributes.Length + 1);
         Array.Copy(attributes, newArray, attributes.Length);
         newArray[attributes.Length] = ContextIdentifierAttribute;
         return newArray;
      }

      protected object[] AddContextIdentifierAttributeIfRequested(Type requestedAttributeType, object[] attributes)
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

      protected bool IsSystemMemberInfo(ICustomAttributeProvider member)
      {
         string typeName = member.GetType().FullName;
         return m_systemMemberTypeNames.Contains(typeName);
      }

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
