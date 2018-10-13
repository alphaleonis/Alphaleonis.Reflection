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
   ///       <see cref="Assembly"/> is exposed to deriving classes by the
   ///       <see cref="UnderlyingAssembly"/> property.</para>
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

      /// <inheritdoc/>
      public override string CodeBase => m_assemblyImpl.CodeBase;

      /// <inheritdoc/>
      public override MethodInfo EntryPoint => m_assemblyImpl.EntryPoint;

      /// <inheritdoc/>
      public override string EscapedCodeBase => m_assemblyImpl.EscapedCodeBase;

#if NETFX
      /// <inheritdoc/>
      public override Evidence Evidence => m_assemblyImpl.Evidence;
#endif

      /// <inheritdoc/>
      public override string FullName => m_assemblyImpl.FullName;

      /// <inheritdoc/>
      public override bool GlobalAssemblyCache => m_assemblyImpl.GlobalAssemblyCache;

      /// <inheritdoc/>
      public override long HostContext => m_assemblyImpl.HostContext;

      /// <inheritdoc/>
      public override string ImageRuntimeVersion => m_assemblyImpl.ImageRuntimeVersion;

      /// <inheritdoc/>
      public override bool IsDynamic => m_assemblyImpl.IsDynamic;

      /// <inheritdoc/>
      public override string Location => m_assemblyImpl.Location;

      /// <inheritdoc/>
      public override Module ManifestModule => m_assemblyImpl.ManifestModule;

      /// <inheritdoc/>
      public override bool ReflectionOnly => m_assemblyImpl.ReflectionOnly;

      /// <inheritdoc/>
      public override SecurityRuleSet SecurityRuleSet => m_assemblyImpl.SecurityRuleSet;

      /// <summary>The underlying assembly passed to the constructor.</summary>
      public Assembly UnderlyingAssembly => m_assemblyImpl;

      /// <inheritdoc/>
      public override object CreateInstance(string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes) => m_assemblyImpl.CreateInstance(typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes);

      /// <inheritdoc/>
      public override object[] GetCustomAttributes(bool inherit) => m_assemblyImpl.GetCustomAttributes(inherit);

      /// <inheritdoc/>
      public override object[] GetCustomAttributes(Type attributeType, bool inherit) => m_assemblyImpl.GetCustomAttributes(attributeType, inherit);

      /// <inheritdoc/>
      public override IList<CustomAttributeData> GetCustomAttributesData() => m_assemblyImpl.GetCustomAttributesData();

      /// <inheritdoc/>
      public override Type[] GetExportedTypes() => m_assemblyImpl.GetExportedTypes();

      /// <inheritdoc/>
      public override FileStream GetFile(string name) => m_assemblyImpl.GetFile(name);

      /// <inheritdoc/>
      public override FileStream[] GetFiles() => m_assemblyImpl.GetFiles();

      /// <inheritdoc/>
      public override FileStream[] GetFiles(bool getResourceModules) => m_assemblyImpl.GetFiles(getResourceModules);

      /// <inheritdoc/>
      public override Module[] GetLoadedModules(bool getResourceModules) => m_assemblyImpl.GetLoadedModules(getResourceModules);

      /// <inheritdoc/>
      public override ManifestResourceInfo GetManifestResourceInfo(string resourceName) => m_assemblyImpl.GetManifestResourceInfo(resourceName);

      /// <inheritdoc/>
      public override string[] GetManifestResourceNames() => m_assemblyImpl.GetManifestResourceNames();

      /// <inheritdoc/>
      public override Stream GetManifestResourceStream(string name) => m_assemblyImpl.GetManifestResourceStream(name);

      /// <inheritdoc/>
      public override Stream GetManifestResourceStream(Type type, string name) => m_assemblyImpl.GetManifestResourceStream(type, name);

      /// <inheritdoc/>
      public override Module GetModule(string name) => m_assemblyImpl.GetModule(name);

      /// <inheritdoc/>
      public override Module[] GetModules(bool getResourceModules) => m_assemblyImpl.GetModules(getResourceModules);

      /// <inheritdoc/>
      public override AssemblyName GetName() => m_assemblyImpl.GetName();

      /// <inheritdoc/>
      public override AssemblyName GetName(bool copiedName) => m_assemblyImpl.GetName(copiedName);

      /// <inheritdoc/>
      public override AssemblyName[] GetReferencedAssemblies() => m_assemblyImpl.GetReferencedAssemblies();

      /// <inheritdoc/>
      public override Assembly GetSatelliteAssembly(CultureInfo culture) => m_assemblyImpl.GetSatelliteAssembly(culture);

      /// <inheritdoc/>
      public override Assembly GetSatelliteAssembly(CultureInfo culture, Version version) => m_assemblyImpl.GetSatelliteAssembly(culture, version);

      /// <inheritdoc/>
      public override Type GetType(string name, bool throwOnError, bool ignoreCase) => m_assemblyImpl.GetType(name, throwOnError, ignoreCase);

      /// <inheritdoc/>
      public override Type[] GetTypes() => m_assemblyImpl.GetTypes();

      /// <inheritdoc/>
      public override bool IsDefined(Type attributeType, bool inherit) => m_assemblyImpl.IsDefined(attributeType, inherit);

      /// <inheritdoc/>
      public override Module LoadModule(string moduleName, byte[] rawModule, byte[] rawSymbolStore) => m_assemblyImpl.LoadModule(moduleName, rawModule, rawSymbolStore);

      /// <inheritdoc/>
      public override string ToString() => m_assemblyImpl.ToString();

      /// <inheritdoc/>
      public override IEnumerable<CustomAttributeData> CustomAttributes => m_assemblyImpl.CustomAttributes;

      /// <inheritdoc/>
      public override IEnumerable<TypeInfo> DefinedTypes => m_assemblyImpl.DefinedTypes;

      /// <inheritdoc/>
      public override bool Equals(object o)
      {
         DelegatingAssembly other = o as DelegatingAssembly;
         if (other != null)
            return m_assemblyImpl.Equals(other.m_assemblyImpl);
         else
            return m_assemblyImpl.Equals(o);
      }

      /// <inheritdoc/>
      public override IEnumerable<Type> ExportedTypes => m_assemblyImpl.ExportedTypes;

      /// <inheritdoc/>
      public override int GetHashCode() => m_assemblyImpl.GetHashCode();

      /// <inheritdoc/>
      public override void GetObjectData(SerializationInfo info, StreamingContext context) => m_assemblyImpl.GetObjectData(info, context);

      /// <inheritdoc/>
      public override Type GetType(string name) => m_assemblyImpl.GetType(name);

      /// <inheritdoc/>
      public override Type GetType(string name, bool throwOnError) => m_assemblyImpl.GetType(name, throwOnError);

      /// <inheritdoc/>
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

      /// <inheritdoc/>
      public override IEnumerable<Module> Modules => m_assemblyImpl.Modules;

#if NETFX
      /// <inheritdoc/>
      public override PermissionSet PermissionSet => m_assemblyImpl.PermissionSet;
#endif

   }




}
