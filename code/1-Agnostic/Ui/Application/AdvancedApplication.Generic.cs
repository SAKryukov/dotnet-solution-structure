namespace SA.Agnostic.UI {
    using System.Windows;

    public class AdvancedApplication<MAINVIEW> : AdvancedApplicationBase where MAINVIEW : Window, new() {
        private protected override Window CreateMainWindow() {
            MAINVIEW mainWindow = new();
            if (mainWindow is IExceptionPresenter exceptionPresenter)
                base.exceptionPresenter = exceptionPresenter;
            return mainWindow;
        }
    } //class AdvancedApplication

    public class AdvancedApplication<MAINVIEW, APPLICATION_RESOURCES_SOURCE> : AdvancedApplication<MAINVIEW>
            where MAINVIEW : Window, new()
            where APPLICATION_RESOURCES_SOURCE : FrameworkElement, new() {
        public AdvancedApplication() {
            Resources = new APPLICATION_RESOURCES_SOURCE().Resources;
        } //AdvancedApplication
    } //class AdvancedApplication

}
