/*
    Copyright (C) 2023-2025 by Sergey A Kryukov
    https://www.SAKryukov.org
    https://github.com/SAKryukov
*/

namespace SA.Agnostic.UI {

    static class DefinitionSet {

        internal const string suffixSatelliteAssemblyFile = ".resources.dll";

        internal const string maskAllFiles = "*";
        internal static System.Func<string, string> formatTitle = productName => $" {productName}";

        internal static string MaskResourceFile(string baseName) =>
            $"{baseName}{maskAllFiles + suffixSatelliteAssemblyFile}";

        internal static class ConsoleHelperUtility {
            internal const string showExit = "Done";
        } //class ConsoleHelperUtility

    } //class DefinitionSet

}
