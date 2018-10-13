// Copyright (c) Peter Palotas
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Alphaleonis.Reflection.Context
{
   
   /// <summary>A reflection context thats adds caching of some reflection lookups.</summary>
   
   public class CachingReflectionContext : ReflectionContext
   {
      private readonly Dictionary<Type, TypeInfo> m_mappedTypes = new Dictionary<Type, TypeInfo>(TypeEqualityComparer.Default);
      private readonly ReflectionContext m_baseContext;

      /// <summary>Constructor.</summary>
      /// <param name="baseContext">The underlying <see cref="ReflectionContext"/> to delegate calls to.</param>
      public CachingReflectionContext(ReflectionContext baseContext)
      {
         m_baseContext = baseContext;
      }

      /// <inheritdoc/> 
      public override Assembly MapAssembly(Assembly assembly)
      {
         return m_baseContext.MapAssembly(assembly);
      }

      /// <inheritdoc/> 
      public override TypeInfo MapType(TypeInfo type)
      {
         TypeInfo result;
         if (!m_mappedTypes.TryGetValue(type, out result))
         {
            result = new CachingProjectedType(m_baseContext.MapType(type));
            m_mappedTypes.Add(type, result);
         }

         return result;         
      }

      private class CachingProjectedProperty : DelegatingPropertyInfo
      {
         private struct Entry : IEquatable<Entry>
         {
            public Entry(bool inherit, Type attributeType)
            {
               Inherit = inherit;
               AttributeType = attributeType;
            }

            public bool Inherit { get; }
            public Type AttributeType { get; }

            public bool Equals(Entry other)
            {
               return Inherit == other.Inherit && AttributeType == other.AttributeType;
            }

            public override bool Equals(object obj)
            {
               if (obj is Entry entry)
                  return Equals(entry);
               else
                  return false;
            }

            public override int GetHashCode()
            {
               return Inherit.GetHashCode() + AttributeType.GetHashCode();
            }
         }

         private readonly Dictionary<Entry, object[]> m_attrs = new Dictionary<Entry, object[]>();

         public CachingProjectedProperty(PropertyInfo property) 
            : base(property)
         {
         }

         public override object[] GetCustomAttributes(bool inherit)
         {
            object[] result;
            if (!m_attrs.TryGetValue(new Entry(inherit, typeof(object)), out result))
            {
               result = base.GetCustomAttributes(inherit);
               m_attrs.Add(new Entry(inherit, typeof(object)), result);
            }

            return result;
         }

         public override object[] GetCustomAttributes(Type attributeType, bool inherit)
         {  
            object[] result;
            if (!m_attrs.TryGetValue(new Entry(inherit, attributeType), out result))
            {
               result = base.GetCustomAttributes(attributeType, inherit);
               m_attrs.Add(new Entry(inherit, attributeType), result);
            }

            return result;
         }

         public override bool IsDefined(Type attributeType, bool inherit)
         {
            return GetCustomAttributes(attributeType, inherit).Any();
         }
      }

      private class CachingProjectedType : DelegatingType
      {
         private PropertyInfo[] publicInstanceProperties;
         private PropertyInfo[] publicInstanceAndStaticProperties;

         public CachingProjectedType(Type delegatingType)
            : base(delegatingType)
         {
         }

         public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
         {
            if (bindingAttr == (BindingFlags.Public | BindingFlags.Instance))
            {
               if (publicInstanceProperties == null)
               {
                  publicInstanceProperties = base.GetProperties(bindingAttr).Select(p => new CachingProjectedProperty(p)).ToArray();
               }

               return publicInstanceProperties;
            }
            else if (bindingAttr == (BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
            {
               if (publicInstanceAndStaticProperties == null)
               {
                  publicInstanceAndStaticProperties = base.GetProperties(bindingAttr).Select(p => new CachingProjectedProperty(p)).ToArray();
               }

               return publicInstanceAndStaticProperties;
            }
            else
            {
               return base.GetProperties(bindingAttr);
            }
         }
      }
   }
}
