namespace SA.Test.Localization {
    using System.Windows;
    using System.Windows.Controls;
    using Agnostic.UI;
    using CultureInfo = System.Globalization.CultureInfo;

    public partial class MainWindow : Window {

        public MainWindow() {
            InitializeComponent();
            PopulateCultureMenu();
            ShowCultureStatus(System.Threading.Thread.CurrentThread.CurrentUICulture);
        } //MainWindow

        void PopulateCultureMenu() {
            var cultures = ApplicationSatelliteAssemblyIndex.ImplementedCultures;
            void AddCulture(CultureInfo culture) {
                MenuItem menuItem = new() {
                    Header = culture.Name,
                    DataContext = culture
                };
                menuItem.Click += (sender, _) => {
                    if (sender is not MenuItem menuItemSender) return;
                    if (menuItemSender.DataContext is not CultureInfo itemCulture) return;
                    AdvancedApplicationBase.Localize(Application.Current, localizationContext, itemCulture);
                    ShowCultureStatus(itemCulture);
                }; //menuItem.Click
                meniItemCulture.Items.Add(menuItem);
            } //AddCulture
            AddCulture(System.Threading.Thread.CurrentThread.CurrentUICulture);
            foreach (var culture in cultures)
                AddCulture(culture);
        } //PopulateCultureMenu

        void ShowCultureStatus(CultureInfo culture) {
            textBlockCulture.Text = culture.Name;
        } //ShowCultureStatus

        readonly Agnostic.UI.LocalizationContext localizationContext = new();

    } //class MainWindow

}
