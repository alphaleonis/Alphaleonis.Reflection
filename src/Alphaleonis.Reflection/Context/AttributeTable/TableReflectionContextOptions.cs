// Copyright (c) Peter Palotas
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Linq;

namespace Alphaleonis.Reflection.Context
{
   // TODO PP (2018-04-21): Document
   [Flags]
   public enum TableReflectionContextOptions
   {
      Default = 0,
      HonorPropertyAttributeInheritance = 1,
      HonorEventAttributeInheritance = 2
   }
}
