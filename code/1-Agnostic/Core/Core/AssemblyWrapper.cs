namespace SA.Agnostic {
    using System.Reflection;
    using Attribute = System.Attribute;
    using Path = System.IO.Path;
    using Version = System.Version;

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
            } // get AssemblyVersion sic! not from AssemblyVersionAttribute!
        } //AssemblyVersion

        public string AssemblyConfiguration {
            get {
                if (assemblyConfiguration == null) {
                    var attribute = Attribute.GetCustomAttribute(assembly, typeof(AssemblyConfigurationAttribute));
                    if (attribute == null) return null;
                    assemblyConfiguration = ((AssemblyConfigurationAttribute)attribute).Configuration;
                } //if
                return assemblyConfiguration;
            } //get AssemblyConfiguratoin
        } //AssemblyConfiguration

        public string[] Authors {
            get {
                if (authors == null) {
                    Attribute[] attributes = Attribute.GetCustomAttributes(assembly, typeof(AuthorAttribute));
                    if (attributes == null) return null;
                    if (attributes.Length < 1) return null;
                    authors = new string[attributes.Length];
                    for (int index = 0; index < attributes.Length; ++index)
                        authors[index] = ((AuthorAttribute)attributes[index]).Author;
                } //if
                return authors;
            } //get Authors
        } //Authors

        public string AuthorList { //custom
            get {
                if (authorList == null) {
                    var attribute = Attribute.GetCustomAttribute(assembly, typeof(AuthorListAttribute));
                    if (attribute == null) return null;
                    authorList = ((AuthorListAttribute)attribute).AuthorList;
                } //if
                return authorList;
            } //get AssemblyConfiguratoin
        } //AuthorList

        public string AssemblyDirectory { //custom
            get {
                if (executablePath == null)
                    executablePath = Path.GetDirectoryName(assembly.Location);
                return executablePath;
            } //get AssemblyDirectory
        } //AssemblyDirectory

        public Assembly Assembly { get { return assembly; } }

        readonly Assembly assembly;
        string executablePath, productName, title, copyright, companyName, assemblyDescription, assemblyConfiguration;
        string[] authors;
        string authorList;
        Version assemblyVersion, assemblyFileVersion;
        string assemblyInfomationalVersion;

    } //class AssemblyWrapper

}
