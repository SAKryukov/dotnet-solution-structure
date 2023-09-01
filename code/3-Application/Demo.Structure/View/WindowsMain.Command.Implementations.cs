namespace SA.Application.View {
    using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

    public partial class WindowMain {

        internal enum PropertyPluginAction {
            ProcessEntryAssembly,
            ProcessPluginAssembly,
            LoadAndProcessAssembly,
        } //enum PropertyPluginAction

        bool LoadPlugin(bool doAct = false) {
            if (!doAct)
                return true;
            SetInitialDirectory();
            if (loadPluginDialog.ShowDialog(this) != true) return false;
            return true;
        } //LoadPlugin

        bool ExecuteUiPlugin(bool doAct = false) {
            if (!doAct)
                return true;
            return true;
        } //ExecuteUiPlugin

        bool ExecutePropertyPlugin(PropertyPluginAction action, bool doAct = false) {
            if (!doAct)
                return true;
            if (action == PropertyPluginAction.LoadAndProcessAssembly)
                if (loadAssemblyDialog.ShowDialog(this) != true) return false;
            return true;
        } //ExecutePropertyPlugin

        void SetInitialDirectory() {
            var path = Agnostic.UI.AdvancedApplicationBase.Current.ExecutablePath;
            loadPluginDialog.InitialDirectory = path;
            loadAssemblyDialog.InitialDirectory = path;
        } //SetInitialDirectory

        void SetupDialogs() {
            loadPluginDialog.Filter = DefinitionSet.DialogPropertySet.pluginDialogFilter;
            loadPluginDialog.Title = DefinitionSet.DialogPropertySet.pluginDialogTitle;
            loadPluginDialog.RestoreDirectory = false;
            loadAssemblyDialog.Filter = DefinitionSet.DialogPropertySet.assemblyDialogFilter;
            loadAssemblyDialog.Title = DefinitionSet.DialogPropertySet.assemblyDialogTitle;
            loadAssemblyDialog.RestoreDirectory = false;
        } //SetupDialogs

        readonly OpenFileDialog loadPluginDialog = new();
        readonly OpenFileDialog loadAssemblyDialog = new();
        readonly About about = new();

    } //class WindowMain

}
