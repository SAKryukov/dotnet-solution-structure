namespace SA.Agnostic.UI {
    using ResourceDictionary = System.Windows.ResourceDictionary;
    using Assembly = System.Reflection.Assembly;
    using CultureInfo = System.Globalization.CultureInfo;
    using Path = System.IO.Path;
    using Directory = System.IO.Directory;
    using ApplicationSatelliteAssemblyList = System.Collections.Generic.List<IApplicationSatelliteAssembly>;
    using ResourceDictionarySet = System.Collections.Generic.HashSet<System.Windows.ResourceDictionary>;
    using SnapshotDictionary = System.Collections.Generic.Dictionary<System.Windows.FrameworkElement, System.Windows.ResourceDictionary>;
    using FrameworkElement = System.Windows.FrameworkElement;
    using FrameworkElementCollection = System.Collections.Generic.IEnumerable<System.Windows.FrameworkElement>;
    using FrameworkElementList = System.Collections.Generic.List<System.Windows.FrameworkElement>;
    using Application = System.Windows.Application;
    using Debug = System.Diagnostics.Debug;

    public class LocalizationContext {

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
            static void DeepClone(ResourceDictionary source, ResourceDictionary target, ResourceDictionarySet resourceDictionarySet) {
                if (resourceDictionarySet == null)
                    resourceDictionarySet = new();
                foreach (var key in source.Keys)
                    target[key] = source[key];
                foreach (var mergedDictionary in source.MergedDictionaries) {
                    if (!resourceDictionarySet.Contains(mergedDictionary)) {
                        resourceDictionarySet.Add(mergedDictionary);
                        ResourceDictionary targetMergedDictionary = new();
                        target.MergedDictionaries.Add(targetMergedDictionary);
                        DeepClone(mergedDictionary, targetMergedDictionary, resourceDictionarySet);
                    } else
                        DeepClone(mergedDictionary, mergedDictionary, resourceDictionarySet);
                } //loop
            } //DeepClone
            internal static void StoreInSnapshot(FrameworkElementCollection elements, SnapshotDictionary snapshot, Application application, ResourceDictionary applicationSnapshop) {
                ResourceDictionarySet resourceDictionarySet = new();
                Debug.Assert(elements != null && snapshot != null);
                foreach (var element in elements) {
                    ResourceDictionary target = new();
                    DeepClone(element.Resources, target, resourceDictionarySet);
                    System.Diagnostics.Debug.Assert(!snapshot.ContainsKey(element));
                    snapshot.Add(element, target);
                } //loop
                DeepClone(application.Resources, applicationSnapshop, resourceDictionarySet);
            } //StoreInSnapshot
            internal static void RestoreFromSnapshot(ResourceDictionary dictionary, SnapshotDictionary snapshot, Application application, ResourceDictionary applicationSnapshop) {
                Debug.Assert(dictionary != null && snapshot != null);
                foreach (var pair in snapshot)
                    MergeHelper.SetValues(pair.Value, pair.Key.Resources);
                MergeHelper.SetValues(applicationSnapshop, application.Resources);
            } //RestoreFromSnapshot
        } //SnapshotHelper

        sealed class ApplicationSnapshot {
            internal ApplicationSnapshot(CultureInfo startupCulture) {
                this.startupCulture = startupCulture;
            } //CultureInfo startupCulture
            internal void Capture(Application application) {
                FrameworkElementList list = new();
                foreach (FrameworkElement window in application.Windows)
                    list.Add(window);
                SnapshotHelper.StoreInSnapshot(list, snapshotDictionary, application, applicationSnapshop);
            } //Capture
            internal void Restore(Application application) {
                FrameworkElementList list = new();
                foreach (FrameworkElement window in application.Windows)
                    list.Add(window);
                SnapshotHelper.RestoreFromSnapshot(application.Resources, snapshotDictionary, application, applicationSnapshop);
            } //Restore
            readonly CultureInfo startupCulture;
            internal CultureInfo StartupCulture => startupCulture;
            readonly SnapshotDictionary snapshotDictionary = new();
            readonly ResourceDictionary applicationSnapshop = new();
        } //class ApplicationSnapshot

        internal CultureInfo Localize(CultureInfo culture, Application application) {
            CultureInfo currentCulture = System.Threading.Thread.CurrentThread.CurrentUICulture;
            if (SameCulture(culture, currentCulture))
                return null;
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;
            System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
            if (applicationSnapshot == null) {
                applicationSnapshot = new(currentCulture);
                applicationSnapshot.Capture(application);
            } else if (SameCulture(culture, applicationSnapshot.StartupCulture)) {
                applicationSnapshot.Restore(application);
                return applicationSnapshot.StartupCulture;
            } //if
            bool result = false;
            result |= Localize(culture, application.Resources, null) != null;
            foreach (FrameworkElement window in application.Windows)
                result |= Localize(culture, window.Resources, window.GetType().FullName) != null;
            if (!result) {
                applicationSnapshot.Restore(application);
                return applicationSnapshot.StartupCulture;
            } //if
            return culture;
        } //Localize

        static CultureInfo Localize(CultureInfo culture, ResourceDictionary targetDictionary, string typeName) {
            bool isApplication = typeName == null;
            IApplicationSatelliteAssembly[] interfaceSet = Load(culture);
            if (interfaceSet == null || interfaceSet.Length < 1) return null;
            foreach (var interfaceImplementation in interfaceSet) {
                ResourceDictionary source = isApplication
                    ? interfaceImplementation.ApplicationResources
                    : interfaceImplementation[typeName];
                MergeHelper.SetValues(source, targetDictionary);
            } //loop
            return culture;
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
                ApplicationSatelliteAssemblyIndex.EnumerationOptions); 
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

        internal static bool SameCulture(CultureInfo left, CultureInfo right) =>
            string.Compare(left.Name, right.Name, System.StringComparison.InvariantCulture) == 0;

        ApplicationSnapshot applicationSnapshot;

    } //class LocalizationContext

}
