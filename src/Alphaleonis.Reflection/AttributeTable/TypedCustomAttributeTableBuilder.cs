namespace Alphaleonis.Reflection
{
   public static partial class CustomAttributeTableBuilderExtensions
   {
      private class TypedCustomAttributeTableBuilder<T> : ITypedCustomAttributeTableBuilder<T>
      {
         private readonly ICustomAttributeTableBuilder m_builder;

         public TypedCustomAttributeTableBuilder(ICustomAttributeTableBuilder builder)
         {
            m_builder = builder;
         }

         public ICustomAttributeTableBuilder Builder => m_builder;
      }
   }
}
