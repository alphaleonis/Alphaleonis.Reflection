using System;
using System.Linq;

namespace CustomAttributeTableTests
{
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
