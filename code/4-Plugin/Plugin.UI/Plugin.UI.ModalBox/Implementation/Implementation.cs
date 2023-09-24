/*
    Copyright (C) 2023 by Sergey A Kryukov
    https://www.SAKryukov.org
    https://github.com/SAKryukov
    https://www.codeproject.com/Members/SAKryukov
*/

[assembly: SA.Agnostic.PluginManifest(
    typeof(SA.Semantic.UI.IUiPlugin),
    typeof(SA.Plugin.Implementation))]

namespace SA.Plugin {
    using INamedPlugin = Semantic.INamedPlugin;
    using IUiHost = Semantic.UI.IUiHost;
    using IUiPlugin = Semantic.UI.IUiPlugin;

    class Implementation : IUiPlugin {

        void IUiPlugin.Create(IUiHost host) { this.host = host; }

        void IUiPlugin.Execute() {
            if (host == null) return;
            View.Box window = new();
            if (host.MainApplicationWindow == null) return;
            window.Owner = host.MainApplicationWindow;
            window.ShowDialog();
        } //IUiPlugin.Execute

        string INamedPlugin.DisplayName { get { return Main.DefinitionSet.pluginName; } }

        IUiHost host;

    } //class Implementation

}
