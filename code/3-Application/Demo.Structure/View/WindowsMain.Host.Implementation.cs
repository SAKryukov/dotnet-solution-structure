namespace SA.Application.View {
    using Semantic;

    public partial class WindowMain : IHost, IUiHost {

        void IHost.Add(string property, string value) {
            AddRow(property, value);
        } //IHost.Add

        void IUiHost.HandleException(System.Exception exception) { }
        System.Windows.Window IUiHost.MainApplicationWindow { get { return this; } }
        System.Windows.Controls.Decorator IUiHost.PluginHost { get { return borderPluginHost; } }

    } //class WindowMain

}
