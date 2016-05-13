using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Policy;

namespace CustomAttributeTable
{
   public class DelegatingAssembly : Assembly
   {
      private readonly Assembly m_assembly;

      public DelegatingAssembly(Assembly assembly)
      {
         m_assembly = assembly;
      }

      public override string CodeBase => m_assembly.CodeBase;

      public override MethodInfo EntryPoint => m_assembly.EntryPoint;

      public override string EscapedCodeBase => m_assembly.EscapedCodeBase;

      public override Evidence Evidence => m_assembly.Evidence;

      public override string FullName => m_assembly.FullName;

      public override bool GlobalAssemblyCache => m_assembly.GlobalAssemblyCache;

      public override long HostContext => m_assembly.HostContext;

      public override string ImageRuntimeVersion => m_assembly.ImageRuntimeVersion;

      public override bool IsDynamic => m_assembly.IsDynamic;

      public override string Location => m_assembly.Location;

      public override Module ManifestModule => m_assembly.ManifestModule;

      public override bool ReflectionOnly => m_assembly.ReflectionOnly;

      public override SecurityRuleSet SecurityRuleSet => m_assembly.SecurityRuleSet;

      public Assembly UnderlyingAssembly => m_assembly;

      public override object CreateInstance(string typeName, bool ignoreCase, BindingFlags bindingAttr, Binder binder, object[] args, CultureInfo culture, object[] activationAttributes) => m_assembly.CreateInstance(typeName, ignoreCase, bindingAttr, binder, args, culture, activationAttributes);

      public override object[] GetCustomAttributes(bool inherit) => m_assembly.GetCustomAttributes(inherit);

      public override object[] GetCustomAttributes(Type attributeType, bool inherit) => m_assembly.GetCustomAttributes(attributeType, inherit);

      public override IList<CustomAttributeData> GetCustomAttributesData() => m_assembly.GetCustomAttributesData();

      public override Type[] GetExportedTypes() => m_assembly.GetExportedTypes();

      public override FileStream GetFile(string name) => m_assembly.GetFile(name);

      public override FileStream[] GetFiles() => m_assembly.GetFiles();

      public override FileStream[] GetFiles(bool getResourceModules) => m_assembly.GetFiles(getResourceModules);

      public override Module[] GetLoadedModules(bool getResourceModules) => m_assembly.GetLoadedModules(getResourceModules);

      public override ManifestResourceInfo GetManifestResourceInfo(string resourceName) => m_assembly.GetManifestResourceInfo(resourceName);

      public override string[] GetManifestResourceNames() => m_assembly.GetManifestResourceNames();

      public override Stream GetManifestResourceStream(string name) => m_assembly.GetManifestResourceStream(name);

      public override Stream GetManifestResourceStream(Type type, string name) => m_assembly.GetManifestResourceStream(type, name);

      public override Module GetModule(string name) => m_assembly.GetModule(name);

      public override Module[] GetModules(bool getResourceModules) => m_assembly.GetModules(getResourceModules);

      public override AssemblyName GetName() => m_assembly.GetName();

      public override AssemblyName GetName(bool copiedName) => m_assembly.GetName(copiedName);

      public override AssemblyName[] GetReferencedAssemblies() => m_assembly.GetReferencedAssemblies();

      public override Assembly GetSatelliteAssembly(CultureInfo culture) => m_assembly.GetSatelliteAssembly(culture);

      public override Assembly GetSatelliteAssembly(CultureInfo culture, Version version) => m_assembly.GetSatelliteAssembly(culture, version);

      public override Type GetType(string name, bool throwOnError, bool ignoreCase) => m_assembly.GetType(name, throwOnError, ignoreCase);

      public override Type[] GetTypes() => m_assembly.GetTypes();

      public override bool IsDefined(Type attributeType, bool inherit) => m_assembly.IsDefined(attributeType, inherit);

      public override Module LoadModule(string moduleName, byte[] rawModule, byte[] rawSymbolStore) => m_assembly.LoadModule(moduleName, rawModule, rawSymbolStore);

      public override string ToString() => m_assembly.ToString();

      public override IEnumerable<CustomAttributeData> CustomAttributes => m_assembly.CustomAttributes;

      public override IEnumerable<TypeInfo> DefinedTypes => m_assembly.DefinedTypes;

      public override bool Equals(object o)
      {
         DelegatingAssembly other = o as DelegatingAssembly;
         if (other != null)
            return m_assembly.Equals(other.m_assembly);
         else
            return m_assembly.Equals(o);
      }

      public override IEnumerable<Type> ExportedTypes => m_assembly.ExportedTypes;

      public override int GetHashCode() => m_assembly.GetHashCode();

      public override void GetObjectData(SerializationInfo info, StreamingContext context) => m_assembly.GetObjectData(info, context);

      public override Type GetType(string name) => m_assembly.GetType(name);

      public override Type GetType(string name, bool throwOnError) => m_assembly.GetType(name, throwOnError);

      public override event ModuleResolveEventHandler ModuleResolve
      {
         add
         {
            m_assembly.ModuleResolve += value;
         }

         remove
         {
            m_assembly.ModuleResolve -= value;
         }
      }

      public override IEnumerable<Module> Modules => m_assembly.Modules;

      public override PermissionSet PermissionSet => m_assembly.PermissionSet;
   }




}
