namespace SA.Agnostic.IO.Extensions {
    using System;
    using System.Windows;
    using System.Windows.Markup;

    public enum MemberKind { Property, Field }
    public class MemberSetter {
        public MemberKind MemberKind { get; set; }
        public Type MemberType { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }
    } //class MemberSetter

    public class EKey : TypeExtension {
        public Type TargetType { get; set; }
        public override object ProvideValue(IServiceProvider serviceProvider) {
            if (TargetType == null) return null;
            /*
            IXamlTypeResolver resolver = (IXamlTypeResolver)serviceProvider.GetService(typeof(IXamlTypeResolver));
            return resolver.Resolve(TargetType);
            */
            return this;
        } //ResourSet
    } //class EKey

        public class NestedTypeExtension : TypeExtension {
            public NestedTypeExtension() { }
            public NestedTypeExtension(string typeName) : base(typeName) { }
            public override object ProvideValue(IServiceProvider serviceProvider) {
                string[] types = TypeName.Split('.');
                IXamlTypeResolver resolver = (IXamlTypeResolver)serviceProvider.GetService(typeof(IXamlTypeResolver));
                if (resolver != null && types.Length > 0) {
                    Type t = resolver.Resolve(types[0]);
                    for (int i = 1; i < types.Length; i++) {
                        t = t.GetNestedType(types[i]);
                    }
                    Type = t;
                    return t;
                }
                return null;
            }
        }

    public class Member : MarkupExtension {
        public bool Static { get; set; }
        public Type Type { get; set; }
        public Type TargetType { get; set; }
        public MemberKind MemberKind { get; set; }
        public ResourceDictionary Setters { get; set; }
        public object Value { get; set; }
        public override object ProvideValue(IServiceProvider serviceProvider) {
            return this;
        } //ProvideValue
    } //class MemberBase

    public class KeyedMember : Member {
        public string Name { get; set; }
    } //KeyedMember

}
