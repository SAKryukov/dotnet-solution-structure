namespace SA.Application.View {
    using System.Windows;
    using AdvancedApplicationBase = Agnostic.UI.AdvancedApplicationBase;
    using DataGridSet = System.Collections.ObjectModel.ObservableCollection<WindowMain.DataGridRow>;

    public partial class WindowMain : Window {

        public class DataGridRow {
            public DataGridRow() {
                Mark = Main.DefinitionSet.markEntryAssembly;
            } //DataGridRow
            public string Mark { get; set; }
            public string Name { get; set; }
            public string Value { get; set; }
        } //DataGridRow

        public WindowMain() {
            InitializeComponent();
            SetupDialogs();
            AdvancedApplicationBase application = AdvancedApplicationBase.Current;
            listBoxPlugin.ItemsSource = pluginSet;
            rowSet.Add(new DataGridRow() { Name = Main.DefinitionSet.AssemblyPropertySet.productName, Value = application.ProductName });
            rowSet.Add(new DataGridRow() { Name = Main.DefinitionSet.AssemblyPropertySet.title, Value = application.Title });
            rowSet.Add(new DataGridRow() { Name = Main.DefinitionSet.AssemblyPropertySet.assemblyDescription, Value = application.AssemblyDescription });
            rowSet.Add(new DataGridRow() { Name = Main.DefinitionSet.AssemblyPropertySet.copyright, Value = application.Copyright });
            rowSet.Add(new DataGridRow() { Name = Main.DefinitionSet.AssemblyPropertySet.companyName, Value = application.CompanyName });
            rowSet.Add(new DataGridRow() { Name = Main.DefinitionSet.AssemblyPropertySet.assemblyVersion, Value = application.AssemblyVersion.ToString() });
            rowSet.Add(new DataGridRow() { Name = Main.DefinitionSet.AssemblyPropertySet.assemblyFileVersion, Value = application.AssemblyFileVersion.ToString() });
            rowSet.Add(new DataGridRow() { Name = Main.DefinitionSet.AssemblyPropertySet.assemblyInformationalVersion, Value = application.AssemblyInformationalVersion });
            initlalRowCount = rowSet.Count;
            dataGrid.ItemsSource = rowSet;
            borderMain.ToolTip = Main.DefinitionSet.dataGridToolTip;
            statusBarItemCopyrightTextBlock.Text = application.Copyright;
            buttonExceptionHide.Click += (_, _) => SetStateVisibility();
            buttonCopyException.Click += (_, _) => CopyLastExceptionDumpToClipboard();
            AddCommandBindings();
            void HidePluginHost() {
                if (borderPluginHost.Visibility == Visibility.Visible)
                    SetStateVisibility();
            } //HidePluginHost
            menu.GotKeyboardFocus += (_, _) => HidePluginHost();
        } //WindowMain

        /*
        protected override void OnContentRendered(EventArgs e) {
            base.OnContentRendered(e);
            SizeToContent = SizeToContent.Manual;
        } //OnContentRendered
        */

        void AddRow(string name, string value, bool isPlugin = true) {
            void ScrollDown() {
                if (dataGrid.Items.Count < 1) return;
                dataGrid.ScrollIntoView(dataGrid.Items[^1]);
            } //ScrollDown
            rowSet.Add(new DataGridRow() {
                Mark = isPlugin ? Main.DefinitionSet.markPluginAssembly : Main.DefinitionSet.markEntryAssembly,
                Name = name,
                Value = value,
            });
            ScrollDown();
        } //AddRow

        void Revert() {
            while (rowSet.Count > initlalRowCount)
                rowSet.RemoveAt(rowSet.Count - 1);
        } //Revert

        enum VisibilityState { Normal, UiPluginHost, Exception, }

        void SetStateVisibility(VisibilityState state = VisibilityState.Normal) {
            switch (state) {
                case VisibilityState.Exception:
                    borderMain.Visibility = Visibility.Collapsed;
                    borderPluginHost.Visibility = Visibility.Collapsed;
                    borderException.Visibility = Visibility.Visible;
                    break;
                case VisibilityState.UiPluginHost:
                    borderMain.Visibility = Visibility.Collapsed;
                    borderException.Visibility = Visibility.Collapsed;
                    borderPluginHost.Visibility = Visibility.Visible;
                    break;
                default:
                    borderException.Visibility = Visibility.Collapsed;
                    borderPluginHost.Visibility = Visibility.Collapsed;
                    borderMain.Visibility = Visibility.Visible;
                    break;
            } //switch
        } //SetStateVisibility

        static Semantic.IPropertyPlugin GetPropertyPlugin() {
            Agnostic.PluginLoader<Semantic.IPropertyPlugin> loader = new(System.IO.Path.Combine(AdvancedApplicationBase.ExecutableDirectory, "Plugin.AssemblyExplorer.dll"));
            return loader.Instance;
        } //GetPropertyPlugin

        readonly DataGridSet rowSet = new();
        readonly int initlalRowCount;

    } //class WindowMain

}
