namespace SA.Agnostic.UI.Markup {
    using System;
    using TypeExtension = System.Windows.Markup.TypeExtension;
    using MemberCollection =
        System.Collections.ObjectModel.Collection<Member>;

    public class TypeKey : TypeExtension {
        public TypeKey() { }
        public TypeKey(Type targetType) { TargetType = targetType; }
        public Type TargetType { get; set; }
        public override object ProvideValue(IServiceProvider serviceProvider) {
            return TargetType;
        } //ProvideValue
    } //class TypeKey

    public enum MemberKind { Property, Field }

    public class Member {
        public bool Static { get; set; }
        public Type Type { get; set; }
        public Type TargetType { get; set; }
        public MemberKind MemberKind { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }
    } //class Member

    public class DataTypeProvider {
        public DataTypeProvider() { Members = new(); }
        public MemberCollection Members { get; set; }
    } //DataTypeProvider

}
