namespace SA.Application.View {
    using Semantic;

    public partial class WindowMain : IHost, IUiHost {

        void IHost.Add(string property, string value) {
            AddRow(property, value);
        } //IHost.Add

        class UiPluginException : System.Exception {
            internal UiPluginException(System.Exception inner)
                : base(Main.DefinitionSet.PluginHost.wrapperExceptionName, inner) { }
        } //System.Exception

        void IUiHost.HandleException(System.Exception exception) { throw new UiPluginException(exception); }
        System.Windows.Window IUiHost.MainApplicationWindow { get { return this; } }
        System.Windows.Controls.Decorator IUiHost.PluginHostContainer { get { return borderPluginHost; } }
        void IUiHost.InitializePluginHostContainer() { SetStateVisibility(state: VisibilityState.UiPluginHost); }

    } //class WindowMain

}
