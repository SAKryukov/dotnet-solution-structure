/*
    Copyright (C) 2023-2024 by Sergey A Kryukov
    https://www.SAKryukov.org
    https://github.com/SAKryukov
*/

namespace SA.Agnostic.UI.Markup {
    using System;
    using System.Windows;

    public enum MemberKind { Property, Field }

    public class Member : DependencyPropertyOwner {
        public bool Static { get; set; }
        public Type Type { get; set; }
        public MemberKind MemberKind { get; set; }
        public static readonly DependencyProperty ValueProperty =
            RegisterDependencyProperty<Member, object>(nameof(Value), (thisObject, newValue) => { thisObject.Value = newValue; });
        public object Value {
            get => GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        } //Value
    } //class Member

}
