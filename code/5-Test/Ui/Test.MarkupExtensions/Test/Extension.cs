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

    public class TypeKey : TypeExtension {
        public TypeKey() { }
        public TypeKey(Type targetType) { TargetType = targetType; }
        public Type TargetType { get; set; }
        public override object ProvideValue(IServiceProvider serviceProvider) {
            return TargetType;
        } //ProvideValue
    } //class TypeKey

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

    public class DataTypeProvider : MarkupExtension {
        public DataTypeProvider() { Members = new(); }
        public ResourceDictionary Members { get; set; }
        public override object ProvideValue(IServiceProvider serviceProvider) {
            return this;
        } //ProvideValue
    } //DataTypeProvider

}
