/*
    Copyright (C) 2004-2025 by Sergey A Kryukov
    http://www.SAKryukov.org
*/

namespace SA.Agnostic.UI {

    static class DefinitionSet {

        internal const string suffixSatelliteAssemblyFile = ".resources.dll";

        internal const string maskAllFiles = "*";
        internal static System.Func<string, string> formatTitle = productName => $" {productName}";

        internal static string MaskResourceFile(string baseName) =>
            $"{baseName}{maskAllFiles + suffixSatelliteAssemblyFile}";

    } //class DefinitionSet

}
