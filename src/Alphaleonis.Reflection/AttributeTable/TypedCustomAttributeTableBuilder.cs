namespace Alphaleonis.Reflection
{
   public static partial class AttributeTableBuilderExtensions
   {
      private class TypedAttributeTableBuilder<T> : ITypedAttributeTableBuilder<T>
      {
         private readonly IAttributeTableBuilder m_builder;

         public TypedAttributeTableBuilder(IAttributeTableBuilder builder)
         {
            m_builder = builder;
         }

         public IAttributeTableBuilder Builder => m_builder;
      }
   }
}
