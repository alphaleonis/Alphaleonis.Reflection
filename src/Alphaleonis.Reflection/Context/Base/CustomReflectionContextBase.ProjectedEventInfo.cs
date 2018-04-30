using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Alphaleonis.Reflection.Context
{
   public partial class CustomReflectionContextBase
   {
      protected class ProjectedEventInfo<TContext> : DelegatingEventInfo, IProjector<TContext> where TContext: CustomReflectionContextBase
      {
         public ProjectedEventInfo(EventInfo eventInfo, TContext reflectionContext) 
            : base(eventInfo)
         {
            ReflectionContext = reflectionContext;
         }

         public TContext ReflectionContext { get; }

         CustomReflectionContextBase IProjector.ReflectionContext => ReflectionContext;

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
            return ReflectionContext.AddContextIdentifierAttributeIfRequested(attributeType, base.GetCustomAttributes(attributeType, inherit));
         }

         public override bool IsDefined(Type attributeType, bool inherit)
         {
            if (attributeType.IsAssignableFrom(typeof(CustomReflectionContextIdAttribute)))
               return true;

            return base.IsDefined(attributeType, inherit);
         }

         public override MethodInfo RaiseMethod => ReflectionContext.MapMember(base.RaiseMethod);

         public override Type ReflectedType => ReflectionContext.MapType(base.ReflectedType);

         public override MethodInfo RemoveMethod => ReflectionContext.MapMember(base.RemoveMethod);         
      }      
   }
}
