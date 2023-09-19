namespace SA.Agnostic.UI {
    using Assembly = System.Reflection.Assembly;
    using CultureInfo = System.Globalization.CultureInfo;
    using Path = System.IO.Path;
    using Directory = System.IO.Directory;
    using EnumerationOptions = System.IO.EnumerationOptions;
    using CultureList = System.Collections.Generic.List<System.Globalization.CultureInfo>;
    using PluginLoader = PluginLoader<IApplicationSatelliteAssembly>;
    using PlugInstanceList = System.Collections.Generic.List<IApplicationSatelliteAssembly>;

    public static class ApplicationSatelliteAssemblyIndex {

        public static CultureInfo[] ImplementedCultures => GetImplementedCultures(null);

        public static CultureInfo[] GetImplementedCultures(PlugInstanceList pluginList) {
            string location = Assembly.GetEntryAssembly().Location;
            string directoryPath = Path.GetDirectoryName(location);
            string baseName = Path.GetFileNameWithoutExtension(location);
            var directories = Directory.EnumerateDirectories(
                directoryPath,
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
                    DefinitionSet.MaskResourceFile(baseName),
                    EnumerationOptions);
                bool found = false;
                foreach (var file in files) {
                    using PluginLoader loader = new(file);
                    if (loader.Instance != null) {
                        pluginList?.Add(loader.Instance);
                        found = true;
                        break;
                    } else
                        continue;
                    //using
                } //file loops
                if (found)
                    list.Add(culture);
            }; //directory loop
            return list.ToArray();
        } //GetImplementedCultures

        internal static EnumerationOptions EnumerationOptions {
            get => new() { IgnoreInaccessible = true, RecurseSubdirectories = false, ReturnSpecialDirectories = false };
        } //EnumerationOptions

    } //class ApplicationSatelliteAssemblyIndex

}
