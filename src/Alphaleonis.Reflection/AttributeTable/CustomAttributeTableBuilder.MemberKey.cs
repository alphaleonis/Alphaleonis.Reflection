using System;
using System.Reflection;

namespace Alphaleonis.Reflection
{
   public partial class CustomAttributeTableBuilder
   {
      private struct MemberKey : IEquatable<MemberKey>
      {
         public MemberKey(MemberInfo member)
            : this(member.MemberType, member.Name)
         {
         }

         public MemberKey(MemberTypes memberType, string name)
         {
            MemberType = memberType;
            Name = name;
         }

         public MemberTypes MemberType { get; }
         public string Name { get; }

         public bool Equals(MemberKey other)
         {
            return MemberType.Equals(other.MemberType) && StringComparer.Ordinal.Equals(Name, other.Name);
         }

         public override bool Equals(object obj)
         {
            if (obj is MemberKey other)
               return Equals(other);

            return false;
         }

         public override int GetHashCode()
         {
            return MemberType.GetHashCode() + 37 * StringComparer.Ordinal.GetHashCode(Name);
         }

         public static bool operator ==(MemberKey self, MemberKey other)
         {
            return self.Equals(other);
         }

         public static bool operator !=(MemberKey self, MemberKey other)
         {
            return !self.Equals(other);
         }
      }
   }
}
