namespace SA.Agnostic.UI {
    using ResourceDictionary = System.Windows.ResourceDictionary;
    using Assembly = System.Reflection.Assembly;
    using CultureInfo = System.Globalization.CultureInfo;
    using Path = System.IO.Path;
    using Directory = System.IO.Directory;
    using EnumerationOptions = System.IO.EnumerationOptions;
    using ApplicationSatelliteAssemblyList = System.Collections.Generic.List<IApplicationSatelliteAssembly>;
    using SnapshotDictionaryKeyValuePair = System.Collections.Generic.KeyValuePair<object, object>;
    using SnapshotDictionary = System.Collections.Generic.Dictionary<System.Windows.ResourceDictionary, System.Collections.Generic.KeyValuePair<object, object>>;

    static class LocalizationContext {

        static class MergeHelper {
            internal static void SetValues(ResourceDictionary source, ResourceDictionary destination) {
                if (source == null || destination == null) return;
                for (int index = source.MergedDictionaries.Count - 1; index >= 0; --index) {
                    ResourceDictionary mergedDictionary = source.MergedDictionaries[index];
                    SetValues(mergedDictionary, destination);
                } //loop
                foreach (var key in source.Keys)
                    SetValue(key, source[key], destination);
            } //SetValues
            static void SetValue(object key, object value, ResourceDictionary dictionary) {
                ResourceDictionary foundDictionary = FindKey(key, dictionary);
                if (foundDictionary == null) return;
                foundDictionary[key] = value;
            } //SetValue
            static bool HasNonRecursiveKey(object key, ResourceDictionary dictionary) {
                foreach (var dictionaryKey in dictionary.Keys)
                    if (dictionaryKey.ToString() == key.ToString())
                        return true;
                return false;
            } //HasNonRecursiveKey
            static ResourceDictionary FindKey(object key, ResourceDictionary dictionary) {
                if (!dictionary.Contains(key)) return null;
                if (HasNonRecursiveKey(key, dictionary))
                    return dictionary;
                for (int index = dictionary.MergedDictionaries.Count - 1; index >= 0; --index) {
                    ResourceDictionary mergedDictionary = dictionary.MergedDictionaries[index];
                    if (HasNonRecursiveKey(key, mergedDictionary))
                        return mergedDictionary;
                } //loop
                return null;
            } //FindKey
        } //class MergeHelper
        static class SnapshotHelper {
            internal static void StoreInSnapshot(ResourceDictionary dictionary, SnapshotDictionary snapshot) {
                if (dictionary == null || snapshot == null) return;
                foreach (var key in dictionary.Keys)
                    snapshot.Add(dictionary, new SnapshotDictionaryKeyValuePair(key, dictionary[key]));
                foreach (var mergedDictionary in dictionary.MergedDictionaries)
                    StoreInSnapshot(mergedDictionary, snapshot);
            } //StoreInSnapshot
            internal static void RestoreFromSnapshot(ResourceDictionary dictionary, SnapshotDictionary snapshot) {
                if (dictionary == null || snapshot == null) return;
                foreach (var pair in snapshot)
                    pair.Key[pair.Value.Key] = pair.Value;
            } //RestoreFromSnapshot
        } //SnapshotHelper

        internal static void Localize(CultureInfo culture, ResourceDictionary targetDictionary, string typeName) {
            bool isApplication = typeName == null;
            IApplicationSatelliteAssembly[] interfaceSet = Load(culture);
            if (interfaceSet == null) return;
            foreach (var interfaceImplementation in interfaceSet) {
                ResourceDictionary source = isApplication
                    ? interfaceImplementation.ApplicationResources
                    : interfaceImplementation[typeName];
                MergeHelper.SetValues(source, targetDictionary);
            } //loop
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
                EnumerationOptions); 
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

        internal static bool SameCulture(CultureInfo left, CultureInfo right) =>
            string.Compare(left.Name, right.Name, System.StringComparison.InvariantCulture) == 0;

    } //class LocalizationContext

}
