namespace SA.Semantic {

    public interface IUiHost {
        System.Windows.Window MainApplicationWindow { get { return null; } }
        System.Windows.Controls.Decorator PluginHostContainer { get { return null; } }
        void InitializePluginHostContainer() { }
        void HandleException(System.Exception exception);
    } //interface IUiHost

    public interface IUiPlugin : INamedPlugin {
        void Create(IUiHost host) { }
        void Execute() { }
        void Destroy() { }
    } //interface IUiPlugin

} //SA.Semantic