[assembly: SA.Agnostic.PluginManifest(
    typeof(SA.Semantic.IUiPlugin),
    typeof(SA.Plugin.Implementation))]

namespace SA.Plugin {
    using INamedPlugin = Semantic.INamedPlugin;
    using IUiPlugin = Semantic.IUiPlugin;
    using IUiHost = Semantic.IUiHost;

    class Implementation : IUiPlugin {

        void IUiPlugin.Create(IUiHost host) { this.host = host; }

        void IUiPlugin.Execute() {
            if (host == null) return;
            View.Box window = new();
            if (host.MainApplicationWindow == null) return;
            window.Owner = host.MainApplicationWindow;
            window.ShowDialog();
        } //IUiPlugin.Execute

        string INamedPlugin.DisplayName { get { return DefinitionSet.pluginName; } }

        IUiHost host;

    } //class Implementation

}