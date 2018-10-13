// Copyright (c) Peter Palotas
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

#if !NETSTANDARD1_6 && !NETSTANDARD2_0
#define NETFX
#endif

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security;
#if NETFX
using System.Security.Policy;
#endif

namespace Alphaleonis.Reflection
{
   /// <summary>
   /// Wraps a <see cref="Assembly"/> instance and delegates all method calls to that
   /// <see cref="Assembly" />.
   /// </summary>
   /// <remarks>
   /// <para>Derive from this type and override only those members you have to provide customization
   /// in.</para>
   /// <para><see cref="DelegatingAssembly"/> derives from <see cref="Assembly"/> and implements most
   /// of the properties and methods of <see cref="Assembly"/>. For
   ///       each member it implements, <see cref="DelegatingAssembly"/> automatically delegates to
   ///       the corresponding member of the internal <see cref="Assembly"/>
   ///       object which was supplied as the argument to the constructor.  This internal
   ///       <see cref="Assembly"/> is exposed to deriving classes by the <see langword="protected" />
   ///       <see cref="m_assemblyImpl"/> field.</para>
   /// </remarks>
   public class DelegatingAssembly : Assembly
   {
      /// <summary>The wrapped assembly.</summary>
      protected readonly Assembly m_assemblyImpl;

      /// <summary>Constructor.</summary>
      /// <param name="assembly">The assembly to wrap.</param>
      public DelegatingAssembly(Assembly assembly)
      {
         m_assemblyImpl = assembly;
      }
      
      public override string CodeBase => m_assemblyImpl.CodeBase;

      public override MethodInfo EntryPoint => m_assemblyImpl.EntryPoint;

      public override string EscapedCodeBase => m_assemblyImpl.EscapedCodeBase;

#if NETFX
      public override Evidence Evidence => m_assemblyImpl.Evidence;
#endif

      public override string FullName => m_assemblyImpl.FullName;

      public override bool GlobalAssemblyCache => m_assemblyImpl.GlobalAssemblyCache;

      public override long HostContext => m_assemblyImpl.HostContext;

      public override string ImageRuntimeVersion => m_assemblyImpl.ImageRuntimeVersion;

      public override bool IsDynamic => m_assemblyImpl.IsDynamic;

      public override string Location => m_assemblyImpl.Location;

      public override Module ManifestModule => m_assemblyImpl.ManifestModule;

      public override bool ReflectionOnly => m_assemblyImpl.ReflectionOnly;

      public override SecurityRuleSet SecurityRuleSet => m_assemblyImpl.SecurityRuleSet;

      public Assembly UnderlyingAssembly => m_assemblyImpl;

      public override object CreateInstance(string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes) => m_assemblyImpl.CreateInstance(typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes);

      public override object[] GetCustomAttributes(bool inherit) => m_assemblyImpl.GetCustomAttributes(inherit);

      public override object[] GetCustomAttributes(Type attributeType, bool inherit) => m_assemblyImpl.GetCustomAttributes(attributeType, inherit);

      public override IList<CustomAttributeData> GetCustomAttributesData() => m_assemblyImpl.GetCustomAttributesData();

      public override Type[] GetExportedTypes() => m_assemblyImpl.GetExportedTypes();

      public override FileStream GetFile(string name) => m_assemblyImpl.GetFile(name);

      public override FileStream[] GetFiles() => m_assemblyImpl.GetFiles();

      public override FileStream[] GetFiles(bool getResourceModules) => m_assemblyImpl.GetFiles(getResourceModules);

      public override Module[] GetLoadedModules(bool getResourceModules) => m_assemblyImpl.GetLoadedModules(getResourceModules);

      public override ManifestResourceInfo GetManifestResourceInfo(string resourceName) => m_assemblyImpl.GetManifestResourceInfo(resourceName);

      public override string[] GetManifestResourceNames() => m_assemblyImpl.GetManifestResourceNames();

      public override Stream GetManifestResourceStream(string name) => m_assemblyImpl.GetManifestResourceStream(name);

      public override Stream GetManifestResourceStream(Type type, string name) => m_assemblyImpl.GetManifestResourceStream(type, name);

      public override Module GetModule(string name) => m_assemblyImpl.GetModule(name);

      public override Module[] GetModules(bool getResourceModules) => m_assemblyImpl.GetModules(getResourceModules);

      public override AssemblyName GetName() => m_assemblyImpl.GetName();

      public override AssemblyName GetName(bool copiedName) => m_assemblyImpl.GetName(copiedName);

      public override AssemblyName[] GetReferencedAssemblies() => m_assemblyImpl.GetReferencedAssemblies();

      public override Assembly GetSatelliteAssembly(CultureInfo culture) => m_assemblyImpl.GetSatelliteAssembly(culture);

      public override Assembly GetSatelliteAssembly(CultureInfo culture, Version version) => m_assemblyImpl.GetSatelliteAssembly(culture, version);

      public override Type GetType(string name, bool throwOnError, bool ignoreCase) => m_assemblyImpl.GetType(name, throwOnError, ignoreCase);

      public override Type[] GetTypes() => m_assemblyImpl.GetTypes();

      public override bool IsDefined(Type attributeType, bool inherit) => m_assemblyImpl.IsDefined(attributeType, inherit);

      public override Module LoadModule(string moduleName, byte[] rawModule, byte[] rawSymbolStore) => m_assemblyImpl.LoadModule(moduleName, rawModule, rawSymbolStore);

      public override string ToString() => m_assemblyImpl.ToString();

      public override IEnumerable<CustomAttributeData> CustomAttributes => m_assemblyImpl.CustomAttributes;

      public override IEnumerable<TypeInfo> DefinedTypes => m_assemblyImpl.DefinedTypes;

      public override bool Equals(object o)
      {
         DelegatingAssembly other = o as DelegatingAssembly;
         if (other != null)
            return m_assemblyImpl.Equals(other.m_assemblyImpl);
         else
            return m_assemblyImpl.Equals(o);
      }

      public override IEnumerable<Type> ExportedTypes => m_assemblyImpl.ExportedTypes;

      public override int GetHashCode() => m_assemblyImpl.GetHashCode();

      public override void GetObjectData(SerializationInfo info, StreamingContext context) => m_assemblyImpl.GetObjectData(info, context);

      public override Type GetType(string name) => m_assemblyImpl.GetType(name);

      public override Type GetType(string name, bool throwOnError) => m_assemblyImpl.GetType(name, throwOnError);

      public override event ModuleResolveEventHandler ModuleResolve
      {
         add
         {
            m_assemblyImpl.ModuleResolve += value;
         }

         remove
         {
            m_assemblyImpl.ModuleResolve -= value;
         }
      }

      public override IEnumerable<Module> Modules => m_assemblyImpl.Modules;

#if NETFX
      public override PermissionSet PermissionSet => m_assemblyImpl.PermissionSet;
#endif

   }




}
