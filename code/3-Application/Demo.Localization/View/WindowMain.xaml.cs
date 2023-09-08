namespace SA.Application.View {
    using System;
    using System.Windows;
    using AdvancedApplicationBase = Agnostic.UI.AdvancedApplicationBase;

    public partial class WindowMain : Window {

        public WindowMain() {
            advancedApplication = AdvancedApplicationBase.Current;
            Agnostic.UI.ApplicationSatelliteAssemblyLoader.Localize(this, culture);
            InitializeComponent();
            //var cultures = Agnostic.UI.ApplicationSatelliteAssemblyLoader.
        } //WindowMain

        readonly System.Globalization.CultureInfo culture = new("ru-RU"); //SA???
        readonly AdvancedApplicationBase advancedApplication;

    } //class WindowMain

}
