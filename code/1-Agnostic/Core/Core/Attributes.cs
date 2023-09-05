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

    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
    public class AuthorAttribute : Attribute {
        public AuthorAttribute(string author) { this.author = author; }
        readonly string author;
        public string Author { get { return author; } }
    } //class AuthorAttribute

}
