using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CustomAttributeTable;

namespace CustomAttributeTableTests
{
   [Flags]
   public enum AttributeTableReflectionContextOptions
   {
      Default = 0,
      HonorPropertyAttributeInheritance = 1,
      HonorEventAttributeInheritance = 2
   }

   public partial class AttributeTableReflectionContext : ReflectionContext
   {
      private readonly static AttributeUsageAttribute DefaultAttributeUsageAttribute = new AttributeUsageAttribute(AttributeTargets.All);
      private readonly AttributeTableReflectionContextOptions m_options;

      public AttributeTableReflectionContext(ICustomAttributeTable table, AttributeTableReflectionContextOptions options)
      {
         if (table == null)
            throw new ArgumentNullException(nameof(table), $"{nameof(table)} is null.");

         Id = Guid.NewGuid();
         ContextIdentifierAttribute = new AttributeTableReflectionContextIdentifierAttribute(Guid.NewGuid());
         Table = table;
         m_options = options;
      }

      private Guid Id { get; }
      private ICustomAttributeTable Table { get; }
      private AttributeTableReflectionContextIdentifierAttribute ContextIdentifierAttribute { get; }

      #region Public Methods

      public override Assembly MapAssembly(Assembly assembly)
      {
         if (IsMapped(assembly))
            return assembly;

         return new AttributeTableProjectedAssembly(assembly, this);
      }

      public IEnumerable<TypeInfo> MapTypes(IEnumerable<Type> types)
      {
         return types.Select(type => MapType(type));
      }

      public TypeInfo MapType(Type type)
      {
         if (type == null)
            return null;

         if (type is AttributeTableProjectedType)
            return type.GetTypeInfo();

         if (IsMapped(type))
            return type.GetTypeInfo();

         return new AttributeTableProjectedType(type, this);
      }

      public override TypeInfo MapType(TypeInfo type)
      {
         return MapType((Type)type).GetTypeInfo();
      }

      #endregion

      private MemberInfo MapMember(MemberInfo member)
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

      private Type[] MapTypes(Type[] types)
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

      private FieldInfo[] MapMembers(FieldInfo[] members)
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

      private PropertyInfo[] MapMembers(PropertyInfo[] members)
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

      private EventInfo[] MapMembers(EventInfo[] members)
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

      private ConstructorInfo[] MapMembers(ConstructorInfo[] members)
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

      private MethodInfo[] MapMembers(MethodInfo[] members)
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

      private FieldInfo MapMember(FieldInfo field)
      {
         if (field == null)
            return null;

         if (IsMapped(field))
            return field;

         return new AttributeTableProjectedFieldInfo(field, this);
      }

      private EventInfo MapMember(EventInfo member)
      {
         if (member == null)
            return null;

         if (IsMapped(member))
            return member;

         return new AttributeTableProjectedEventInfo(member, this);
      }

      private ConstructorInfo MapMember(ConstructorInfo constructor)
      {
         if (constructor == null)
            return null;

         if (IsMapped(constructor))
            return constructor;

         return new AttributeTableProjectedConstructorInfo(constructor, this);
      }

      private ParameterInfo MapParameter(ParameterInfo parameter)
      {
         if (parameter == null)
            return null;

         if (IsMapped(parameter))
            return parameter;

         return new AttributeTableProjectedParameterInfo(parameter, this);
      }

      private ParameterInfo[] MapParameters(ParameterInfo[] parameters)
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

      private PropertyInfo MapMember(PropertyInfo property)
      {
         if (property == null)
            return null;

         if (IsMapped(property))
            return property;

         return new AttributeTableProjectedPropertyInfo(property, this);
      }

      private MethodInfo MapMember(MethodInfo method)
      {
         if (method == null)
            return null;

         if (IsMapped(method))
            return method;

         return new AttributeTableProjectedMethodInfo(method, this);
      }

      private bool IsMapped(ICustomAttributeProvider member)
      {
         IAttributeTableProjector otherType = member as IAttributeTableProjector;
         if (otherType?.ReflectionContext.Id == Id)
            return true;

         return member.GetCustomAttributes(typeof(AttributeTableReflectionContextIdentifierAttribute), false)
                                     .OfType<AttributeTableReflectionContextIdentifierAttribute>()
                                     .Any(attr => attr.ContextId == ContextIdentifierAttribute.ContextId);
      }

      private object[] AddContextIdentifierAttribute(object[] attributes)
      {
         Type elementType = attributes.GetType().GetElementType();
         object[] newArray = (object[])Array.CreateInstance(elementType, attributes.Length + 1);
         Array.Copy(attributes, newArray, attributes.Length);
         newArray[attributes.Length] = ContextIdentifierAttribute;
         return newArray;
      }

      private IEnumerable<object> AddContextIdentifierAttribute(IEnumerable<object> attributes)
      {
         return attributes.Concat(new[] { ContextIdentifierAttribute });
      }

      internal static AttributeUsageAttribute GetAttributeUsage(ICustomAttributeProvider decoratedAttribute)
      {
         return decoratedAttribute.GetCustomAttributes(typeof(AttributeUsageAttribute), true).OfType<AttributeUsageAttribute>().FirstOrDefault() ?? DefaultAttributeUsageAttribute;
      }
   }




}
