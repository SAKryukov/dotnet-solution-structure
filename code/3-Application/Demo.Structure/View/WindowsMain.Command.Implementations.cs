namespace SA.Application.View {
    using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
    using Assembly = System.Reflection.Assembly;
    using AssemblyLoadContext = System.Runtime.Loader.AssemblyLoadContext;

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
            if (loadPluginDialog.FileNames.Length < 1) return false;
            foreach (var filename in loadPluginDialog.FileNames) {
                if (!pluginSet.Add(filename)) continue;
                listBoxPlugin.Items.Refresh();
                listBoxPlugin.SelectedItem = listBoxPlugin.Items[^1];
                listBoxPlugin.Focus();
            } //loop
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

        bool ExecuteUiPlugin(bool doAct = false) {
            if (listBoxPlugin.SelectedIndex < 0) return false;
            if (!doAct)
                return pluginSet[listBoxPlugin.SelectedIndex].Classifier == Main.PluginSetElementClassifier.Ui;
            if (pluginSet[listBoxPlugin.SelectedIndex].Plugin is not Semantic.IUiPlugin plugin) return false;
            SetStateVisibility(state: VisibilityState.UiPluginHost);
            plugin.Create(this);
            plugin.Execute();
            plugin.Destroy();
            return true;
        } //ExecuteUiPlugin

        bool ExecutePropertyPlugin(PropertyPluginAction action, bool doAct = false) {
            if (listBoxPlugin.SelectedIndex < 0) return false;
            if (!doAct)
                return pluginSet[listBoxPlugin.SelectedIndex].Classifier == Main.PluginSetElementClassifier.Property;
            Assembly assembly = null;
            AssemblyLoadContext assemblyLoadContext = null;
            if (action == PropertyPluginAction.ProcessEntryAssembly)
                assembly = Assembly.GetEntryAssembly();
            else if (action == PropertyPluginAction.ProcessPluginAssembly)
                assembly = pluginSet[listBoxPlugin.SelectedIndex].Loader.Assembly;
            if (action == PropertyPluginAction.LoadAndProcessAssembly) {
                if (loadAssemblyDialog.ShowDialog(this) != true) return false;
                assemblyLoadContext = new(null, isCollectible: true);
                // can throw exception:
                assembly = assemblyLoadContext.LoadFromAssemblyPath(loadAssemblyDialog.FileName);
            } //if
            if (assembly == null) return false;
            Revert();
            pluginSet[listBoxPlugin.SelectedIndex].PropertyPlugin.DiscoverProperties(assembly, this);
            if (assemblyLoadContext == null) return true;
            assemblyLoadContext.Unload();
            return true;
        } //ExecutePropertyPlugin

        void SetInitialDirectory() {
            var path = Agnostic.UI.AdvancedApplicationBase.Current.ExecutablePath;
            loadPluginDialog.InitialDirectory = path;
            loadAssemblyDialog.InitialDirectory = path;
        } //SetInitialDirectory

        void SetupDialogs() {
            loadPluginDialog.Filter = Main.DefinitionSet.DialogPropertySet.pluginDialogFilter;
            loadPluginDialog.Title = Main.DefinitionSet.DialogPropertySet.pluginDialogTitle;
            loadPluginDialog.Multiselect = true;
            loadPluginDialog.RestoreDirectory = false;
            loadAssemblyDialog.Filter = Main.DefinitionSet.DialogPropertySet.assemblyDialogFilter;
            loadAssemblyDialog.Title = Main.DefinitionSet.DialogPropertySet.assemblyDialogTitle;
            loadAssemblyDialog.RestoreDirectory = false;
        } //SetupDialogs

        readonly OpenFileDialog loadPluginDialog = new();
        readonly OpenFileDialog loadAssemblyDialog = new();
        readonly About about = new();
        readonly Main.PluginSetExtension pluginSet = new();

    } //class WindowMain

}
