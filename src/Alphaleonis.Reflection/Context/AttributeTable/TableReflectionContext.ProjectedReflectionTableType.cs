// Copyright (c) Peter Palotas
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Alphaleonis.Reflection.Context
{
   public partial class TableReflectionContext
   {
      /// <summary>Projector used for <see cref="Type"/> instances in this reflection context.</summary>
      protected class ProjectedReflectionTableType : ProjectedType<TableReflectionContext>
      {
         /// <summary>Constructor.</summary>
         /// <param name="type">The type to wrap.</param>
         /// <param name="reflectionContext">The parent reflection context.</param>
         public ProjectedReflectionTableType(Type type, TableReflectionContext reflectionContext) 
            : base(type, reflectionContext)
         {
         }

         /// <inheritdoc />
         public override object[] GetCustomAttributes(bool inherit)
         {
            return GetCustomAttributes(typeof(Attribute), inherit);
         }

         /// <inheritdoc />
         public override object[] GetCustomAttributes(Type attributeType, bool inherit)
         {
            bool stop = this.IsGenericType;
            // Start with the table attributes.
            List<object> result = new List<object>();

            result.AddRange(ReflectionContext.Table.GetCustomAttributes(this).Where(attr => attributeType.IsAssignableFrom(attr.GetType())));

            // Then check this type, without inheritance. Add only attributes if Multiple = true OR attribute not already exists.
            foreach (var ca in base.GetCustomAttributes(attributeType, false))
            {
               if (AttributeUtil.GetAttributeUsage(ca.GetType()).AllowMultiple || !result.Any(attr => attr.GetType().Equals(ca.GetType())))
                  result.Add(ca);
            }

            // Then get base attributes, add only if Inherit = true AND (Multiple = true OR attribute not already exists).
            if (inherit && BaseType != null && !BaseType.Equals(typeof(object)))
            {
               foreach (var ca in BaseType.GetCustomAttributes(attributeType, inherit))
               {
                  AttributeUsageAttribute attributeUsage = AttributeUtil.GetAttributeUsage(ca.GetType());
                  if (attributeUsage.Inherited && (attributeUsage.AllowMultiple || !result.Any(attr => attr.GetType().Equals(ca.GetType()))))
                     result.Add(ca);
               }
            }

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
