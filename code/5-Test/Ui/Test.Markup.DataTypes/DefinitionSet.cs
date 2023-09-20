namespace My {
    using StringList = System.Collections.Generic.List<string>;
    using Color = System.Windows.Media.Color;

    static class DefinitionSet {

        internal static string Dump(string typeName, params (string name, string value)[] data) {
            StringList list = new();
            foreach (var (name, value) in data)
                list.Add($"{name}: {value}");
            string result = string.Join(", ", list);
            return $"{typeName}:\n  {result}";
        } //Dump

        internal static string FormatColors(Color[] colors) {
            string array = string.Join(", ", System.Array.ConvertAll<Color, string>(colors, value => value.ToString()));
            return $"[ {array} ]";
        } //FormatColors

    } //class DefinitionSet

}
