using System;
using System.Linq;

namespace Alphaleonis.Reflection.Context
{
   /// <summary>
   /// Internal attribute used to detect whether a member has been mapped using a particular
   /// reflection context.
   /// </summary>
   [AttributeUsage(AttributeTargets.All, AllowMultiple = true, Inherited = false)]
   public class CustomReflectionContextIdAttribute : Attribute
   {
      public CustomReflectionContextIdAttribute(Guid contextId)
      {
         ContextId = contextId;
      }

      public Guid ContextId { get; }
   }
}
