namespace SA.Agnostic.UI {
    using System;
    using System.Windows;
    using System.Reflection;
    using Path = System.IO.Path;

    public abstract class AdvancedApplicationBase : Application {

        static class DefinitionSet {
            internal static Func<Exception, string> formatExceptionMessage = exception => $"{exception.GetType().FullName}: {exception.Message}";
            internal static Func<string, string> formatTitle = productName => $" {productName}";
            internal const string exceptionStackFrameDelimiter = "\n\n";
        } //definitionSet

        public AdvancedApplicationBase() {
            DispatcherUnhandledException += (sender, eventArgs) => {
                ShowException(eventArgs.Exception);
                eventArgs.Handled = true;
            }; //DispatcherUnhandledException
        } //AdvancedApplicationBase

        private protected IExceptionPresenter exceptionPresenter;

        private protected abstract Window CreateMainWindow();

        public static string ExecutableDirectory {
            get {
                return Path.GetDirectoryName(Assembly.GetEntryAssembly().ManifestModule.FullyQualifiedName);
            }
        } //EexecutableDirectory

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
                exceptionPresenter.Show(e.GetType().Name, e.Message, e.ToString());
            if (!startupComplete)
                Shutdown();
        } //ShowException

        public string CompanyName {
            get {
                if (companyName == null) {
                    var attribute = Attribute.GetCustomAttribute(TheAssembly, typeof(AssemblyCompanyAttribute));
                    if (attribute == null) return null;
                    companyName = ((AssemblyCompanyAttribute)attribute).Company;
                } //if
                return companyName;
            } //get CompanyName
        } //CompanyName
        public string ConfigurationName {
            get {
                if (configurationName == null) {
                    var attribute = Attribute.GetCustomAttribute(TheAssembly, typeof(AssemblyConfigurationAttribute));
                    if (attribute == null) return null;
                    configurationName = ((AssemblyConfigurationAttribute)attribute).Configuration;
                } //if
                return configurationName;
            } //get ConfigurationName
        } //ConfigurationName
        public string Title {
            get {
                if (title == null) {
                    var attribute = Attribute.GetCustomAttribute(TheAssembly, typeof(AssemblyTitleAttribute));
                    if (attribute == null) return null;
                    title = ((AssemblyTitleAttribute)attribute).Title;
                } //if
                return title;
            } //get Title
        } //Title
        public string ProductName {
            get {
                if (productName == null) {
                    var attribute = Attribute.GetCustomAttribute(TheAssembly, typeof(AssemblyProductAttribute));
                    if (attribute == null) return null;
                    productName = ((AssemblyProductAttribute)attribute).Product;
                } //if
                return productName;
            } //get ProductName
        } //ProductName
        public string AssemblyDescription {
            get {
                if (assemblyDescription == null) {
                    var attribute = Attribute.GetCustomAttribute(TheAssembly, typeof(AssemblyDescriptionAttribute));
                    if (attribute == null) return null;
                    assemblyDescription = ((AssemblyDescriptionAttribute)attribute).Description;
                } //if
                return assemblyDescription;
            } //get AssemblyDescription
        } //ProductName
        public string Copyright {
            get {
                if (copyright == null) {
                    var attribute = Attribute.GetCustomAttribute(TheAssembly, typeof(AssemblyCopyrightAttribute));
                    if (attribute == null) return null;
                    copyright = ((AssemblyCopyrightAttribute)attribute).Copyright;
                } //if
                return copyright;
            } //get Copyright
        } //Copyright
        public Version AssemblyFileVersion {
            get {
                if (assemblyFileVersion == null) {
                    var attribute = Attribute.GetCustomAttribute(TheAssembly, typeof(AssemblyFileVersionAttribute));
                    if (attribute == null) return null;
                    assemblyFileVersion = new Version(((AssemblyFileVersionAttribute)attribute).Version);
                } //if
                return assemblyFileVersion;
            } //get AssemblyFileVersion
        } //AssemblyFileVersion
        public string AssemblyInformationalVersion {
            get {
                if (assemblyInfomationalVersion == null) {
                    var attribute = Attribute.GetCustomAttribute(TheAssembly, typeof(AssemblyInformationalVersionAttribute));
                    if (attribute == null) return null;
                    assemblyInfomationalVersion = ((AssemblyInformationalVersionAttribute)attribute).InformationalVersion;
                } //if
                return assemblyInfomationalVersion;
            } //get AssemblyInformationalVersion
        } //AssemblyInformationalVersion
        public Version AssemblyVersion {
            // in .NET Code, .NET 5+ .csproj, defined by PropertyGroup > AssemblyVersion
            get {
                if (assemblyVersion == null)
                    assemblyVersion = TheAssembly.GetName().Version;
                return assemblyVersion;
            } // sic! not from AssemblyVersionAttribute!
        } //AssemblyVersion
        public string AssemblyConfiguration {
            get {
                if (assemblyConfiguration == null) {
                    var attribute = Attribute.GetCustomAttribute(TheAssembly, typeof(AssemblyConfigurationAttribute));
                    if (attribute == null) return null;
                    assemblyConfiguration = ((AssemblyConfigurationAttribute)attribute).Configuration;
                } //if
                return assemblyConfiguration;
            } //AssemblyConfiguratoin
        } //AssemblyConfiguration

        public string ExecutablePath {
            get {
                if (executablePath == null)
                    executablePath = Path.GetDirectoryName(TheAssembly.Location);
                return executablePath;
            }
        } //ExecutablePath

        Assembly TheAssembly {
            get {
                if (assembly == null)
                    assembly = Assembly.GetEntryAssembly();
                return assembly;
            } //get TheAssembly
        } //TheAssembly

        public static new AdvancedApplicationBase Current { get { return (AdvancedApplicationBase)Application.Current; } }

        bool startupComplete;
        Assembly assembly;
        string executablePath, productName, title, copyright, companyName, configurationName, assemblyDescription, assemblyConfiguration;
        Version assemblyVersion, assemblyFileVersion;
        string assemblyInfomationalVersion;

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
