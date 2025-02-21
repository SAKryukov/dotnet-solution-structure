/*
    Copyright (C) 2023-2025 by Sergey A Kryukov
    https://www.SAKryukov.org
    https://github.com/SAKryukov
*/

namespace SA.Application.View {
    using Semantic;
    using Semantic.UI;

    public partial class WindowMain : IHost, IUiHost {

        void IHost.Add(string property, string value) {
            AddRow(property, value, isPlugin: true);
        } //IHost.Add

        System.Windows.Window IUiHost.MainApplicationWindow { get { return this; } }
        System.Windows.Controls.Decorator IUiHost.PluginHostContainer { get { return borderPluginHostContaner; } }
        void IUiHost.InitializePluginHostContainer() { SetStateVisibility(state: VisibilityState.UiPluginHost); }

    } //class WindowMain

}
