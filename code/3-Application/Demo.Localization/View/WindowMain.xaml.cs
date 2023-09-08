namespace SA.Application.View {
    using System;
    using System.Windows;
    using AdvancedApplicationBase = Agnostic.UI.AdvancedApplicationBase;

    public partial class WindowMain : Window {

        public WindowMain() {
            advancedApplication = AdvancedApplicationBase.Current;
            InitializeComponent();
        } //WindowMain

        protected override void OnContentRendered(EventArgs e) {
            base.OnContentRendered(e);
            System.Windows.Input.Keyboard.Focus(treeView);
        }

        readonly AdvancedApplicationBase advancedApplication;

    } //class WindowMain

}
