namespace SA.Agnostic {
    using System;

    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false, Inherited = false)]
    public class PluginDataElementDescriptorAttribute : System.Attribute {
        public PluginDataElementDescriptorAttribute(System.Enum pluginTypeRole) { PluginTypeRole = pluginTypeRole; }
        public System.Enum PluginTypeRole { get; set; }
    } //class PluginDataElementDescriptorAttribute

    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
    public class PluginManifestAttribute : System.Attribute {
        public PluginManifestAttribute(Type interfaceType, Type implementorType) {
            this.interfaceType = interfaceType;
            this.implementorType = implementorType;
        }
        readonly Type interfaceType = default;
        readonly Type implementorType = default;
        public Type InterfaceType { get { return interfaceType; } }
        public Type ImplementorType { get { return implementorType; } }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class EntryPointAttribute : System.Attribute { }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class InitializationPointAttribute : System.Attribute { }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class FinalizationPointAttribute : System.Attribute { }

}
