namespace SA.Application.View {
    using System;
    using System.Windows;
    using AdvancedApplicationBase = Agnostic.UI.AdvancedApplicationBase;
    using DataGridSet = System.Collections.ObjectModel.ObservableCollection<WindowMain.DataGridRow>;

    public partial class WindowMain : Window {

        public class DataGridRow {
            public DataGridRow() {
                Mark = DefinitionSet.markEntryAssembly;
            } //DataGridRow
            public string Mark { get; set; }
            public string Name { get; set; }
            public string Value { get; set; }
        } //DataGridRow

        public WindowMain() {
            InitializeComponent();
            AdvancedApplicationBase application = AdvancedApplicationBase.Current;
            rowSet.Add(new DataGridRow() { Name = DefinitionSet.AssemblyPropertySet.productName, Value = application.ProductName });
            rowSet.Add(new DataGridRow() { Name = DefinitionSet.AssemblyPropertySet.title, Value = application.Title });
            rowSet.Add(new DataGridRow() { Name = DefinitionSet.AssemblyPropertySet.assemblyDescription, Value = application.AssemblyDescription });
            rowSet.Add(new DataGridRow() { Name = DefinitionSet.AssemblyPropertySet.copyright, Value = application.Copyright });
            rowSet.Add(new DataGridRow() { Name = DefinitionSet.AssemblyPropertySet.companyName, Value = application.CompanyName });
            rowSet.Add(new DataGridRow() { Name = DefinitionSet.AssemblyPropertySet.assemblyVersion, Value = application.AssemblyVersion.ToString() });
            rowSet.Add(new DataGridRow() { Name = DefinitionSet.AssemblyPropertySet.assemblyFileVersion, Value = application.AssemblyFileVersion.ToString() });
            rowSet.Add(new DataGridRow() { Name = DefinitionSet.AssemblyPropertySet.assemblyInformationalVersion, Value = application.AssemblyInformationalVersion });
            initlalRowCount = rowSet.Count;
            dataGrid.ItemsSource = rowSet;
            borderMain.ToolTip = DefinitionSet.dataGridToolTip;
            statusBarItemCopyrightTextBlock.Text = application.Copyright;
            Semantic.IPropertyPlugin plugin = GetPropertyPlugin(); //SA??? to be moved
            statusBarItemCopyrightTextBlock.MouseDown += (_, _) => {
                if (rowSet.Count < 20) //SA??? to be moved
                    plugin.DiscoverProperties(System.Reflection.Assembly.GetEntryAssembly(), this);
                else
                    Revert();
            }; // statusBarItemCopyrightTextBlock.MouseDown
            buttonExceptionHide.Click += (_, _) => SetExceptionVisibility(false);
            buttonCopyException.Click += (_, _) => CopyLastExceptionDumpToClipboard();
            AddCommandBindings();
        } //WindowMain

        protected override void OnContentRendered(EventArgs e) {
            base.OnContentRendered(e);
            SizeToContent = SizeToContent.Manual;
        } //OnContentRendered

        void AddRow(string name, string value, bool isPlugin = true) {
            void ScrollDown() {
                if (dataGrid.Items.Count < 1) return;
                dataGrid.ScrollIntoView(dataGrid.Items[^1]);
            } //ScrollDown
            rowSet.Add(new DataGridRow() {
                Mark = isPlugin ? DefinitionSet.markPluginAssembly : DefinitionSet.markEntryAssembly,
                Name = name,
                Value = value,
            });
            ScrollDown();
        } //AddRow

        void Revert() {
            while (rowSet.Count > initlalRowCount)
                rowSet.RemoveAt(rowSet.Count - 1);
        } //Revert

        static Semantic.IPropertyPlugin GetPropertyPlugin() {
            System.Reflection.Assembly assembly = //SA??? to move
                System.Reflection.Assembly.LoadFrom(System.IO.Path.Combine(AdvancedApplicationBase.ExecutableDirectory, "Plugin.AssemblyExplorer.dll"));
            Agnostic.PluginFinder<Semantic.IPropertyPlugin> finder = new(assembly);
            return finder.Instance;
        } //GetPropertyPlugin

        readonly DataGridSet rowSet = new();
        readonly int initlalRowCount;

    } //class WindowMa

}
