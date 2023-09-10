namespace SA.Application.View {
    using System;
    using System.Windows;
    using CultureInfo = System.Globalization.CultureInfo;
    using MenuItem = System.Windows.Controls.MenuItem;
    using AdvancedApplicationBase = Agnostic.UI.AdvancedApplicationBase;
    using ApplicationSatelliteAssemblyIndex = Agnostic.UI.ApplicationSatelliteAssemblyIndex;

    public partial class WindowMain : Window {

        public WindowMain() {
            InitializeComponent();
            PopulateCultureMenu();
            menuItemHelp.Click += (_, _) => about.ShowAbout(this);
            KeyDown += (_, eventArgs) => {
                if (eventArgs.Key == System.Windows.Input.Key.F1)
                    about.ShowAbout(this);
            }; //KeyDown
            menuItemQuit.Click += (_, _) => Close();
            ShowCultureStatus();
        } //WindowMain

        void PopulateCultureMenu() {
            var cultures = ApplicationSatelliteAssemblyIndex.ImplementedCultures;
            void AddCulture(CultureInfo culture) {
                MenuItem menuItem = new() {
                    Header = Main.DefinitionSet.FormatCulture(culture.Name, culture.EnglishName, culture.NativeName),
                    DataContext = culture
                };
                menuItem.Click += (sender, _) => {
                    if (sender is not MenuItem menuItemSender) return;
                    if (menuItemSender.DataContext is not CultureInfo itemCulture) return;
                    advancedApplication.Localize(itemCulture);
                    ShowCultureStatus(itemCulture);
                }; //menuItem.Click
                menuItemLanguage.Items.Add(menuItem);
            } //AddCulture
            AddCulture(System.Threading.Thread.CurrentThread.CurrentUICulture);
            foreach (var culture in cultures)
                AddCulture(culture);
        } //PopulateCultureMenu

        void ShowCultureStatus(CultureInfo culture = null) {
            if (culture == null)
                culture = System.Threading.Thread.CurrentThread.CurrentUICulture;
            textBlockEnglishName.Text = culture.EnglishName;
            textBlockNativeName.Text = culture.EnglishName == culture.NativeName ? null : culture.NativeName;
        } //ShowCultureStatus

        protected override void OnContentRendered(EventArgs e) {
            base.OnContentRendered(e);
            System.Windows.Input.Keyboard.Focus(treeView);
        } //OnContentRendered

        readonly AdvancedApplicationBase advancedApplication = AdvancedApplicationBase.Current;
        readonly About about = new();

    } //class WindowMain

}
