// Copyright (c) Peter Palotas
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;

namespace Alphaleonis.Reflection.Context
{
   public sealed partial class ReflectionTableBuilder : ReflectionTableBase
   {
      private class ReflectionTable : ReflectionTableBase, IReflectionTable
      {
         private readonly IImmutableDictionary<Type, TypeMetadata> m_metadata;

         public ReflectionTable(IImmutableDictionary<Type, TypeMetadata> metadata)
         {
            m_metadata = metadata;
         }

         private protected override TypeMetadata GetTypeMetadata(Type type)
         {
            TypeMetadata metadata;
            if (m_metadata.TryGetValue(type, out metadata))
            {
               return metadata;
            }

            return TypeMetadata.Empty;
         }

      }      
   }


}
