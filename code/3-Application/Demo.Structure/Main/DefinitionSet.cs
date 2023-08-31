namespace SA.Application {
    using Environment = System.Environment;

    static class DefinitionSet {
        
        internal static readonly string markEntryAssembly = char.ConvertFromUtf32(0x1F3DA);
        internal static readonly string markPluginAssembly = char.ConvertFromUtf32(0x1F50C);
        internal static string FormatExceptionForClipboard(string productName, string assemblyLocation, string exception) =>
            $"Unhandled exception{Environment.NewLine}{productName}{Environment.NewLine}{assemblyLocation}:{Environment.NewLine}{Environment.NewLine}{exception}";
        internal static readonly string dataGridToolTip = $"{markEntryAssembly}: Entry Assembly, {markPluginAssembly}: Plugin Assembly";

        internal static class AssemblyPropertySet {
            internal const string productName = "Product Name";
            internal const string title = "Title";
            internal const string assemblyDescription = "Assembly Description";
            internal const string copyright = "Copyright";
            internal const string companyName = "Company Name";
            internal const string assemblyVersion = "Assembly Version";
            internal const string assemblyFileVersion = "Assembly File Version";
            internal const string assemblyInformationalVersion = "Assembly Informational Version";
        } //class AssemblyPropertySet

    } //class DefinitionSet

}
