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
            advancedApplication = AdvancedApplicationBase.Current;
            InitializeComponent();
            SetupDialogs();
            AdvancedApplicationBase application = advancedApplication;
            listBoxPlugin.ItemsSource = pluginSet;
            var cd = System.Reflection.CustomAttributeData.GetCustomAttributes(application.EntryAssembly);
            AddRow(Main.DefinitionSet.AssemblyPropertySet.productName, application.ProductName);
            AddRow(Main.DefinitionSet.AssemblyPropertySet.title, application.Title);
            AddRow(Main.DefinitionSet.AssemblyPropertySet.assemblyDescription, application.AssemblyDescription);
            AddRow(Main.DefinitionSet.AssemblyPropertySet.copyright, application.Copyright);
            AddRow(Main.DefinitionSet.AssemblyPropertySet.companyName, application.CompanyName);
            AddRow(Main.DefinitionSet.AssemblyPropertySet.assemblyVersion, application.AssemblyVersion.ToString());
            AddRow(Main.DefinitionSet.AssemblyPropertySet.assemblyFileVersion, application.AssemblyFileVersion.ToString());
            AddRow(Main.DefinitionSet.AssemblyPropertySet.assemblyInformationalVersion, application.AssemblyInformationalVersion);
            AddRow(Main.DefinitionSet.AssemblyPropertySet.assemblyConfiguration, application.AssemblyConfiguration);
            AddRow(Main.DefinitionSet.AssemblyPropertySet.targetFrameworkName, application.TargetFrameworkName);
            AddRow(Main.DefinitionSet.AssemblyPropertySet.targetFrameworkDisplayName, application.TargetFrameworkDisplayName);
            AddRow(Main.DefinitionSet.AssemblyPropertySet.targetPlatformName, application.TargetPlatformName);
            AddRow(Main.DefinitionSet.AssemblyPropertySet.supportedOSPlatformName, application.SupportedOSPlatformName);
            if (application.AssemblyAuthors != null)
                for (int index = 0; index < application.AssemblyAuthors.Length; ++index)
                    if (application.AssemblyAuthors[index] != null && application.AssemblyAuthors[index].Trim().Length > 0)
                        AddRow(Main.DefinitionSet.AssemblyPropertySet.assemblyAuthor, application.AssemblyAuthors[index]);
            if (application.AssemblyMetadata != null)
                foreach(var pair in application.AssemblyMetadata)
                    AddRow(pair.Key, pair.Value);
            initlalRowCount = rowSet.Count;
            dataGrid.ItemsSource = rowSet;
            textBoxStatusBarCopyright.Text = application.Copyright;
            buttonSaveExceptionAndClose.Click += (_, _) => SaveExceptionAndClose();
            AddCommandBindings();
            void HidePluginHost() {
                if (borderPluginHostContaner.Visibility == Visibility.Visible)
                    SetStateVisibility();
            } //HidePluginHost
            menu.GotKeyboardFocus += (_, _) => HidePluginHost();
            KeyUp += (_, _) => textBoxScreenshotStatusBarTool.Text = null;
        } //WindowMain

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo) {
            if (((System.Windows.Input.Keyboard.GetKeyStates(System.Windows.Input.Key.LeftCtrl) & System.Windows.Input.KeyStates.Down) > 0)
                &&
               ((System.Windows.Input.Keyboard.GetKeyStates(System.Windows.Input.Key.LeftShift) & System.Windows.Input.KeyStates.Down) > 0))
                textBoxScreenshotStatusBarTool.Text = Main.DefinitionSet.FormatSizeInformatin(sizeInfo.NewSize.Width, sizeInfo.NewSize.Height);
        } //OnRenderSizeChanged

        void AddRow(string name, string value, bool isPlugin = false) {
            if (string.IsNullOrEmpty(name)) return;
            if (string.IsNullOrEmpty(value)) return;
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
                    borderPluginHostContaner.Visibility = Visibility.Collapsed;
                    borderException.Visibility = Visibility.Visible;
                    break;
                case VisibilityState.UiPluginHost:
                    borderMain.Visibility = Visibility.Collapsed;
                    borderException.Visibility = Visibility.Collapsed;
                    borderPluginHostContaner.Visibility = Visibility.Visible;
                    break;
                default:
                    borderException.Visibility = Visibility.Collapsed;
                    borderPluginHostContaner.Visibility = Visibility.Collapsed;
                    borderMain.Visibility = Visibility.Visible;
                    break;
            } //switch
        } //SetStateVisibility

        readonly DataGridSet rowSet = new();
        readonly int initlalRowCount;
        readonly AdvancedApplicationBase advancedApplication;

    } //class WindowMain

}
