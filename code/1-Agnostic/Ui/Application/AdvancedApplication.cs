namespace SA.Agnostic.UI {
    using System;
    using System.Reflection;
    using System.Windows;
    using MetadataDictionary = System.Collections.Generic.Dictionary<string, string>;
    using FlatResourceDictionary = System.Collections.Generic.Dictionary<string, object>;

    public abstract class AdvancedApplicationBase : Application {

        static class DefinitionSet {
            internal static Func<string, string> formatTitle = productName => $" {productName}";
        } //definitionSet

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
            this.ShutdownMode = ShutdownMode.OnMainWindowClose;
            var mainWindow = CreateMainWindow();
            MainWindow = mainWindow;
            mainWindow.Title = DefinitionSet.formatTitle(ProductName);
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

        public FlatResourceDictionary GetResources() {
            return GetResources(Resources);
        } //GetResources
        public static FlatResourceDictionary GetResources(Window window) {
            return GetResources(window?.Resources);
        } //GetResources
        public static FlatResourceDictionary GetResources(ResourceDictionary dictionary) {
            if (dictionary == null) return null;
            System.Collections.Generic.Dictionary<string, object> result = new();
            static void GetPartialResources(System.Collections.Generic.Dictionary<string, object> currentResult, ResourceDictionary partialDictionary) {
                var keys = partialDictionary.Keys;
                foreach (var merged in partialDictionary.MergedDictionaries)
                    GetPartialResources(currentResult, merged);
                foreach (var key in keys) {
                    string stringKey = key.ToString();
                    if (currentResult.ContainsKey(stringKey))
                        currentResult[stringKey] = partialDictionary[key];
                    else
                        currentResult.Add(stringKey, partialDictionary[key]);
                } //loop
            } //GetPartialResources
            GetPartialResources(result, dictionary);
            return result;
        } //GetResources

        bool startupComplete;

        readonly AssemblyWrapper assemblyWrapper = new(Assembly.GetEntryAssembly());

    } //class AdvancedApplicationBase

    public interface IExceptionPresenter {
        void Show(string exceptionTypeName, string exceptionMessage, string exception);
    } //interface IExceptionPresenter

    public class AdvancedApplication<MAINVIEW> : AdvancedApplicationBase where MAINVIEW : Window, new() {
        private protected override Window CreateMainWindow() {
            MAINVIEW mainWindow = new();
            if (mainWindow is IExceptionPresenter exceptionPresenter)
                base.exceptionPresenter = exceptionPresenter;
            return mainWindow;
        }
    } //class AdvancedApplication

}
