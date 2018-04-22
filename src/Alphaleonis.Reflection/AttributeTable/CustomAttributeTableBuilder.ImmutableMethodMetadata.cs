using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Alphaleonis.Reflection
{
   public partial class CustomAttributeTableBuilder
   {
      private struct ImmutableMethodMetadata
      {
         public ImmutableMethodMetadata(int parameterCount)
         {
            ParameterAttributes = ImmutableList.CreateRange(Enumerable.Range(1, parameterCount).Select(p => (IImmutableList<Attribute>)ImmutableList<Attribute>.Empty)).ToImmutableList();
            ReturnParameterAttributes = ImmutableList<Attribute>.Empty;
            MethodAttributes = ImmutableList<Attribute>.Empty;
         }

         public ImmutableMethodMetadata(IImmutableList<IImmutableList<Attribute>> parameterAttributes, IImmutableList<Attribute> returnParameterAttributes, IImmutableList<Attribute> methodAttributes)
         {
            ParameterAttributes = parameterAttributes;
            ReturnParameterAttributes = returnParameterAttributes;
            MethodAttributes = methodAttributes;
         }

         public IImmutableList<IImmutableList<Attribute>> ParameterAttributes { get; }
         public IImmutableList<Attribute> ReturnParameterAttributes { get; }
         public IImmutableList<Attribute> MethodAttributes { get; }

         public ImmutableMethodMetadata AddParameterAttributes(int parameterIndex, IEnumerable<Attribute> attributes)
         {
            return new ImmutableMethodMetadata(ParameterAttributes.SetItem(parameterIndex, ParameterAttributes[parameterIndex].AddRange(attributes)), ReturnParameterAttributes, MethodAttributes);
         }

         public ImmutableMethodMetadata AddReturnParameterAttributes(IEnumerable<Attribute> attributes)
         {
            return new ImmutableMethodMetadata(ParameterAttributes, ReturnParameterAttributes.AddRange(attributes), MethodAttributes);
         }

         public ImmutableMethodMetadata AddMethodAttributes(IEnumerable<Attribute> methodAttributes)
         {
            return new ImmutableMethodMetadata(ParameterAttributes, ReturnParameterAttributes, MethodAttributes.AddRange(methodAttributes));
         }
      }
   }

   // TODO PP: Add a simplified builder that is specific to a type, eg. builder.ForType<MyType>().AddMemberAttributes(c => c.MyProperty);
   //                                                                                                                ^ Note: No generic argument here!

   // TODO PP: Change AddMemberAttribute to specific methods instead, it seems that it may be needed. Why!?
}
