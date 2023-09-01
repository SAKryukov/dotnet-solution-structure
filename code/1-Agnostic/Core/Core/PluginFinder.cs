namespace SA.Agnostic {
    using System;
    using System.Reflection;
    using Type = System.Type;
    using Debug = System.Diagnostics.Debug;

    public interface IRecognizable { } // all host-side plugin interfaces derive from this interface

    public abstract class PluginFinderBase {
        public virtual void Unload() { }
    } //PluginFinderBase

    public class PluginFinder<INTERFACE> : PluginFinderBase
        where INTERFACE : IRecognizable {

        static class DefinitionSet {
            internal static string FormatAssemblyException(string assemblyLocation, string pluginImplementorAttributeName, string reason) =>
                $@"Assembly ""{assemblyLocation}"", {pluginImplementorAttributeName} problem: {reason}";
            internal static string FormatNonInterfaceException(string typeName) =>
                $"implementor interface type {typeName} should be interface";
            internal static string FormatNullInterfaceException(string pluginImplementorAttributeName) =>
                $"the {pluginImplementorAttributeName} implementor interface type cannot be null";
            internal static string FormatNullImplementorException(string typeName) =>
                $"the implementor of the interface {typeName} cannot be null";
            internal static string FormatNonClassImplementorException(string typeName) =>
                $"the implementor type {typeName} should be a class";
            internal static string FormatNonImplementorException(string implementorTypeName, string interfaceTypeName) =>
                $"the type {implementorTypeName} should implement the interface {interfaceTypeName}";
        } //class

        public class PluginImplementationException : System.ApplicationException {
            internal PluginImplementationException(string message) : base(message) { }
        }

        private protected void Construct(Assembly assembly) {
            void ThrowImplementingAssemblyException(string reason) {
                var pluginImplementorAttributeName = typeof(PluginManifestAttribute).Name;
                throw new PluginImplementationException(DefinitionSet.FormatAssemblyException(assembly.Location, pluginImplementorAttributeName, reason));
            } //ThrowImplementingAssemblyException
            object[] attributes = assembly.GetCustomAttributes(typeof(PluginManifestAttribute), false);
            if (attributes == null) return;
            if (attributes.Length < 0) return;
            Type implementorType;
            foreach (var attribute in attributes) {
                var implementorAttribute = (PluginManifestAttribute)attribute;
                if (implementorAttribute == null)
                    ThrowImplementingAssemblyException(DefinitionSet.FormatNullInterfaceException(typeof(PluginManifestAttribute).Name));
                var interfaceName = implementorAttribute.InterfaceType.FullName;
                if (!implementorAttribute.InterfaceType.IsInterface)
                    ThrowImplementingAssemblyException(DefinitionSet.FormatNonInterfaceException(interfaceName));
                if (typeof(INTERFACE) == implementorAttribute.InterfaceType) {
                    if (implementorAttribute.ImplementorType == null)
                        ThrowImplementingAssemblyException(DefinitionSet.FormatNullImplementorException(implementorAttribute.InterfaceType.FullName));
                    implementorType = assembly.GetType(implementorAttribute.ImplementorType.FullName);
                    Debug.Assert(implementorType != null);
                    if (!implementorType.IsClass)
                        ThrowImplementingAssemblyException(DefinitionSet.FormatNonClassImplementorException(implementorType.FullName));
                    if (!implementorType.IsAssignableTo(implementorAttribute.InterfaceType))
                        ThrowImplementingAssemblyException(DefinitionSet.FormatNonImplementorException(implementorType.FullName, implementorAttribute.InterfaceType.FullName));
                    var constructorInfo = implementorType.GetConstructor(Type.EmptyTypes);
                    anInstance = constructorInfo.Invoke(Array.Empty<object>());
                    break;
                }
            } //loop
        } //Construct

        public PluginFinder(Assembly assembly) {
            Construct(assembly);
        } //PluginFinder

        public INTERFACE Instance { get { return (INTERFACE)anInstance; } }

        object anInstance;

        public PluginFinder() { }

    } //class PluginFinder

}
