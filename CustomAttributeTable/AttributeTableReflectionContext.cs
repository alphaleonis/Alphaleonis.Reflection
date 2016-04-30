using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CustomAttributeTable;

namespace CustomAttributeTableTests
{
   public partial class AttributeTableReflectionContext : ReflectionContext
   {
      private readonly static AttributeUsageAttribute DefaultAttributeUsageAttribute = new AttributeUsageAttribute(AttributeTargets.All);

      public AttributeTableReflectionContext(ICustomAttributeTable table)
      {
         if (table == null)
            throw new ArgumentNullException(nameof(table), $"{nameof(table)} is null.");

         Id = Guid.NewGuid();
         ContextIdentifierAttribute = new AttributeTableReflectionContextIdentifierAttribute(Guid.NewGuid());
         Table = table;
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

      

      private FieldInfo MapMember(FieldInfo field)
      {
         if (IsMapped(field))
            return field;

         return new AttributeTableProjectedFieldInfo(field, this);
      }

      private ConstructorInfo MapMember(ConstructorInfo constructor)
      {
         if (IsMapped(constructor))
            return constructor;

         return new AttributeTableProjectedConstructorInfo(constructor, this);
      }

      private ParameterInfo MapParameter(ParameterInfo parameter)
      {
         if (IsMapped(parameter))
            return parameter;

         return new AttributeTableProjectedParameterInfo(parameter, this);
      }

      private PropertyInfo MapMember(PropertyInfo property)
      {
         if (IsMapped(property))
            return property;

         return new AttributeTableProjectedPropertyInfo(property, this);
      }

      private MethodInfo MapMember(MethodInfo method)
      {
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
