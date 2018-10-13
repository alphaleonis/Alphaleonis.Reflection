// Copyright (c) Peter Palotas
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Linq;
using System.Reflection;

namespace Alphaleonis.Reflection.Context
{
   /// <summary>A bit-field of flags for specifying various options for a <see cref="TableReflectionContext"/>.</summary>
   [Flags]
   public enum TableReflectionContextOptions
   {
      /// <summary>Use default options.</summary>
      Default = 0,

      /// <summary>
      /// If specified, attribute inheritance will be respected with regards to properties. 
      /// </summary>
      /// <remarks>
      /// This is different from how normal reflection works. Calling for example <see cref="MemberInfo.GetCustomAttributes(bool)"/> on a property specifying <see langword="true"/> for the `inherit` flag, inheritance will still be ignored.
      /// </remarks>
      HonorPropertyAttributeInheritance = 1,

      /// <summary>
      /// If specified, attribute inheritance will be respected with regards to events. 
      /// </summary>
      /// <remarks>
      /// This is different from how normal reflection works. Calling for example <see cref="MemberInfo.GetCustomAttributes(bool)"/> on an <see cref="EventInfo"/> 
      /// instance specifying <see langword="true"/> for the `inherit` flag, inheritance will still be ignored.
      /// </remarks>
      HonorEventAttributeInheritance = 2
   }
}
