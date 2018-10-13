// Copyright (c) Peter Palotas
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;

namespace Alphaleonis.Reflection.Context
{
   public static partial class ReflectionTableBuilderExtensions
   {
      private class TypedReflectionTableBuilder<T> : ITypedReflectionTableBuilder<T>
      {
         private readonly IReflectionTableBuilder m_builder;

         public TypedReflectionTableBuilder(IReflectionTableBuilder builder)
         {
            m_builder = builder;
         }

         public IReflectionTableBuilder Builder => m_builder;
      }

      private class MappedTypedReflectionTableBuilder<T> : IMappedTypedReflectionTableBuilder<T>
      {
         private readonly IReflectionTableBuilder m_builder;
         private readonly Type m_concreteType;

         public MappedTypedReflectionTableBuilder(IReflectionTableBuilder builder, Type concreteType)
         {
            m_concreteType = concreteType;
            m_builder = builder;
         }

         public IReflectionTableBuilder Builder => m_builder;

         public Type TargetType => m_concreteType;
      }
   }
}
