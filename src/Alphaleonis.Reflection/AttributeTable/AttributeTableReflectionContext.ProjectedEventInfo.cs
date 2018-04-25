using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Alphaleonis.Reflection
{
   public partial class AttributeTableReflectionContext
   {
      private class ProjectedEventInfo : DelegatingEventInfo, IAttributeTableProjector
      {
         public ProjectedEventInfo(EventInfo eventInfo, AttributeTableReflectionContext reflectionContext) 
            : base(eventInfo)
         {
            ReflectionContext = reflectionContext;
         }

         public AttributeTableReflectionContext ReflectionContext { get; }

         public override MethodInfo AddMethod => ReflectionContext.MapMember(base.AddMethod);

         public override Type DeclaringType => ReflectionContext.MapType(base.DeclaringType);

         public override Type EventHandlerType => ReflectionContext.MapType(base.EventHandlerType);

         public override MethodInfo GetAddMethod(bool nonPublic) => ReflectionContext.MapMember(base.GetAddMethod(nonPublic));

         public override MethodInfo[] GetOtherMethods(bool nonPublic) => ReflectionContext.MapMembers(base.GetOtherMethods(nonPublic));

         public override MethodInfo GetRaiseMethod(bool nonPublic) => ReflectionContext.MapMember(base.GetRaiseMethod(nonPublic));

         public override MethodInfo GetRemoveMethod(bool nonPublic) => ReflectionContext.MapMember(base.GetRemoveMethod(nonPublic));

         public override object[] GetCustomAttributes(bool inherit)
         {
            return GetCustomAttributes(typeof(Attribute), inherit);
         }

         public override object[] GetCustomAttributes(Type attributeType, bool inherit)
         {
            List<object> result = new List<object>();

            // Add the reflection context identifier attribute.
            result.Add(ReflectionContext.ContextIdentifierAttribute);

            // Then add any attributes defined in the reflection context table.
            result.AddRange(ReflectionContext.Table.GetCustomAttributes(this).Where(attr => attributeType.IsAssignableFrom(attr.GetType())));

            // Then check this method, without inheritance. Add only attributes if Multiple = true OR attribute not already exists.
            foreach (var ca in base.GetCustomAttributes(attributeType, false))
            {
               if (GetAttributeUsage(ca.GetType()).AllowMultiple || !result.Any(attr => attr.GetType().Equals(ca.GetType())))
                  result.Add(ca);
            }

            // If we want also inherited attributes, we must check the base definition.
            if (inherit && (ReflectionContext.Options & AttributeTableReflectionContextOptions.HonorEventAttributeInheritance) != 0)
            {
               // ...if it is different from the declaring type of this method, we get all attributes from there and add them as well, depending
               // on their attribute usage settings.
               MemberInfo baseEvent = this.GetParentDefinition();

               // Then get base attributes, add only if Inherit = true AND (Multiple = true OR attribute not already exists).
               if (baseEvent != null && !baseEvent.DeclaringType.Equals(DeclaringType))
               {
                  foreach (var ca in baseEvent.GetCustomAttributes(attributeType, true))
                  {
                     AttributeUsageAttribute attributeUsage = GetAttributeUsage(ca.GetType());
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

         public override bool IsDefined(Type attributeType, bool inherit)
         {
            // Then add any attributes defined in the reflection context table.
            if (ReflectionContext.Table.GetCustomAttributes(this).Any(attr => attributeType.IsAssignableFrom(attr.GetType())))
               return true;

            if (base.IsDefined(attributeType, false))
               return true;

            if (inherit && (ReflectionContext.Options & AttributeTableReflectionContextOptions.HonorEventAttributeInheritance) != 0)
            {
               return this.GetParentDefinition()?.IsDefined(attributeType, true) == true;
            }

            return false;
         }

         public override MethodInfo RaiseMethod => ReflectionContext.MapMember(base.RaiseMethod);

         public override Type ReflectedType => ReflectionContext.MapType(base.ReflectedType);

         public override MethodInfo RemoveMethod => ReflectionContext.MapMember(base.RemoveMethod);         
      }      
   }
}
