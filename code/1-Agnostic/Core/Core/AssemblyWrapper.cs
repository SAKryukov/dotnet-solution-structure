namespace SA.Agnostic {
    using System.Reflection;
    using System.Runtime.Versioning;
    using Attribute = System.Attribute;
    using Path = System.IO.Path;
    using Version = System.Version;
    using MetadataDictionary = System.Collections.Generic.Dictionary<string, string>;

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

        public MetadataDictionary AssemblyMetadata {
            get {
                if (assemblyMetadata == null) {
                    assemblyMetadata = new();
                    Attribute[] attributes = Attribute.GetCustomAttributes(assembly, typeof(AssemblyMetadataAttribute));
                    if (attributes == null) return null;
                    if (attributes.Length < 1) return null;
                    for (int index = 0; index < attributes.Length; ++index) {
                        AssemblyMetadataAttribute assemblyMetadataAttribute = (AssemblyMetadataAttribute)attributes[index];
                        if (assemblyMetadata.ContainsKey(assemblyMetadataAttribute.Key))
                            assemblyMetadata[assemblyMetadataAttribute.Key] = assemblyMetadataAttribute.Value;
                        else
                            assemblyMetadata.Add(assemblyMetadataAttribute.Key, assemblyMetadataAttribute.Value);
                    } //loop                        
                } //if
                return assemblyMetadata;
            } //get AssemblyMetadata
        } //AssemblyMetadata

        public string[] Authors { //custom
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

        public string TargetFrameworkName {
            get {
                if (targetFrameworkName == null) {
                    var attribute = Attribute.GetCustomAttribute(assembly, typeof(TargetFrameworkAttribute));
                    if (attribute == null) return null;
                    targetFrameworkName = ((TargetFrameworkAttribute)attribute).FrameworkName;
                    targetFrameworkDisplayName = ((TargetFrameworkAttribute)attribute).FrameworkDisplayName;
                } //if
                return targetFrameworkName;
            } //get TargetFrameworkName
        } //TargetFrameworkName
        public string TargetFrameworkDisplayName {
            get {
                if (targetFrameworkDisplayName == null) {
                    var attribute = Attribute.GetCustomAttribute(assembly, typeof(TargetFrameworkAttribute));
                    if (attribute == null) return null;
                    targetFrameworkName = ((TargetFrameworkAttribute)attribute).FrameworkName;
                    targetFrameworkDisplayName = ((TargetFrameworkAttribute)attribute).FrameworkDisplayName;
                } //if
                return targetFrameworkDisplayName;
            } //get TargetFrameworkDisplayName
        } //TargetFrameworkDisplayName

        public string TargetPlatformName {
            get {
                if (targetPlatformName == null) {
                    var attribute = Attribute.GetCustomAttribute(assembly, typeof(TargetPlatformAttribute));
                    if (attribute == null) return null;
                    targetPlatformName = ((TargetPlatformAttribute)attribute).PlatformName;
                } //if
                return targetPlatformName;
            } //get TargetPlatformName
        } //TargetPlatformName

        public string SupportedOSPlatformName {
            get {
                if (supportedOSPlatformName == null) {
                    var attribute = Attribute.GetCustomAttribute(assembly, typeof(SupportedOSPlatformAttribute));
                    if (attribute == null) return null;
                    supportedOSPlatformName = ((SupportedOSPlatformAttribute)attribute).PlatformName;
                } //if
                return supportedOSPlatformName;
            } //get SupportedOSPlatformName
        } //SupportedOSPlatformName

        public string AssemblyDirectory {
            get {
                if (executablePath == null)
                    executablePath = Path.GetDirectoryName(assembly.Location);
                return executablePath;
            } //get AssemblyDirectory
        } //AssemblyDirectory

        public Assembly Assembly { get { return assembly; } }

        readonly Assembly assembly;
        string executablePath, productName, title, copyright, companyName, assemblyDescription, assemblyConfiguration;
        string targetFrameworkName, targetFrameworkDisplayName, targetPlatformName, supportedOSPlatformName;
        string[] authors;
        MetadataDictionary assemblyMetadata;
        Version assemblyVersion, assemblyFileVersion;
        string assemblyInfomationalVersion;

    } //class AssemblyWrapper

}
