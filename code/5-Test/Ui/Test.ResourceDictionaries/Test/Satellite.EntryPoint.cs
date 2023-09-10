namespace SA.Test {
    using Console = System.Console;
    using System.Windows;
    using SnapshotDictionaryKeyValuePair = System.Collections.Generic.KeyValuePair<object, object>;
    using SnapshotDictionary = System.Collections.Generic.Dictionary<System.Windows.ResourceDictionary, System.Collections.Generic.KeyValuePair<object, object>>;

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
        internal static void SetValue(object key, object value, ResourceDictionary dictionary) {
            ResourceDictionary foundDictionary = FindKey(key, dictionary);
            if (foundDictionary == null) return;
            foundDictionary[key] = value;
        } //SetValue
        internal static bool HasNonRecursiveKey(object key, ResourceDictionary dictionary) {
            foreach (var dictionaryKey in dictionary.Keys)
                if (dictionaryKey == key)
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
            object found = MergeHelper.FindKey(keyToFind, top);
            SnapshotDictionary snapshot = new();
            SnapshotHelper.StoreInSnapshot(top, snapshot);
            Console.ReadKey(true);
        } //Execute

        static void Main() {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            (new Test()).Execute();
        } //Main

    } //class Test

}
