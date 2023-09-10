namespace SA.Agnostic {
    using System;
    using System.Reflection;
    using Debug = System.Diagnostics.Debug;
    using Type = System.Type;

    public interface IRecognizable { } // all host-side plugin interfaces derive from this interface

    public abstract class PluginFinderBase : IDisposable {
        public virtual void Unload() { }
        public Assembly Assembly { get; private protected set; }
        void IDisposable.Dispose() { Unload(); }
    } //PluginFinderBase

    public class PluginFinder<INTERFACE> : PluginFinderBase
        where INTERFACE : IRecognizable {

        public class PluginImplementationException : System.ApplicationException {
            internal PluginImplementationException(string message) : base(message) { }
        } //PluginImplementationException

        private protected void Construct(Assembly assembly) {
            void ThrowImplementingAssemblyException(string reason) {
                var pluginImplementorAttributeName = typeof(PluginManifestAttribute).Name;
                throw new PluginImplementationException(DefinitionSet.FormatAssemblyException(assembly.Location, pluginImplementorAttributeName, reason));
            } //ThrowImplementingAssemblyException
            object[] attributes = assembly.GetCustomAttributes(typeof(PluginManifestAttribute), false);
            Assembly = assembly; //assembly is valid after the first successful call to an assembly method
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
                    var constructorInfo = implementorType.GetConstructor(
                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                        null,
                        Type.EmptyTypes,
                        null);
                    if (constructorInfo != null)
                        anInstance = constructorInfo.Invoke(Array.Empty<object>());
                    break;
                }
            } //loop
        } //Construct

        private protected PluginFinder() { } // sic! required by PluginLoader
        public PluginFinder(Assembly assembly) {
            Construct(assembly);
        } //PluginFinder

        public INTERFACE Instance { get { return (INTERFACE)anInstance; } }

        object anInstance;

    } //class PluginFinder

}
