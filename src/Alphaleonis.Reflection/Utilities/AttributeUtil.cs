// Copyright (c) Peter Palotas
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Linq;
using System.Reflection;

namespace Alphaleonis.Reflection
{
   internal static class AttributeUtil
   {
      private readonly static AttributeUsageAttribute DefaultAttributeUsageAttribute = new AttributeUsageAttribute(AttributeTargets.All);

      public static AttributeUsageAttribute GetAttributeUsage(ICustomAttributeProvider decoratedAttribute)
      {
         return decoratedAttribute.GetCustomAttributes(typeof(AttributeUsageAttribute), true).OfType<AttributeUsageAttribute>().FirstOrDefault() ?? DefaultAttributeUsageAttribute;
      }
   }
}
