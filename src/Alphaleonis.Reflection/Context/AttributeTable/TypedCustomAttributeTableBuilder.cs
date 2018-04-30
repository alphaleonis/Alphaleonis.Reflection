namespace Alphaleonis.Reflection.Context
{
   public static partial class ReflectionTableBuilderExtensions
   {
      private class TypedReflectionTableBuilder<T> : ITypedReflectionTableBuilder<T>
      {
         private readonly IReflectionTableBuilder m_builder;

         public TypedReflectionTableBuilder(IReflectionTableBuilder builder)
         {
            m_builder = builder;
         }

         public IReflectionTableBuilder Builder => m_builder;
      }
   }
}
