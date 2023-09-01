namespace SA.Plugin {
    using INamedPlugin = Semantic.INamedPlugin;
    using IUiPlugin = Semantic.IUiPlugin;
    using IUiHost = Semantic.IUiHost;

    class Implementation : IUiPlugin {

        void IUiPlugin.Create(IUiHost host) { this.host = host; }

        void IUiPlugin.Execute() {
            if (host == null) return;
            host.PluginHost.Content = new View.EmbeddedControl();
        } //IUiPlugin.Execute

        string INamedPlugin.DisplayName { get { return DefinitionSet.pluginName; } }

        IUiHost host;

    } //class Implementation

}
