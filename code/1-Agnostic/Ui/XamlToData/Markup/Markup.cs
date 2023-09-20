namespace SA.Agnostic.UI.Markup {
    using System;
    using System.Windows;
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

    public class Member : DependencyObject {
        public bool Static { get; set; }
        public Type Type { get; set; }
        public Type TargetType { get; set; }
        public MemberKind MemberKind { get; set; }
        public string Name { get; set; }
        public static DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(object),
            typeof(Member),
            new PropertyMetadata(string.Empty, (sender, eventArgs) => {
                if (sender is Member member)
                    member.Value = eventArgs.NewValue;
            }));
        public object Value {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value); 
        }
    } //class Member

    public class DataSetter {
        public DataSetter() { Members = new(); }
        public MemberCollection Members { get; set; }
    } //DataSetter

}
