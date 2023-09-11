namespace SA.Test {
    using Console = System.Console;
    using System.Windows;
    using ResourceDictionarySet = System.Collections.Generic.HashSet<System.Windows.ResourceDictionary>;
    using SnapshotDictionary = System.Collections.Generic.Dictionary<System.Windows.FrameworkElement, System.Windows.ResourceDictionary>;
    using FrameworkElementCollection = System.Collections.Generic.IEnumerable<System.Windows.FrameworkElement>;

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
        internal static ResourceDictionary FindKey(object key, ResourceDictionary dictionary) {
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
        internal static void DeepClone(ResourceDictionary source, ResourceDictionary target, ResourceDictionarySet resourceDictionarySet) {
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
            if (elements == null || snapshot == null) return;
            foreach (var element in elements) {
                ResourceDictionary target = new();
                DeepClone(element.Resources, target, resourceDictionarySet);
                System.Diagnostics.Debug.Assert(!snapshot.ContainsKey(element));
                snapshot.Add(element, target);
            } //loop
            DeepClone(application.Resources, applicationSnapshop, resourceDictionarySet);
        } //StoreInSnapshot
        internal static void RestoreFromSnapshot(ResourceDictionary dictionary, SnapshotDictionary snapshot, Application application, ResourceDictionary applicationSnapshop) {
            if (dictionary == null || snapshot == null) return;
            foreach (var pair in snapshot)
                MergeHelper.SetValues(pair.Value, pair.Key.Resources);
            MergeHelper.SetValues(applicationSnapshop, application.Resources);
        } //RestoreFromSnapshot
    } //SnapshotHelper

    class Test {

        void Execute() {
            string keyName = "name";
            string keyLevel = "level";
            string keyToFind = "found!";
            ResourceDictionary top = new(); top[keyName] = "top"; top[keyLevel] = 0;
            ResourceDictionary child1 = new(); child1[keyName] = "child1"; child1[keyLevel] = 1;
            ResourceDictionary child2 = new(); child2[keyName] = "child2"; child2[keyLevel] = 1;
            ResourceDictionary grandchild1 = new(); grandchild1[keyName] = "grandchild1"; grandchild1[keyLevel] = 2;
            ResourceDictionary grandchild2 = new(); grandchild2[keyName] = "grandchild2"; grandchild2[keyLevel] = 2;
            child1.MergedDictionaries.Add(grandchild1);
            child1.MergedDictionaries.Add(grandchild2);
            child2.MergedDictionaries.Add(grandchild1);
            child2.MergedDictionaries.Add(grandchild2);
            top.MergedDictionaries.Add(child1);
            top.MergedDictionaries.Add(child2);
            top[keyToFind] = $"{keyToFind} in Top";
            child1[keyToFind] = $"{keyToFind} in child1";
            grandchild2[keyToFind] = $"{keyToFind} in grandchild2";
            //object found = MergeHelper.FindKey(keyToFind, top);
            ResourceDictionary snapshot = new();
            SnapshotHelper.DeepClone(top, snapshot, null);
            Console.Write("Press any key...");
            Console.ReadKey(true);
        } //Execute

        static void Main() {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            (new Test()).Execute();
        } //Main

    } //class Test

}
