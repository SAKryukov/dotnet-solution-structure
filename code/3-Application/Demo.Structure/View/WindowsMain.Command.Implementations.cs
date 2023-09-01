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

        bool ExecuteUiPlugin(bool doAct = false) {
            if (listBoxPlugin.SelectedIndex < 0) return false;
            if (!doAct)
                return pluginSet[listBoxPlugin.SelectedIndex].Classifier == PluginSetElementClassifier.Ui;
            Semantic.IUiPlugin plugin = pluginSet[listBoxPlugin.SelectedIndex].Plugin as Semantic.IUiPlugin;
            if (plugin == null) return false;
            plugin.Create(this);
            plugin.Execute();
            plugin.Destroy();
            return true;
        } //ExecuteUiPlugin

        bool ExecutePropertyPlugin(PropertyPluginAction action, bool doAct = false) {
            if (listBoxPlugin.SelectedIndex < 0) return false;
            if (!doAct)
                return pluginSet[listBoxPlugin.SelectedIndex].Classifier == PluginSetElementClassifier.Property;
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
