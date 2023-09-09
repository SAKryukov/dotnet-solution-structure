namespace SA.Application.Main {

    static class DefinitionSet {

        internal static string FormatCulture(string name, string EnglishName, string nativeName) =>
            $"{name}: {EnglishName}, {nativeName}";

        /*
        internal static readonly string markEntryAssembly = char.ConvertFromUtf32(0x1F3DA);
        internal static readonly string markPluginAssembly = char.ConvertFromUtf32(0x1F50C);
        internal static string FormatSizeInformatin(double x, double y) =>
            $"{x} x {y}";

        internal static class AssemblyPropertySet {
            internal const string productName = "Product Name";
            internal const string title = "Title";
            internal const string assemblyDescription = "Assembly Description";
            internal const string copyright = "Copyright";
            internal const string companyName = "Company Name";
            internal const string assemblyVersion = "Assembly Version";
            internal const string assemblyFileVersion = "Assembly File Version";
            internal const string assemblyInformationalVersion = "Assembly Informational Version";
            internal const string assemblyConfiguration = "Assembly Configuration";
            internal const string assemblyAuthor = "Author";
            internal const string targetFrameworkName = "Target Framework";
            internal const string targetFrameworkDisplayName = "Target Framework Name";
            internal const string targetPlatformName = "Target Platform";
            internal const string supportedOSPlatformName = "Supported OS Platform";
        } //class AssemblyPropertySet

        internal static class DialogPropertySet {
            internal const string pluginDialogTitle = " Load Plugin";
            internal const string pluginDialogFilter = "Plugin files|Plugin*.dll";
            internal const string assemblyDialogTitle = " Load Assembly";
            internal const string assemblyDialogFilter = "Assembly files|*.dll";
            internal const string saveExceptionReportDialogTitle = " Save Exception Report";
            internal const string saveExceptionReportDialogFilter = "HTML files|*.html";
        } //class DialogPropertySet

        internal static class ExceptionReport {
            internal static string FormatReport(string time, string localTimeZone, string productName, string assembly, string file, string type, string messsage, string dump) =>
                $"<!doctype HTML><html lang=\"en-us\"><body><dl><dt>UTC time:</dt><dd>{time}</dd><dt>Local time zone:</dt><dd>{localTimeZone}</dd><dt>Product:<dt><dd>{productName}</dd><dt>Assembly:</dt><dd>{assembly}<dt><dt>File:</dt><dd>{file}<dt>Unhandled exception, type:<dt><dd>{type}</dd><td>Exception message:</dt><dd>{messsage}</dd></dl><pre>{dump}</pre></body></html>";
            internal static string FormatFilename(string time, string assemblyName) =>
                $"{time}.{assemblyName}.exception.html";
            internal static string FormatTimeFile(System.DateTime time) =>
                time.ToString("yyyy-MM-dd.HH-mm-ss");
            internal static string FormatTime(System.DateTime time) =>
                time.ToString("yyyy-MM-dd HH:mm:ss");
        } //class ExceptionReport
*/
    } //class DefinitionSet

}
