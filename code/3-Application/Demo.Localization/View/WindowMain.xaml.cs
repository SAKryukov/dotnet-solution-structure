namespace SA.Application.View {
    using System;
    using System.Windows;
    using AdvancedApplicationBase = Agnostic.UI.AdvancedApplicationBase;

    public partial class WindowMain : Window {

        public WindowMain() {
            advancedApplication = AdvancedApplicationBase.Current;
            InitializeComponent();
            textBlockStatusBarCopyright.Text = advancedApplication.Title;
            Agnostic.UI.ApplicationSatelliteAssemblyLoader.Localize(this, culture);
            //var cultures = Agnostic.UI.ApplicationSatelliteAssemblyLoader.
        } //WindowMain

        protected override void OnContentRendered(EventArgs e) {
            base.OnContentRendered(e);
            System.Windows.Input.Keyboard.Focus(treeView);
        }

        readonly System.Globalization.CultureInfo culture = new("es"); //SA???
        readonly AdvancedApplicationBase advancedApplication;

    } //class WindowMain

}
