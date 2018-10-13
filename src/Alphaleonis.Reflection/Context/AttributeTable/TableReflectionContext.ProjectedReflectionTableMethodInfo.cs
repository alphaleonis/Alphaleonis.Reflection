// Copyright (c) Peter Palotas
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Alphaleonis.Reflection.Context
{
   public partial class TableReflectionContext
   {
      /// <summary>Projector used for <see cref="MethodInfo"/> instances in this reflection context.</summary>
      protected class ProjectedReflectionTableMethodInfo : ProjectedMethodInfo<TableReflectionContext>
      {
         /// <summary>Constructor.</summary>
         /// <param name="method">The method to wrap.</param>
         /// <param name="reflectionContext">The parent reflection context.</param>
         public ProjectedReflectionTableMethodInfo(MethodInfo method, TableReflectionContext reflectionContext) 
            : base(method, reflectionContext)
         {
         }

         /// <inheritdoc />
         public override object[] GetCustomAttributes(bool inherit) => GetCustomAttributes(typeof(Attribute), inherit);

         /// <inheritdoc />
         public override object[] GetCustomAttributes(Type attributeType, bool inherit)
         {
            List<object> result = new List<object>();

            // Then add any attributes defined in the reflection context table.
            result.AddRange(ReflectionContext.Table.GetCustomAttributes(this).Where(attr => attributeType.IsAssignableFrom(attr.GetType())));

            // Then check this method, without inheritance. Add only attributes if Multiple = true OR attribute not already exists.
            foreach (var ca in base.GetCustomAttributes(attributeType, false))
            {
               if (AttributeUtil.GetAttributeUsage(ca.GetType()).AllowMultiple || !result.Any(attr => attr.GetType().Equals(ca.GetType())))
                  result.Add(ca);
            }

            // If we want also inherited attributes, we must check the base definition.
            if (inherit)
            {
               // ...if it is different from the declaring type of this method, we get all attributes from there and add them as well, depending
               // on their attribute usage settings.
               MethodInfo baseMethod = GetBaseDefinition();

               // Then get base attributes, add only if Inherit = true AND (Multiple = true OR attribute not already exists).
               if (baseMethod != null && !baseMethod.DeclaringType.Equals(DeclaringType))
               {
                  foreach (var ca in baseMethod.GetCustomAttributes(attributeType, true))
                  {
                     AttributeUsageAttribute attributeUsage = AttributeUtil.GetAttributeUsage(ca.GetType());
                     if (attributeUsage.Inherited && (attributeUsage.AllowMultiple || !result.Any(attr => attr.GetType().Equals(ca.GetType()))))
                        result.Add(ca);
                  }
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
            // Then add any attributes defined in the reflection context table.
            if (ReflectionContext.Table.GetCustomAttributes(this).Any(attr => attributeType.IsAssignableFrom(attr.GetType())))
               return true;

            if (base.IsDefined(attributeType, false))
               return true;

            if (inherit)
            {
               return this.GetParentDefinition()?.IsDefined(attributeType, true) == true;
            }

            return false;
         }
      }
   }




}
