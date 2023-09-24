/*
    Copyright (C) 2023 by Sergey A Kryukov
    https://www.SAKryukov.org
    https://github.com/SAKryukov
    https://www.codeproject.com/Members/SAKryukov
*/

namespace SA.Agnostic.UI.Markup {
    using System;
    using System.Windows;

    public class DependencyPropertyOwner : DependencyObject {
 
        public static DependencyProperty RegisterDependencyProperty<T_OWNER, T_PROPERTY>(string propertyName, Action<T_OWNER, T_PROPERTY> action = null)
                => DependencyProperty.Register(
                    propertyName,
                    typeof(T_PROPERTY),
                    typeof(T_OWNER),
                    new PropertyMetadata(null, (sender, eventArgs) => {
                    if (sender is T_OWNER owner && eventArgs.NewValue is T_PROPERTY newValue && action != null)
                        action(owner, newValue);
                }));

        public static DependencyProperty RegisterAttachedProperty<T_OWNER, T_PROPERTY>(string propertyName, Action<T_OWNER, T_PROPERTY> action = null)
                => DependencyProperty.RegisterAttached(
                    propertyName,
                    typeof(T_PROPERTY),
                    typeof(T_OWNER),
                    new PropertyMetadata(null, (sender, eventArgs) => {
                    if (sender is T_OWNER owner && eventArgs.NewValue is T_PROPERTY newValue && action != null)
                        action(owner, newValue);
                }));

        public static DependencyPropertyKey RegisterReadOnlyProperty<T_OWNER, T_PROPERTY>(string propertyName) =>
                DependencyProperty.RegisterReadOnly(
                    propertyName,
                    typeof(T_PROPERTY),
                    typeof(T_OWNER),
                    new PropertyMetadata());

        public static DependencyPropertyKey RegisterAttachedReadOnlyProperty<T_OWNER, T_PROPERTY>(string propertyName) =>
                DependencyProperty.RegisterAttachedReadOnly(
                    propertyName,
                    typeof(T_PROPERTY),
                    typeof(T_OWNER),
                    new PropertyMetadata());

    } //class DependencyPropertyOwner

}

