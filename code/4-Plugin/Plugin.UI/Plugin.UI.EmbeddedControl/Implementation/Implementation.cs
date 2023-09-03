[assembly: SA.Agnostic.PluginManifest(
    typeof(SA.Semantic.IUiPlugin),
    typeof(SA.Plugin.Implementation))]

namespace SA.Plugin {
    using INamedPlugin = Semantic.INamedPlugin;
    using IUiHost = Semantic.IUiHost;
    using IUiPlugin = Semantic.IUiPlugin;

    class Implementation : IUiPlugin {

        void IUiPlugin.Create(IUiHost host) { this.host = host; }

        void IUiPlugin.Execute() {
            if (host == null) return;
            if (host.PluginHostContainer == null) return;
            if (host.MainApplicationWindow == null) return;
            host.InitializePluginHostContainer();
            View.EmbeddedControl control = new();
            control.host = host;
            host.PluginHostContainer.Child = control;
            control.Focus();
        } //IUiPlugin.Execute

        string INamedPlugin.DisplayName { get { return DefinitionSet.pluginName; } }

        IUiHost host;

    } //class Implementation

}
