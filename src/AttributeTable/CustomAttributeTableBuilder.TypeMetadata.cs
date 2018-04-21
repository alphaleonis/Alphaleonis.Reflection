using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Alphaleonis.Reflection
{
   public partial class CustomAttributeTableBuilder
   {
      private class TypeMetadata
      {
         public static readonly TypeMetadata Empty = new TypeMetadata();

         #region Constructors

         private TypeMetadata()
         {
            MemberAttributes = ImmutableDictionary<MemberKey, IImmutableList<Attribute>>.Empty;
            MethodAttributes = ImmutableDictionary<MethodKey, ImmutableMethodMetadata>.Empty;
            TypeAttributes = ImmutableList<Attribute>.Empty;
         }

         private TypeMetadata(IImmutableList<Attribute> typeAttributes, IImmutableDictionary<MemberKey, IImmutableList<Attribute>> memberAttributes, IImmutableDictionary<MethodKey, ImmutableMethodMetadata> methodAttributes)
         {
            MemberAttributes = memberAttributes;
            MethodAttributes = methodAttributes;
            TypeAttributes = typeAttributes;
         }

         #endregion

         #region Properties

         public IImmutableDictionary<MemberKey, IImmutableList<Attribute>> MemberAttributes { get; }
         public IImmutableDictionary<MethodKey, ImmutableMethodMetadata> MethodAttributes { get; }
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

         public TypeMetadata AddMemberAttributes(MemberKey key, IEnumerable<Attribute> attributes)
         {
            IImmutableList<Attribute> list;
            if (!MemberAttributes.TryGetValue(key, out list))
            {
               list = ImmutableList.CreateRange<Attribute>(attributes);
            }
            else
            {
               list = list.AddRange(attributes);
            }

            return new TypeMetadata(TypeAttributes, MemberAttributes.SetItem(key, list), MethodAttributes);
         }

         private ImmutableMethodMetadata GetMethodMetadata(MethodKey key)
         {
            ImmutableMethodMetadata metadata;
            if (!MethodAttributes.TryGetValue(key, out metadata))
            {
               metadata = new ImmutableMethodMetadata(key.Parameters.Count);
            }

            return metadata;
         }

         #endregion
      }      
   }

   // TODO PP: Add a simplified builder that is specific to a type, eg. builder.ForType<MyType>().AddMemberAttributes(c => c.MyProperty);
   //                                                                                                                ^ Note: No generic argument here!

   // TODO PP: Change AddMemberAttribute to specific methods instead, it seems that it may be needed. Why!?
}
