﻿using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Alphaleonis.Reflection.Context
{
   public partial class CustomReflectionContextBase
   {
      protected class ProjectedFieldInfo<TContext> : DelegatingFieldInfo, IProjector<TContext> where TContext : CustomReflectionContextBase
      {
         public ProjectedFieldInfo(FieldInfo field, TContext reflectionContext)
            : base(field)
         {
            ReflectionContext = reflectionContext;
         }

         public TContext ReflectionContext { get; }

         CustomReflectionContextBase IProjector.ReflectionContext => ReflectionContext;

         public override Type FieldType => ReflectionContext.MapType(base.FieldType);

         public override Type DeclaringType => ReflectionContext.MapType(base.DeclaringType);

         public override object[] GetCustomAttributes(bool inherit)
         {
            return GetCustomAttributes(typeof(Attribute), inherit);
         }

         public override object[] GetCustomAttributes(Type attributeType, bool inherit)
         {
            return ReflectionContext.AddContextIdentifierAttributeIfRequested(attributeType, base.GetCustomAttributes(attributeType, inherit));
         }

         public override bool IsDefined(Type attributeType, bool inherit)
         {
            if (attributeType.IsAssignableFrom(typeof(CustomReflectionContextIdAttribute)))
               return true;

            return base.IsDefined(attributeType, inherit);
         }

         public override Type[] GetOptionalCustomModifiers() => ReflectionContext.MapTypes(base.GetOptionalCustomModifiers());

         public override Type[] GetRequiredCustomModifiers() => ReflectionContext.MapTypes(base.GetRequiredCustomModifiers());

         public override Type ReflectedType => ReflectionContext.MapType(base.ReflectedType);
      }
   }
}
