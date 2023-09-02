namespace SA.Application.View {
    using Semantic;

    public partial class WindowMain : IHost, IUiHost {

        void IHost.Add(string property, string value) {
            AddRow(property, value);
        } //IHost.Add

        System.Windows.Window IUiHost.MainApplicationWindow { get { return this; } }
        System.Windows.Controls.Decorator IUiHost.PluginHostContainer { get { return borderPluginHostContaner; } }
        void IUiHost.InitializePluginHostContainer() { SetStateVisibility(state: VisibilityState.UiPluginHost); }

    } //class WindowMain

}
