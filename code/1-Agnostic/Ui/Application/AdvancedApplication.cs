namespace SA.Agnostic.UI {
    using System;
    using System.Reflection;
    using System.Windows;
    using CultureInfo = System.Globalization.CultureInfo;
    using MetadataDictionary = System.Collections.Generic.Dictionary<string, string>;

    public interface IExceptionPresenter {
        void Show(string exceptionTypeName, string exceptionMessage, string exception);
    } //interface IExceptionPresenter

    public abstract class AdvancedApplicationBase : Application {

        public AdvancedApplicationBase() {
            DispatcherUnhandledException += (sender, eventArgs) => {
                ShowException(eventArgs.Exception);
                eventArgs.Handled = true;
            }; //DispatcherUnhandledException
        } //AdvancedApplicationBase

        private protected IExceptionPresenter exceptionPresenter;

        private protected abstract Window CreateMainWindow();

        public string ExecutableDirectory { get { return assemblyWrapper.AssemblyDirectory; } }

        protected override void OnStartup(StartupEventArgs e) {
            ShutdownMode = ShutdownMode.OnMainWindowClose;
            var mainWindow = CreateMainWindow();
            MainWindow = mainWindow;
            mainWindow.Title = DefinitionSet.formatTitle(Title);
            mainWindow.Show();
            base.OnStartup(e);
            startupComplete = true;
        } //OnStartup

        void ShowException(Exception e) {
            if (exceptionPresenter == null) // exception fallback
                MessageBox.Show(
                    e.ToString(),
                    ProductName,
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            else
                exceptionPresenter.Show(e.GetType().FullName, e.Message, e.ToString());
            if (!startupComplete)
                Shutdown();
        } //ShowException

        public string CompanyName { get { return assemblyWrapper.CompanyName; } }
        public string AssemblyConfiguration { get { return assemblyWrapper.AssemblyConfiguration; } }
        public string Title { get { return assemblyWrapper.Title; } }
        public string ProductName { get { return assemblyWrapper.ProductName; } }
        public string AssemblyDescription { get { return assemblyWrapper.AssemblyDescription; } }
        public string Copyright { get { return assemblyWrapper.Copyright; } }
        public Version AssemblyFileVersion { get { return assemblyWrapper.AssemblyFileVersion; } }
        public string AssemblyInformationalVersion { get { return assemblyWrapper.AssemblyInformationalVersion; } }
        public Version AssemblyVersion { get { return assemblyWrapper.AssemblyVersion; } }
        public string ExecutablePath { get { return assemblyWrapper.AssemblyDirectory; } }
        public Assembly EntryAssembly { get { return assemblyWrapper.Assembly; } }
        public MetadataDictionary AssemblyMetadata { get { return assemblyWrapper.AssemblyMetadata; } }
        public string[] AssemblyAuthors { get { return assemblyWrapper.Authors; } }
        public string TargetFrameworkName { get { return assemblyWrapper.TargetFrameworkName; } }
        public string TargetFrameworkDisplayName { get { return assemblyWrapper.TargetFrameworkDisplayName; } }
        public string TargetPlatformName { get { return assemblyWrapper.TargetPlatformName; } }
        public string SupportedOSPlatformName { get { return assemblyWrapper.SupportedOSPlatformName; } }
        public static new AdvancedApplicationBase Current { get { return (AdvancedApplicationBase)Application.Current; } }

        public CultureInfo Localize(CultureInfo culture) {
            return localizationContext.Localize(culture, this);
        } //Localize
        public static CultureInfo Localize(Application application, LocalizationContext localizationContext, CultureInfo culture) {
            return localizationContext?.Localize(culture, application);
        } //Localize

        bool startupComplete;

        readonly AssemblyWrapper assemblyWrapper = new(Assembly.GetEntryAssembly());
        readonly LocalizationContext localizationContext = new();
    } //class AdvancedApplicationBase

}
