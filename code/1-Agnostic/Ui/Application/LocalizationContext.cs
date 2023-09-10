namespace SA.Agnostic.UI {
    //using System.Windows;
    //using StringDictionary = System.Collections.Generic.Dictionary<string, object>;
    //using Thread = System.Threading.Thread;
    using CultureInfo = System.Globalization.CultureInfo;
    using ResourceDictionary = System.Windows.ResourceDictionary;
    using SnapshotDictionaryKeyValuePair = System.Collections.Generic.KeyValuePair<object, object>;
    using SnapshotDictionary = System.Collections.Generic.Dictionary<System.Windows.ResourceDictionary, System.Collections.Generic.KeyValuePair<object, object>>;
    using LocalizationContextDictionary = System.Collections.Generic.Dictionary<System.Windows.ResourceDictionary, System.Collections.Generic.Dictionary<string, object>>;

    class LocalizationContext : LocalizationContextDictionary {

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
                    if (dictionaryKey == key)
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

        internal static bool SameCulture(CultureInfo left, CultureInfo right) =>
            string.Compare(left.Name, right.Name, System.StringComparison.InvariantCulture) == 0;

    } //class LocalizationContext

}
