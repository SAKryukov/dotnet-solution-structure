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

    public sealed class ApplicationSatelliteAssemblyLoader {

        public static void Localize(FrameworkElement target, CultureInfo currentCulture) {
            if (target == null) return;
            ResourceDictionary targetDictionary = target.Resources;
            FlatResourceDictionary targetFlatResourceDictionary = AdvancedApplicationBase.GetResources(targetDictionary);
            IApplicationSatelliteAssembly[] interfaceSet = Load(currentCulture);
            foreach (var interfaceImplementation in interfaceSet) {
                FlatResourceDictionary sourceFlatResourceDictionary =
                    AdvancedApplicationBase.GetResources(interfaceImplementation[target.GetType().FullName]);
                foreach(var pair in targetFlatResourceDictionary) {
                    if (pair.Value == null) continue;
                    if (!sourceFlatResourceDictionary.TryGetValue(pair.Key, out object sourceValue)) continue;
                    if (sourceValue == null) continue;
                    if (sourceValue.GetType() != pair.Value.GetType()) continue;
                    targetDictionary[pair.Key] = sourceValue;
                } //loop
            } //loop
            if (interfaceSet == null) return; 
        } //Localize

        static IApplicationSatelliteAssembly[] Load(CultureInfo currentCulture) {
            string applicationFileName = Assembly.GetEntryAssembly().Location;
            string executableDirectory = Path.GetDirectoryName(applicationFileName);
            ApplicationSatelliteAssemblyList list = //exact name first;
                Load(executableDirectory, applicationFileName, currentCulture.Name);
            if (list == null) //fallback one step:
                list = Load(executableDirectory, applicationFileName, currentCulture.TwoLetterISOLanguageName);
            return list.ToArray();
        } //Load

        static ApplicationSatelliteAssemblyList Load(string executableDirectory, string applicationFileName, string cultureName) {
            string satelliteDirectory = Path.Combine(executableDirectory, cultureName);
            if (!Directory.Exists(satelliteDirectory)) return null;
            var candidates = Directory.EnumerateFiles(
                satelliteDirectory,
                $"{Path.GetFileNameWithoutExtension(applicationFileName)}.resources.dll", //SA???
                new EnumerationOptions() { IgnoreInaccessible = true, RecurseSubdirectories = false, ReturnSpecialDirectories = false });
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

    } //class ApplicationSatelliteAssemblyLoader

}
