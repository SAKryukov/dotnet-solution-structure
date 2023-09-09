namespace SA.Application.View {
    using System;
    using System.Windows;
    using CultureInfo = System.Globalization.CultureInfo;
    using AdvancedApplicationBase = Agnostic.UI.AdvancedApplicationBase;
    using MenuItem = System.Windows.Controls.MenuItem;

    public partial class WindowMain : Window {

        public WindowMain() {
            advancedApplication = AdvancedApplicationBase.Current;
            InitializeComponent();
            PopulateCultureMenu();
            textBlockStatusBarCopyright.Text = advancedApplication.Title;
            menuItemHelp.Click += (_, _) => about.ShowAbout(this);
            KeyDown += (_, eventArgs) => {
                if (eventArgs.Key == System.Windows.Input.Key.F1)
                    about.ShowAbout(this);
            }; //KeyDown
        } //WindowMain

        void PopulateCultureMenu() {
            var cultures = Agnostic.UI.ApplicationSatelliteAssemblyLoader.ImplementedCultures;
            foreach (var culture in cultures) {
                MenuItem menuItem = new() {
                    Header = Main.DefinitionSet.FormatCulture(culture.Name, culture.EnglishName, culture.NativeName),
                    DataContext = culture };
                menuItem.Click += (sender, _) => {
                    if (sender is not MenuItem menuItemSender) return;
                    if (menuItemSender.DataContext is not CultureInfo itemCulture) return;
                    Agnostic.UI.ApplicationSatelliteAssemblyLoader.Localize(this, itemCulture);
                    Agnostic.UI.ApplicationSatelliteAssemblyLoader.Localize(about, itemCulture);
                }; //menuItem.Click
                menuItemLanguage.Items.Add(menuItem);
            } //loop
        } //PopulateCultureMenu

        protected override void OnContentRendered(EventArgs e) {
            base.OnContentRendered(e);
            System.Windows.Input.Keyboard.Focus(treeView);
        } //OnContentRendered

        readonly AdvancedApplicationBase advancedApplication;
        readonly About about = new();

    } //class WindowMain

}
