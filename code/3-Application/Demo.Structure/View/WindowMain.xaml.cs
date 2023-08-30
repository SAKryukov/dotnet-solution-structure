namespace SA.View {
    using System.Windows;
    using AdvancedApplicationBase = Agnostic.UI.AdvancedApplicationBase;

    public partial class WindowMain : Window {

        public class DataGridRow {
            public string Name { get; set; }
            public string Value { get; set; }
        } //DataGridRow

        public WindowMain() {
            InitializeComponent();
            AdvancedApplicationBase application = AdvancedApplicationBase.Current;
            DataGridRow[] rowSet = new DataGridRow[] {
                new DataGridRow() { Name = "Product Name", Value = application.ProductName },
                new DataGridRow() { Name = "Title", Value = application.Title },
                new DataGridRow() { Name = "Assembly Description", Value = application.AssemblyDescription },
                new DataGridRow() { Name = "Copyright", Value = application.Copyright },
                new DataGridRow() { Name = "Company Name", Value = application.CompanyName },
                new DataGridRow() { Name = "Assembly Version", Value = application.AssemblyVersion.ToString() },
                new DataGridRow() { Name = "Assembly File Version", Value = application.AssemblyFileVersion.ToString() },
                new DataGridRow() { Name = "Assembly Informational Version", Value = application.AssemblyInformationalVersion },
            };
            dataGrid.ItemsSource = rowSet;
            statusBarItemCopyrightTextBlock.Text = application.Copyright;
            statusBarItemCopyrightTextBlock.MouseDown += (sender, eventArgs) => {
                int x = 0;
                int y = 1;
                x = y / x;
            }; // statusBarItemCopyrightTextBlock.MouseDown
            buttonExceptionHide.Click += (sender, eventArgs) => SetExceptionVisibility(false);
            buttonCopyException.Click += (sender, eventArgs) => CopyLastExceptionDumpToClipboard();
        } //WindowMain

    } //class WindowMain

}

