using System.Reflection;

namespace Alphaleonis.Reflection.Context
{
   public partial class TableReflectionContext
   {
      // TODO PP (2018-04-30): Add support for adding assembly attributes to the table perhaps?
      protected class ProjectedReflectionTableAssembly : ProjectedAssembly<TableReflectionContext>
      {
         public ProjectedReflectionTableAssembly(Assembly assembly, TableReflectionContext context) 
            : base(assembly, context)
         {
         }
      }
   }
}
