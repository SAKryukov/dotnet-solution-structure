/*
    Copyright (C) 2023 by Sergey A Kryukov
    https://www.SAKryukov.org
    https://github.com/SAKryukov
    https://www.codeproject.com/Members/SAKryukov
*/

namespace SA.Test.Plugin {

    static class DefinitionSet {

        internal const string maskAllFiles = "*";
        internal const string maskSatelliteAssemblyNames = "*.resources.dll";
        
        internal static readonly string goodbye = $"{System.Environment.NewLine}Press any key...";

        internal static string FormatCulture(string cultureName, string cultureNativeName, string cultureEnglishName, string cultureCultureTypes) =>
            $"\t{cultureName}, {cultureNativeName}, {cultureEnglishName}, Types: {{{cultureCultureTypes}}}";
        internal static string FormatFoundIn(string directory) =>
            $"Found plugins: {directory}";

    } //class DefinitionSet

}
