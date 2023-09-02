namespace SA.Agnostic {
    using System.Reflection;
    using Version = System.Version;
    using Attribute = System.Attribute;
    using Path = System.IO.Path;

    public class AssemblyWrapper : Assembly {

        public AssemblyWrapper(Assembly anAssembly) { assembly = anAssembly; }

        public string CompanyName {
            get {
                if (companyName == null) {
                    var attribute = Attribute.GetCustomAttribute(assembly, typeof(AssemblyCompanyAttribute));
                    if (attribute == null) return null;
                    companyName = ((AssemblyCompanyAttribute)attribute).Company;
                } //if
                return companyName;
            } //get CompanyName
        } //CompanyName

        /*
        public string ConfigurationName {
            get {
                if (configurationName == null) {
                    var attribute = Attribute.GetCustomAttribute(assembly, typeof(AssemblyConfigurationAttribute));
                    if (attribute == null) return null;
                    configurationName = ((AssemblyConfigurationAttribute)attribute).Configuration;
                } //if
                return configurationName;
            } //get ConfigurationName
        } //ConfigurationName
        */

        public string Title {
            get {
                if (title == null) {
                    var attribute = Attribute.GetCustomAttribute(assembly, typeof(AssemblyTitleAttribute));
                    if (attribute == null) return null;
                    title = ((AssemblyTitleAttribute)attribute).Title;
                } //if
                return title;
            } //get Title
        } //Title

        public string ProductName {
            get {
                if (productName == null) {
                    var attribute = Attribute.GetCustomAttribute(assembly, typeof(AssemblyProductAttribute));
                    if (attribute == null) return null;
                    productName = ((AssemblyProductAttribute)attribute).Product;
                } //if
                return productName;
            } //get ProductName
        } //ProductName

        public string AssemblyDescription {
            get {
                if (assemblyDescription == null) {
                    var attribute = Attribute.GetCustomAttribute(assembly, typeof(AssemblyDescriptionAttribute));
                    if (attribute == null) return null;
                    assemblyDescription = ((AssemblyDescriptionAttribute)attribute).Description;
                } //if
                return assemblyDescription;
            } //get AssemblyDescription
        } //AssemblyDescription

        public string Copyright {
            get {
                if (copyright == null) {
                    var attribute = Attribute.GetCustomAttribute(assembly, typeof(AssemblyCopyrightAttribute));
                    if (attribute == null) return null;
                    copyright = ((AssemblyCopyrightAttribute)attribute).Copyright;
                } //if
                return copyright;
            } //get Copyright
        } //Copyright

        public Version AssemblyFileVersion {
            get {
                if (assemblyFileVersion == null) {
                    var attribute = Attribute.GetCustomAttribute(assembly, typeof(AssemblyFileVersionAttribute));
                    if (attribute == null) return null;
                    assemblyFileVersion = new Version(((AssemblyFileVersionAttribute)attribute).Version);
                } //if
                return assemblyFileVersion;
            } //get AssemblyFileVersion
        } //AssemblyFileVersion

        public string AssemblyInformationalVersion {
            get {
                if (assemblyInfomationalVersion == null) {
                    var attribute = Attribute.GetCustomAttribute(assembly, typeof(AssemblyInformationalVersionAttribute));
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
                    assemblyVersion = assembly.GetName().Version;
                return assemblyVersion;
            } // sic! not from AssemblyVersionAttribute!
        } //AssemblyVersion

        public string AssemblyConfiguration {
            get {
                if (assemblyConfiguration == null) {
                    var attribute = Attribute.GetCustomAttribute(assembly, typeof(AssemblyConfigurationAttribute));
                    if (attribute == null) return null;
                    assemblyConfiguration = ((AssemblyConfigurationAttribute)attribute).Configuration;
                } //if
                return assemblyConfiguration;
            } //AssemblyConfiguratoin
        } //AssemblyConfiguration

        public string AssemblyDirectory {
            get {
                if (executablePath == null)
                    executablePath = Path.GetDirectoryName(assembly.Location);
                return executablePath;
            }
        } //AssemblyDirectory

        public Assembly Assembly { get { return assembly; } }

        readonly Assembly assembly;
        string executablePath, productName, title, copyright, companyName, assemblyDescription, assemblyConfiguration;
        Version assemblyVersion, assemblyFileVersion;
        string assemblyInfomationalVersion;

    } //class AssemblyWrapper

}
