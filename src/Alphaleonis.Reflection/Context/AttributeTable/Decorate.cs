// Copyright (c) Peter Palotas
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphaleonis.Reflection.Context
{
   public static class Decorate
   {
      public static T Parameter<T>(IEnumerable<Attribute> attributes) => default(T);

      public static T Parameter<T>(params Attribute[] attributes) => default(T);
   }
}
