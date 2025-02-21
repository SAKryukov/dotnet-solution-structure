/*
    Copyright (C) 2023-2025 by Sergey A Kryukov
    https://www.SAKryukov.org
    https://github.com/SAKryukov
*/

namespace SA.Test.Plugin {

    static class DefinitionSet {

        internal const string pluginFileSearchPattern = "Plugin.*.dll";
        internal static readonly string goodbye = $"{System.Environment.NewLine}Press any key...";

        internal static string FormatPluginData(string filename, string displayName, string exploredAssemblyName) =>
            $"{displayName}, assembly file: {filename}{System.Environment.NewLine}Exploring assimbly: {exploredAssemblyName}";
        internal static string FormatHost(string name, string value) =>
            $"\t{name} => {value}";

    } //class DefinitionSet

}
