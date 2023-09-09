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
        } //WindowMain

        void PopulateCultureMenu() {
            var cultures = Agnostic.UI.ApplicationSatelliteAssemblyLoader.ImplementedCultures;
            foreach (var culture in cultures) {
                MenuItem menuItem = new() { Header = $"{culture.Name}: {culture.EnglishName}, {culture.NativeName}", DataContext = culture };
                menuItem.Click += (sender, _) => {
                    if (sender is not MenuItem menuItemSender) return;
                    if (menuItemSender.DataContext is not CultureInfo itemCulture) return;
                        Agnostic.UI.ApplicationSatelliteAssemblyLoader.Localize(this, itemCulture);
                    }; //menuItem.Click
                menuItemLanguage.Items.Add(menuItem);
            } //loop
        } //PopulateCultureMenu

        protected override void OnContentRendered(EventArgs e) {
            base.OnContentRendered(e);
            System.Windows.Input.Keyboard.Focus(treeView);
        } //OnContentRendered

        readonly AdvancedApplicationBase advancedApplication;

    } //class WindowMain

}
