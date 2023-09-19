namespace My {
    using StringList = System.Collections.Generic.List<string>;

    static class DefinitionSet {

        internal static string Dump(string typeName, params (string name, string value)[] data) {
            StringList list = new();
            foreach (var (name, value) in data)
                list.Add($"{name}: {value}");
            string result = string.Join(", ", list);
            return $"{typeName}:\n  {result}"; 
        } //

    } //class DefinitionSet

}
