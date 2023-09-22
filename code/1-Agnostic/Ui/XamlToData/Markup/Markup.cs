namespace SA.Agnostic.UI.Markup {
    using System;
    using TypeExtension = System.Windows.Markup.TypeExtension;
    using System.Windows;
    using MemberList = System.Collections.Generic.List<Member>;

    public enum TypeKeyKind { Type, TypeHandle, FullName, Name }

    public class TypeKey : TypeExtension {
        public TypeKey() { }
        public TypeKey(Type targetType) { TargetType = targetType; }
        public TypeKey(Type targetType, TypeKeyKind typeKeyKind) { TargetType = targetType; TypeKeyKind = typeKeyKind; }
        public Type TargetType { get; set; }
        public TypeKeyKind TypeKeyKind { get; set; }
        public override object ProvideValue(IServiceProvider serviceProvider) =>
            TypeKeyKind switch {
                TypeKeyKind.Type => TargetType,
                TypeKeyKind.TypeHandle => TargetType.TypeHandle,
                TypeKeyKind.FullName => TargetType.FullName,
                TypeKeyKind.Name => TargetType.Name,
                _ => TargetType,
            };
        internal static object UserKey(Type requiredType, TypeKeyKind keyKind) =>
            keyKind switch {
                TypeKeyKind.Type => requiredType,
                TypeKeyKind.TypeHandle => requiredType.TypeHandle,
                TypeKeyKind.FullName => requiredType.FullName,
                TypeKeyKind.Name => requiredType.Name,
                _ => requiredType,
            };
    } //class TypeKey

    public enum MemberKind { Property, Field }

    public class Member : DependencyPropertyOwner {
        public bool Static { get; set; }
        public Type Type { get; set; }
        public Type TargetType { get; set; }
        public MemberKind MemberKind { get; set; }
        public string Name { get; set; }
        public static readonly DependencyProperty ValueProperty =
            RegisterDependencyProperty<Member, object>(nameof(Value), (thisObject, newValue) => { thisObject.Value = newValue; });
        public object Value {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        } //Value
    } //class Member

    public class DataSetter : MemberList { }

}
