/*
    Copyright (C) 2023 by Sergey A Kryukov
    https://www.SAKryukov.org
    https://github.com/SAKryukov
    https://www.codeproject.com/Members/SAKryukov
*/

namespace SA.Test.View {
    using System.Windows;
    using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

    public partial class WindowMainTest : Window {

        public WindowMainTest() {
            InitializeComponent();
            SetupDialogs();
            menuItemMain.Click += (_, _) => LoadAndExecuteUiPlugin();
            textBlockCopyright.Text = Agnostic.UI.AdvancedApplicationBase.Current.Copyright;
            menu.GotKeyboardFocus += (_, _) => borderPluginHostContainer.Child = null;
        } //WindowMainTest

        void LoadAndExecuteUiPlugin() {
            loadPluginDialog.InitialDirectory = Agnostic.UI.AdvancedApplicationBase.Current.ExecutablePath;
            if (loadPluginDialog.ShowDialog() != true) return;
            Agnostic.PluginLoader<Semantic.UI.IUiPlugin> loader = new(loadPluginDialog.FileName);
            if (loader.Instance == null) return;
            loader.Instance.Create(this);
            loader.Instance.Execute();
            loader.Instance.Destroy();
            loader.Unload();
        } //LoadAndExecuteUiPlugin

        void SetupDialogs() {
            loadPluginDialog.Filter = Main.DefinitionSet.DialogPropertySet.pluginDialogFilter;
            loadPluginDialog.Title = Main.DefinitionSet.DialogPropertySet.pluginDialogTitle;
        } //SetupDialogs

        readonly OpenFileDialog loadPluginDialog = new();

    } //class WindowMainTest

}
