namespace SA.Test.View {
    using Semantic;

    public partial class WindowMainTest : IUiHost {

        System.Windows.Window IUiHost.MainApplicationWindow { get { return this; } }
        System.Windows.Controls.Decorator IUiHost.PluginHostContainer { get { return borderPluginHostContainer; } }

    } //class WindowMain

}
