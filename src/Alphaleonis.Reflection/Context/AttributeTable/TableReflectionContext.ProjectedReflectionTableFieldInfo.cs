// Copyright (c) Peter Palotas
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Alphaleonis.Reflection.Context;

namespace Alphaleonis.Reflection.Context
{
   public partial class TableReflectionContext
   {
      /// <summary>Projector used for <see cref="FieldInfo"/> instances in this reflection context.</summary>
      protected class ProjectedReflectionTableFieldInfo : ProjectedFieldInfo<TableReflectionContext>
      {
         /// <summary>Constructor.</summary>
         /// <param name="field">The field to wrap.</param>
         /// <param name="reflectionContext">The parent reflection context.</param>
         public ProjectedReflectionTableFieldInfo(FieldInfo field, TableReflectionContext reflectionContext) 
            : base(field, reflectionContext)
         {
         }

         /// <inheritdoc />
         public override object[] GetCustomAttributes(Type attributeType, bool inherit)
         {
            List<object> result = new List<object>();

            // Then add all the attributes from the attribute table.
            result.AddRange(ReflectionContext.Table.GetCustomAttributes(this).Where(attr => attributeType.IsAssignableFrom(attr.GetType())));

            // Then check this type, without inheritance. (Field attributes are not inherited). Add only attributes if Multiple = true OR attribute not already exists.
            foreach (var ca in base.GetCustomAttributes(attributeType, false))
            {
               if (AttributeUtil.GetAttributeUsage(ca.GetType()).AllowMultiple || !result.Any(attr => attr.GetType().Equals(ca.GetType())))
                  result.Add(ca);
            }

            // Finally create a resulting array of the correct type.
            object[] arrResult = (object[])Array.CreateInstance(attributeType, result.Count);
            for (int i = 0; i < result.Count; i++)
            {
               arrResult[i] = result[i];
            }

            return arrResult;
         }

         /// <inheritdoc />
         public override bool IsDefined(Type attributeType, bool inherit)
         {
            return GetCustomAttributes(attributeType, inherit).Length > 0;
         }
      }
   }




}
