using System;
using System.Linq;

namespace Alphaleonis.Reflection.Context
{
   /// <summary>Represents an <see cref="IReflectionTableBuilder"/> that is associated with a specific type, used by various extension methods..</summary>
   /// <typeparam name="T">Generic type parameter.</typeparam>
   public interface ITypedReflectionTableBuilder<T>
   {
      /// <summary>Gets the builder associated with this instance.</summary>
      /// <value>The builder.</value>
      IReflectionTableBuilder Builder { get; }
   }
}
