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

    public class AdvancedApplication<MAINVIEW, APPLICATION_RESOURCES_SOURCE> : AdvancedApplicationBase
            where MAINVIEW : Window, new()
            where APPLICATION_RESOURCES_SOURCE : FrameworkElement, new() {
        private protected override Window CreateMainWindow() {
            Resources = new APPLICATION_RESOURCES_SOURCE().Resources;
            MAINVIEW mainWindow = new();
            if (mainWindow is IExceptionPresenter exceptionPresenter)
                base.exceptionPresenter = exceptionPresenter;
            return mainWindow;
        }
    } //class AdvancedApplication

}
