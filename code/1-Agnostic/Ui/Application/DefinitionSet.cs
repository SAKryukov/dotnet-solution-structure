namespace SA.Agnostic.UI {

    static class DefinitionSet {

        internal const string suffixSatelliteAssemblyFile = ".resources.dll";

        internal const string maskAllFiles = "*";
        internal const string maskResourceFile = maskAllFiles + suffixSatelliteAssemblyFile;
        internal static System.Func<string, string> formatTitle = productName => $" {productName}";

    } //class DefinitionSet

}