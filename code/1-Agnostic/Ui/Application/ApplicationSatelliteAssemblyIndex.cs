namespace SA.Agnostic.UI {
    using Assembly = System.Reflection.Assembly;
    using CultureInfo = System.Globalization.CultureInfo;
    using Path = System.IO.Path;
    using Directory = System.IO.Directory;
    using EnumerationOptions = System.IO.EnumerationOptions;
    using CultureList = System.Collections.Generic.List<System.Globalization.CultureInfo>;
    using PluginLoader = PluginLoader<IApplicationSatelliteAssembly>;

    public static class ApplicationSatelliteAssemblyIndex {

        public static CultureInfo[] ImplementedCultures {
            get {
                var directories = Directory.EnumerateDirectories(
                    Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
                    DefinitionSet.maskAllFiles,
                    EnumerationOptions);
                CultureList list = new();
                foreach (var directory in directories) {
                    CultureInfo culture;
                    try {
                        culture = new(Path.GetFileName(directory));
                    } catch { continue; };
                    if (culture == null) continue;
                    var files = Directory.EnumerateFiles(
                        directory,
                        DefinitionSet.maskResourceFile,
                        EnumerationOptions);
                    bool found = false;
                    foreach (var file in files) {
                        using (PluginLoader loader = new(file)) {
                            if (loader.Instance != null) {
                                found = true;
                                break;
                            } else
                                continue;
                        } //using
                    } //file loops
                    if (found)
                        list.Add(culture);
                }; //directory loop
                return list.ToArray();
            } //get ImplementedCultures
        } //ImplementedCultures

        static EnumerationOptions EnumerationOptions {
            get => new EnumerationOptions() { IgnoreInaccessible = true, RecurseSubdirectories = false, ReturnSpecialDirectories = false };
        } //EnumerationOptions

    } //class ApplicationSatelliteAssemblyIndex

}
