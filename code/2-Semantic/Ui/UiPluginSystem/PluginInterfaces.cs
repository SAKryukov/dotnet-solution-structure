namespace SA.Semantic {

    public interface IUiHost {
        System.Windows.Window MainApplicationWindows { get; }
        System.Windows.Controls.ContentControl PluginHost { get; }
        void HandleException(System.Exception exception);
    } //interface IUiHost

    public interface IUiPlugin : INamedPlugin {
        void Create(IUiHost host) { }
        void Execute() { }
        void Destroy() { }
    } //interface IUiPlugin

} //SA.Semantic