namespace SA.Agnostic.UI.Extensions {
    using System;
    using System.Windows;
    using System.Windows.Markup;

    public enum MemberKind { Property, Field }

    public class TypeKey : TypeExtension {
        public TypeKey() { }
        public TypeKey(Type targetType) { TargetType = targetType; }
        public Type TargetType { get; set; }
        public override object ProvideValue(IServiceProvider serviceProvider) {
            return TargetType;
        } //ProvideValue
    } //class TypeKey

    public class Member {
        public bool Static { get; set; }
        public Type Type { get; set; }
        public Type TargetType { get; set; }
        public MemberKind MemberKind { get; set; }
        public ResourceDictionary Setters { get; set; }
        public object Value { get; set; }
    } //class MemberBase

    public class DataTypeProvider {
        public DataTypeProvider() { Members = new(); }
        public ResourceDictionary Members { get; set; }
    } //DataTypeProvider

    //-------------------------------------------------------------------------

    public class TestParent {
        public TestParent() { Children = new(); }
        public System.Collections.ObjectModel.Collection<TestChild> Children { get; set; }
    }

    public class TestChild {
        public string Descr { get; set; }
    }

}
