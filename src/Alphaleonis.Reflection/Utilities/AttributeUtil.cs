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
