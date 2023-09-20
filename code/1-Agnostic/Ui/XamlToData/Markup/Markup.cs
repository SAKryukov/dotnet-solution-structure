namespace SA.Agnostic.UI.Markup {
    using System;
    //using TypeExtension = System.Windows.Markup.TypeExtension;
    using StaticExtension = System.Windows.Markup.StaticExtension;

    public class TypeKey : StaticExtension {
        public TypeKey() { }
        public TypeKey(Type targetType) { TargetType = targetType; }
        public Type TargetType { get; set; }
        public override object ProvideValue(IServiceProvider serviceProvider) {
            return TargetType;
        } //ProvideValue
    } //class TypeKey

}
