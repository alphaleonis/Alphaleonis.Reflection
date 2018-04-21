using System;
using System.Linq;

namespace Alphaleonis.Reflection
{
   /// <summary>Attribute for attribute table reflection context identifier.</summary>
   [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
   public class AttributeTableReflectionContextIdentifierAttribute : Attribute
   {
      public AttributeTableReflectionContextIdentifierAttribute(Guid contextId)
      {
         ContextId = contextId;
      }

      public Guid ContextId { get; }
   }
}
