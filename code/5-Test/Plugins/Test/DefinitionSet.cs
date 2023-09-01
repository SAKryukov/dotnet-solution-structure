namespace SA.Test.Plugin {

    static class DefinitionSet {

        internal const string pluginFileSearchPattern = "Plugin.*.dll";

        internal const string goodbye = "Press any key...";

        internal static string FormatPluginData(string filename, string displayName) =>
            $"{displayName}, assembly file: {filename}";

        internal static string FormatHost(string name, string value) =>
            $"\t{name} => {value}";

    } //class DefinitionSet

}
