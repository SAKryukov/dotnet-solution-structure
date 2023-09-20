namespace SA.Agnostic.UI.Markup {
    using System.Windows;
    using InstanceDictionary = System.Collections.Generic.Dictionary<System.Type, object>;

    public static class ResourseDictionaryUtility {

        class DataTypeProviderException : System.SystemException {
            internal DataTypeProviderException(string message) : base(message) { }
        } //class DataTypeProviderException

        public static T_REQUIRED FindObject<T_REQUIRED>(ResourceDictionary dictionary) where T_REQUIRED: new() {
            if (dictionary == null) return default;
            foreach (object key in dictionary.Keys)
                if (dictionary[key] is T_REQUIRED required)
                    return required;
            foreach (ResourceDictionary child in dictionary.MergedDictionaries)
                return FindObject<T_REQUIRED>(child);
            return default;
        } //FindObject

        public static T_REQUIRED GetObject<T_REQUIRED>(ResourceDictionary dictionary) where T_REQUIRED : new() =>
            (T_REQUIRED)dictionary?[typeof(T_REQUIRED)];

        public static InstanceDictionary CollectDictionary(ResourceDictionary dictionary) {
            static void CollectDictionary(ResourceDictionary dictionary, InstanceDictionary instanceDictionary) {
                foreach (object key in dictionary.Keys)
                    CollectWithKey(dictionary, key, instanceDictionary);
                foreach (ResourceDictionary child in dictionary.MergedDictionaries)
                    CollectDictionary(child, instanceDictionary);
            } //CollectDictionary
            static void CollectWithKey(ResourceDictionary dictionary, object key, InstanceDictionary instanceDictionary) {
                object @object = dictionary[key];
                if (@object == null) return;
                System.Type objectType = @object.GetType();
                if (instanceDictionary.ContainsKey(objectType)) return;
                instanceDictionary.Add(objectType, @object);
            } //CollectWithKey
            if (dictionary == null) return default;
            InstanceDictionary instanceDictionary = new();
            CollectDictionary(dictionary, instanceDictionary);
            return instanceDictionary;
        } //CollectDictionary

    } //ResourseDictionaryUtility

}
