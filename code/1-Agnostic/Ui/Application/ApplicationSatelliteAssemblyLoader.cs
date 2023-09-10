namespace SA.Agnostic.UI {
    using FlatResourceDictionary = System.Collections.Generic.Dictionary<string, object>;
    using FrameworkElement = System.Windows.FrameworkElement;
    using ResourceDictionary = System.Windows.ResourceDictionary;
    using Assembly = System.Reflection.Assembly;
    using CultureInfo = System.Globalization.CultureInfo;
    using Path = System.IO.Path;
    using Directory = System.IO.Directory;
    using EnumerationOptions = System.IO.EnumerationOptions;
    using ApplicationSatelliteAssemblyList = System.Collections.Generic.List<IApplicationSatelliteAssembly>;
    using CultureList = System.Collections.Generic.List<System.Globalization.CultureInfo>;
    using PluginLoader = PluginLoader<IApplicationSatelliteAssembly>;

    public static class ApplicationSatelliteAssemblyLoader {

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

        public static void Localize(CultureInfo culture, params FrameworkElement[] targets) {
            foreach (var target in targets)
                Localize(culture, target);
        } //Localize
        
        internal static void Localize(CultureInfo culture, ResourceDictionary targetDictionary, string typeName) {
            bool isApplication = typeName == null;
            FlatResourceDictionary targetFlatResourceDictionary = AdvancedApplicationBase.GetResources(targetDictionary);
            IApplicationSatelliteAssembly[] interfaceSet = Load(culture);
            if (interfaceSet == null) return;
            foreach (var interfaceImplementation in interfaceSet) {
                FlatResourceDictionary sourceFlatResourceDictionary = isApplication
                    ? AdvancedApplicationBase.GetResources(interfaceImplementation.ApplicationResources)
                    : AdvancedApplicationBase.GetResources(interfaceImplementation[typeName]);
                foreach (var pair in targetFlatResourceDictionary) {
                    if (pair.Value == null) continue;
                    if (sourceFlatResourceDictionary == null) continue;
                    if (!sourceFlatResourceDictionary.TryGetValue(pair.Key, out object sourceValue)) continue;
                    if (sourceValue == null) continue;
                    if (sourceValue.GetType() != pair.Value.GetType()) continue;
                    targetDictionary[pair.Key] = sourceValue;
                } //loop
            } //loop
            if (interfaceSet == null) return;
        } //Localize

        static void Localize(CultureInfo culture, FrameworkElement target) {
            if (target == null) return;
            Localize(culture, target.Resources, target.GetType().FullName);
        } //Localize

        static IApplicationSatelliteAssembly[] Load(CultureInfo culture) {
            string applicationFileName = Assembly.GetEntryAssembly().Location;
            string executableDirectory = Path.GetDirectoryName(applicationFileName);
            ApplicationSatelliteAssemblyList list = //exact name first;
                Load(executableDirectory, applicationFileName, culture.Name);
            if (list == null) //fallback one step:
                list = Load(executableDirectory, applicationFileName, culture.TwoLetterISOLanguageName);
            return list?.ToArray();
        } //Load

        static ApplicationSatelliteAssemblyList Load(string executableDirectory, string applicationFileName, string culture) {
            string satelliteDirectory = Path.Combine(executableDirectory, culture);
            if (!Directory.Exists(satelliteDirectory)) return null;
            var candidates = Directory.EnumerateFiles(
                satelliteDirectory,
                Path.GetFileNameWithoutExtension(applicationFileName) + DefinitionSet.suffixSatelliteAssemblyFile,
                EnumerationOptions); ;
            ApplicationSatelliteAssemblyList list = new();
            foreach (string candidate in candidates) {
                PluginLoader<IApplicationSatelliteAssembly> loader = new(candidate);
                if (loader.Assembly != null && loader.Instance == null) {
                    loader.Unload();
                    continue;
                } //if
                list.Add(loader.Instance);
            } //loop
            if (list.Count < 1) return null;
            return list;
        } //Load

        static EnumerationOptions EnumerationOptions {
            get => new EnumerationOptions() { IgnoreInaccessible = true, RecurseSubdirectories = false, ReturnSpecialDirectories = false };
        } //EnumerationOptions

    } //class ApplicationSatelliteAssemblyLoader

}
