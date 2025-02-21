/*
    Copyright (C) 2023-2025 by Sergey A Kryukov
    https://www.SAKryukov.org
    https://github.com/SAKryukov
*/

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
            unimplementedCulture.Click += (_, _) => {
                ShowCultureStatus(AdvancedApplicationBase.Localize(Application.Current, localizationContext, new CultureInfo("fr")));
            };
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
                    ShowCultureStatus(AdvancedApplicationBase.Localize(Application.Current, localizationContext, itemCulture));
                }; //menuItem.Click
                meniItemCulture.Items.Insert(meniItemCulture.Items.Count - 1, menuItem);
            } //AddCulture
            AddCulture(System.Threading.Thread.CurrentThread.CurrentUICulture);
            foreach (var culture in cultures)
                AddCulture(culture);
        } //PopulateCultureMenu

        void ShowCultureStatus(CultureInfo culture) {
            textBlockCulture.Text = culture.EnglishName;
        } //ShowCultureStatus

        readonly Agnostic.UI.LocalizationContext localizationContext = new();

    } //class MainWindow

}
