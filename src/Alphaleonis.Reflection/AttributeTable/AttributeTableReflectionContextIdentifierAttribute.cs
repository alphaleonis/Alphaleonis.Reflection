using System;
using System.Linq;

namespace Alphaleonis.Reflection
{
   /// <summary>
   /// Internal attribute used to detect whether a member has been mapped using a particular
   /// reflection context.
   /// </summary>
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
