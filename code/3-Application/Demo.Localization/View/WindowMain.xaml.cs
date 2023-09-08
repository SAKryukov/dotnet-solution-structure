namespace SA.Application.View {
    using System.Windows;
    using AdvancedApplicationBase = Agnostic.UI.AdvancedApplicationBase;

    public partial class WindowMain : Window {

        public WindowMain() {
            advancedApplication = AdvancedApplicationBase.Current;
            InitializeComponent();
        } //WindowMain

        readonly AdvancedApplicationBase advancedApplication;

    } //class WindowMain

}
