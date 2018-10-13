// Copyright (c) Peter Palotas
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

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
