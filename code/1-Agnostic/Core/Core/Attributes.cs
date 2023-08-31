namespace SA.Agnostic {
    using System;

    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
    public class PluginManifestAttribute : Attribute {
        public PluginManifestAttribute(Type interfaceType, Type implementorType) {
            this.interfaceType = interfaceType;
            this.implementorType = implementorType;
        }
        readonly Type interfaceType = default;
        readonly Type implementorType = default;
        public Type InterfaceType { get { return interfaceType; } }
        public Type ImplementorType { get { return implementorType; } }
    }

}
