// Copyright (c) Peter Palotas
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Reflection;

namespace Alphaleonis.Reflection.Context
{
   public partial class TableReflectionContext
   {
      /// <summary>Projector used for <see cref="Assembly"/> instances in this reflection context.</summary>
      /// <remarks>Adding attributes on an assembly level is currently not supported.</remarks>
      protected class ProjectedReflectionTableAssembly : ProjectedAssembly<TableReflectionContext>
      {
         /// <summary>Constructor.</summary>
         /// <param name="assembly">The assembly to wrap.</param>
         /// <param name="context">The parent reflection context.</param>
         public ProjectedReflectionTableAssembly(Assembly assembly, TableReflectionContext context) 
            : base(assembly, context)
         {
         }
      }
   }
}
