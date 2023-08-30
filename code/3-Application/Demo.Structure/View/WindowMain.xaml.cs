namespace SA.Application.View {
    using System.Windows;
    using AdvancedApplicationBase = Agnostic.UI.AdvancedApplicationBase;
    using DataGridList = System.Collections.Generic.List<WindowMain.DataGridRow>;

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
            void ScrollDown() {
                if (dataGrid.Items.Count < 1) return;
                dataGrid.ScrollIntoView(dataGrid.Items[^1]);
            } //ScrollDown
            void AddRow(string name, string value, bool isPlugin = true) {
                dataGrid.ItemsSource = null;
                rowSet.Add(new DataGridRow() {
                    Mark = isPlugin ? DefinitionSet.markPluginAssembly : DefinitionSet.markEntryAssembly,
                    Name = name,
                    Value = value });
                dataGrid.ItemsSource = rowSet;
                ScrollDown();
            } //AddRow
            rowSet.Add(new DataGridRow() { Name = "Product Name", Value = application.ProductName });
            rowSet.Add(new DataGridRow() { Name = "Title", Value = application.Title });
            rowSet.Add(new DataGridRow() { Name = "Assembly Description", Value = application.AssemblyDescription });
            rowSet.Add(new DataGridRow() { Name = "Copyright", Value = application.Copyright });
            rowSet.Add(new DataGridRow() { Name = "Company Name", Value = application.CompanyName });
            rowSet.Add(new DataGridRow() { Name = "Assembly Version", Value = application.AssemblyVersion.ToString() });
            rowSet.Add(new DataGridRow() { Name = "Assembly File Version", Value = application.AssemblyFileVersion.ToString() });
            rowSet.Add(new DataGridRow() { Name = "Assembly Informational Version", Value = application.AssemblyInformationalVersion });
            initlalRowCount = rowSet.Count;
            dataGrid.ItemsSource = rowSet;
            borderMain.ToolTip = DefinitionSet.dataGridToolTip;
            statusBarItemCopyrightTextBlock.Text = application.Copyright;
            statusBarItemCopyrightTextBlock.MouseDown += (sender, eventArgs) => {
                if (rowSet.Count < 20) { //SA???
                    System.Reflection.Assembly assembly = System.Reflection.Assembly.GetEntryAssembly();
                    AddRow("Location", assembly.Location);
                    AddRow("Full Name", assembly.FullName);
                    AddRow("Loaded Modules", assembly.GetLoadedModules().Length.ToString());
                    AddRow("Referenced Assemblies", assembly.GetReferencedAssemblies().Length.ToString());
                    AddRow("Custom Attributes", assembly.GetCustomAttributes(false).Length.ToString());
                    AddRow("Types", assembly.GetTypes().Length.ToString());
                    AddRow("Exported Types", assembly.GetExportedTypes().Length.ToString());
                    AddRow("Forwarded Types", assembly.GetForwardedTypes().Length.ToString());
                    if (assembly.EntryPoint != null) {
                        AddRow("Entry Point Name", assembly.EntryPoint.Name);
                        AddRow("Entry Point Return Type", assembly.EntryPoint.ReturnType.ToString());
                        AddRow("Entry Point Return Declaring Type", assembly.EntryPoint.DeclaringType.FullName);
                        AddRow("Entry Point Parameters", assembly.EntryPoint.GetParameters().Length.ToString());
                    } //if
                } else
                    Revert();
            }; // statusBarItemCopyrightTextBlock.MouseDown
            buttonExceptionHide.Click += (_, _) => SetExceptionVisibility(false);
            buttonCopyException.Click += (_, _) => CopyLastExceptionDumpToClipboard();
            AddCommandBindings();
        } //WindowMain

        void Revert() {
            dataGrid.ItemsSource = null;
            rowSet.RemoveRange(initlalRowCount, rowSet.Count - initlalRowCount);
            dataGrid.ItemsSource = rowSet;
        } //Revert

        readonly DataGridList rowSet = new();
        readonly int initlalRowCount;

    } //class WindowMa

}
