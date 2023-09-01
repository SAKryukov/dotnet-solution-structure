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
            if (!pluginSet.Add(loadPluginDialog.FileName)) return false;
            listBoxPlugin.Items.Refresh();
            listBoxPlugin.SelectedItem = listBoxPlugin.Items[^1];
            listBoxPlugin.Focus();
            return true;
        } //LoadPlugin

        bool UnloadPlugin(bool doAct = false) {
            int index = listBoxPlugin.SelectedIndex;
            if (!doAct) {
                if (index < 0) return false;
                return pluginSet[index] != null;
            } //if
            pluginSet[listBoxPlugin.SelectedIndex].Loader?.Unload();
            pluginSet.RemoveAt(index);
            listBoxPlugin.Items.Refresh();
            if (listBoxPlugin.Items.Count < 1) return true;
            int closestIndex = index;
            if (closestIndex >= listBoxPlugin.Items.Count)
                --closestIndex;
            if (closestIndex < 0)
                closestIndex = 0;
            listBoxPlugin.SelectedItem = listBoxPlugin.Items[closestIndex];
            listBoxPlugin.Focus();
            return true;
        } //UnloadPlugin

        static bool ExecuteUiPlugin(bool doAct = false) {
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
        readonly PluginSetExtension pluginSet = new();

    } //class WindowMain

}
