using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace CustomAttributeTable
{
   public class CustomAttributeTableBuilder
   {
      private ImmutableDictionary<Type, TypeMetadata>.Builder m_metadata;

      public CustomAttributeTableBuilder()
      {
         m_metadata = ImmutableDictionary.CreateBuilder<Type, TypeMetadata>(TypeEqualityComparer.Default);
      }

      public ICustomAttributeTable CreateTable()
      {
         return new CustomAttributeTable(m_metadata.ToImmutable());
      }

      public void AddAttributes(Type type, string memberName, IEnumerable<Attribute> attributes)
      {
         m_metadata[type] = GetTypeMetadata(type).AddMemberAttributes(memberName, attributes);
      }

      public void AddAttributes(MemberInfo member, IEnumerable<Attribute> attributes)
      {
         if (member == null)
            throw new ArgumentNullException(nameof(member), $"{nameof(member)} is null.");

         if (attributes == null)
            throw new ArgumentNullException(nameof(attributes), $"{nameof(attributes)} is null.");

         if (member is Type)
         {
            Type type = member as Type;
            m_metadata[type] = GetTypeMetadata(type).AddTypeAttributes(attributes);
         }
         else if (member is MethodBase)
         {
            var method = member as MethodBase;
            m_metadata[method.DeclaringType] = GetTypeMetadata(method.DeclaringType).AddMethodAttributes(new MethodKey(method), attributes);            
         }
         else
         {
            AddAttributes(member.DeclaringType, member.Name, attributes);
         }
      }

      private TypeMetadata GetTypeMetadata(Type type)
      {
         TypeMetadata metadata;
         if (!m_metadata.TryGetValue(type, out metadata))
         {
            metadata = TypeMetadata.Empty;
         }
         return metadata;
      }

      private class MethodMetadata
      {
         public MethodMetadata(int parameterCount)
         {
            ParameterAttributes = ImmutableList.CreateRange(Enumerable.Range(1, parameterCount).Select(p => (IImmutableList<Attribute>)ImmutableList<Attribute>.Empty)).ToImmutableList();
            ReturnParameterAttributes = ImmutableList<Attribute>.Empty;
            MethodAttributes = ImmutableList<Attribute>.Empty;
         }

         public MethodMetadata(IImmutableList<IImmutableList<Attribute>> parameterAttributes, IImmutableList<Attribute> returnParameterAttributes, IImmutableList<Attribute> methodAttributes)
         {
            ParameterAttributes = parameterAttributes;
            ReturnParameterAttributes = returnParameterAttributes;
            MethodAttributes = methodAttributes;
         }

         public IImmutableList<IImmutableList<Attribute>> ParameterAttributes { get; }
         public IImmutableList<Attribute> ReturnParameterAttributes { get; }
         public IImmutableList<Attribute> MethodAttributes { get; }

         public MethodMetadata AddParameterAttributes(int parameterIndex, IEnumerable<Attribute> attributes)
         {
            return new MethodMetadata(ParameterAttributes.SetItem(parameterIndex, ParameterAttributes[parameterIndex].AddRange(attributes)), ReturnParameterAttributes, MethodAttributes);
         }

         public MethodMetadata AddReturnParameterAttributes(IEnumerable<Attribute> attributes)
         {
            return new MethodMetadata(ParameterAttributes, ReturnParameterAttributes.AddRange(attributes), MethodAttributes);
         }

         public MethodMetadata AddMethodAttributes(IEnumerable<Attribute> methodAttributes)
         {
            return new MethodMetadata(ParameterAttributes, ReturnParameterAttributes, MethodAttributes.AddRange(methodAttributes));
         }
      }

      private class TypeMetadata
      {
         public static readonly TypeMetadata Empty = new TypeMetadata();

         #region Constructors

         private TypeMetadata()
         {
            MemberAttributes = ImmutableDictionary<string, IImmutableList<Attribute>>.Empty;
            MethodAttributes = ImmutableDictionary<MethodKey, MethodMetadata>.Empty;
            TypeAttributes = ImmutableList<Attribute>.Empty;
         }

         private TypeMetadata(IImmutableList<Attribute> typeAttributes, IImmutableDictionary<string, IImmutableList<Attribute>> memberAttributes, IImmutableDictionary<MethodKey, MethodMetadata> methodAttributes)
         {
            MemberAttributes = memberAttributes;
            MethodAttributes = methodAttributes;
            TypeAttributes = typeAttributes;
         }

         #endregion

         #region Properties

         public IImmutableDictionary<string, IImmutableList<Attribute>> MemberAttributes { get; }
         public IImmutableDictionary<MethodKey, MethodMetadata> MethodAttributes { get; }
         public IImmutableList<Attribute> TypeAttributes { get; }

         #endregion

         #region Methods

         public TypeMetadata AddTypeAttributes(IEnumerable<Attribute> attributes)
         {
            return new TypeMetadata(TypeAttributes.AddRange(attributes), MemberAttributes, MethodAttributes);
         }

         public TypeMetadata AddMethodAttributes(MethodKey method, IEnumerable<Attribute> attributes)
         {
            var methodMetadata = GetMethodMetadata(method).AddMethodAttributes(attributes);
            return new TypeMetadata(TypeAttributes, MemberAttributes, MethodAttributes.SetItem(method, methodMetadata));
         }

         public TypeMetadata AddMethodReturnParameterAttributes(MethodKey method, IEnumerable<Attribute> attributes)
         {
            var methodMetadata = GetMethodMetadata(method).AddReturnParameterAttributes(attributes);
            return new TypeMetadata(TypeAttributes, MemberAttributes, MethodAttributes.SetItem(method, methodMetadata));
         }

         public TypeMetadata AddMethodParameterAttributes(MethodKey method, int parameterIndex, IEnumerable<Attribute> attributes)
         {
            var methodMetadata = GetMethodMetadata(method).AddParameterAttributes(parameterIndex, attributes);
            return new TypeMetadata(TypeAttributes, MemberAttributes, MethodAttributes.SetItem(method, methodMetadata));
         }

         public TypeMetadata AddMemberAttributes(string memberName, IEnumerable<Attribute> attributes)
         {
            IImmutableList<Attribute> list;
            if (!MemberAttributes.TryGetValue(memberName, out list))
            {
               list = ImmutableList.CreateRange<Attribute>(attributes);
            }
            else
            {
               list = list.AddRange(attributes);
            }

            return new TypeMetadata(TypeAttributes, MemberAttributes.SetItem(memberName, list), MethodAttributes);
         }

         private MethodMetadata GetMethodMetadata(MethodKey key)
         {
            MethodMetadata metadata;
            if (!MethodAttributes.TryGetValue(key, out metadata))
            {
               metadata = new MethodMetadata(key.Parameters.Count);
            }

            return metadata;
         }
         #endregion
      }

      public class MethodKey : IEquatable<MethodKey>
      {
         public MethodKey(MethodBase method)
         {
            if (method == null)
               throw new ArgumentNullException(nameof(method), $"{nameof(method)} is null.");

            MethodName = method.Name;
            TypeArguments = method.GetGenericArguments().ToImmutableArray();
            Parameters = method.GetParameters().Select(p => p.ParameterType).ToImmutableArray();
         }

         public string MethodName { get; }

         public IImmutableList<Type> TypeArguments { get; }

         public IImmutableList<Type> Parameters { get; }

         public bool Equals(MethodKey other)
         {
            if (Object.ReferenceEquals(other, null))
               return false;

            return MethodName.Equals(other.MethodName) &&
                   TypeArguments.SequenceEqual(other.TypeArguments) &&
                   Parameters.SequenceEqual(other.Parameters);
         }

         public override bool Equals(object obj)
         {
            return Equals(obj as MethodKey);
         }

         public override int GetHashCode()
         {
            return MethodName.GetHashCode() + 11 * Parameters.Count.GetHashCode();
         }
      }

      private class CustomAttributeTable : ICustomAttributeTable
      {
         private ImmutableDictionary<Type, TypeMetadata> m_metadata;

         public CustomAttributeTable(ImmutableDictionary<Type, TypeMetadata> metadata)
         {
            m_metadata = metadata;
         }

         private TypeMetadata GetTypeMetadata(Type type)
         {
            TypeMetadata metadata;
            if (m_metadata.TryGetValue(type, out metadata))
            {
               return metadata;
            }

            return TypeMetadata.Empty;
         }

         private IEnumerable<Attribute> GetMemberAttributes(MemberInfo member)
         {
            IImmutableList<Attribute> attributes;
            if (GetTypeMetadata(member.DeclaringType).MemberAttributes.TryGetValue(member.Name, out attributes))
            {
               return attributes;
            }
            else
            {
               return ImmutableList<Attribute>.Empty;
            }
         }
         public IEnumerable<Attribute> GetCustomAttributes(MemberInfo member, bool inherit = false)
         {
            if (member == null)
               throw new ArgumentNullException(nameof(member));

            switch (member.MemberType)
            {
               case MemberTypes.Event:
               case MemberTypes.Field:
               case MemberTypes.Property:
                  var result = GetMemberAttributes(member);
                  
                  return result;                  

               case MemberTypes.Method:
               case MemberTypes.Constructor:
                  MethodMetadata methodMetadata;
                  if (GetTypeMetadata(member.DeclaringType).MethodAttributes.TryGetValue(new MethodKey(member as MethodBase), out methodMetadata))
                  {
                     return methodMetadata.MethodAttributes;
                  }
                  else
                  {
                     return ImmutableList<Attribute>.Empty;
                  }

               case MemberTypes.TypeInfo:
               case MemberTypes.NestedType:
                  Type type = (Type)member;
                  IEnumerable<Attribute> typeAttributes = GetTypeMetadata(member as Type).TypeAttributes;
                  
                  return typeAttributes;


               case MemberTypes.Custom:
               default:
                  return ImmutableList<Attribute>.Empty;
            }
         }

         public IEnumerable<Attribute> GetCustomAttributes(ParameterInfo parameterInfo)
         {
            if (parameterInfo == null)
               throw new ArgumentNullException(nameof(parameterInfo));

            var typeMetadata = GetTypeMetadata(parameterInfo.Member.DeclaringType);
            MethodMetadata methodMetadata;
            if (typeMetadata.MethodAttributes.TryGetValue(new MethodKey(parameterInfo.Member as MethodBase), out methodMetadata))
            {
               return methodMetadata.ParameterAttributes[parameterInfo.Position];
            }
            else
            {
               return ImmutableList<Attribute>.Empty;
            }
         }
      }
   }


   //internal static Object[] GetCustomAttributes(RuntimeMethodInfo method, RuntimeType caType, bool inherit)
   //{
   //   Contract.Requires(method != null);
   //   Contract.Requires(caType != null);

   //   if (method.IsGenericMethod && !method.IsGenericMethodDefinition)
   //      method = method.GetGenericMethodDefinition() as RuntimeMethodInfo;

   //   int pcaCount = 0;
   //   Attribute[] pca = PseudoCustomAttribute.GetCustomAttributes(method, caType, true, out pcaCount);

   //   // if we are asked to go up the hierarchy chain we have to do it now and regardless of the
   //   // attribute usage for the specific attribute because a derived attribute may override the usage...           
   //   // ... however if the attribute is sealed we can rely on the attribute usage
   //   if (!inherit || (caType.IsSealed && !CustomAttribute.GetAttributeUsage(caType).Inherited))
   //   {
   //      object[] attributes = GetCustomAttributes(method.GetRuntimeModule(), method.MetadataToken, pcaCount, caType, !AllowCriticalCustomAttributes(method));
   //      if (pcaCount > 0) Array.Copy(pca, 0, attributes, attributes.Length - pcaCount, pcaCount);
   //      return attributes;
   //   }

   //   List<object> result = new List<object>();
   //   bool mustBeInheritable = false;
   //   bool useObjectArray = (caType == null || caType.IsValueType || caType.ContainsGenericParameters);
   //   Type arrayType = useObjectArray ? typeof(object) : caType;

   //   while (pcaCount > 0)
   //      result.Add(pca[--pcaCount]);

   //   while (method != null)
   //   {
   //      object[] attributes = GetCustomAttributes(method.GetRuntimeModule(), method.MetadataToken, 0, caType, mustBeInheritable, result, !AllowCriticalCustomAttributes(method));
   //      mustBeInheritable = true;
   //      for (int i = 0; i < attributes.Length; i++)
   //         result.Add(attributes[i]);

   //      method = method.GetParentDefinition();
   //   }

   //   object[] typedResult = CreateAttributeArrayHelper(arrayType, result.Count);
   //   Array.Copy(result.ToArray(), 0, typedResult, 0, result.Count);
   //   return typedResult;
   //}

   //[System.Security.SecuritySafeCritical]  // auto-generated
   //internal static Object[] GetCustomAttributes(RuntimeConstructorInfo ctor, RuntimeType caType)
   //{
   //   Contract.Requires(ctor != null);
   //   Contract.Requires(caType != null);

   //   int pcaCount = 0;
   //   Attribute[] pca = PseudoCustomAttribute.GetCustomAttributes(ctor, caType, true, out pcaCount);
   //   object[] attributes = GetCustomAttributes(ctor.GetRuntimeModule(), ctor.MetadataToken, pcaCount, caType, !AllowCriticalCustomAttributes(ctor));
   //   if (pcaCount > 0) Array.Copy(pca, 0, attributes, attributes.Length - pcaCount, pcaCount);
   //   return attributes;
   //}


}
