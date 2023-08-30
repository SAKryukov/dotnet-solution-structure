namespace SA.Application {
    using Environment = System.Environment;

    static class DefinitionSet {
        internal static readonly string markEntryAssembly = char.ConvertFromUtf32(0x1F3DA);
        internal static readonly string markPluginAssembly = char.ConvertFromUtf32(0x1F50C);
        internal static string FormatExceptionForClipboard(string productName, string assemblyLocation, string exception) =>
            $"Unhandled exception{Environment.NewLine}{productName}{Environment.NewLine}{assemblyLocation}:{Environment.NewLine}{Environment.NewLine}{exception}";
        internal static readonly string dataGridToolTip = $"{markEntryAssembly}: Entry Assembly, {markPluginAssembly}: Plugin Assembly";
    } //class DefinitionSet

}
