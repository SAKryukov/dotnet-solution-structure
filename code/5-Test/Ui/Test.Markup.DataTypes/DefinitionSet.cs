/*
    Copyright (C) 2023-2025 by Sergey A Kryukov
    https://www.SAKryukov.org
    https://github.com/SAKryukov
*/

namespace My {
    using StringList = System.Collections.Generic.List<string>;
    using Color = System.Windows.Media.Color;

    public static class DefinitionSet {

        public static string Dump(string typeName, params (string name, string value)[] data) {
            StringList list = new();
            foreach (var (name, value) in data)
                list.Add($"{name}: {value}");
            string result = string.Join(", ", list);
            return $"{typeName}:\n  {result}";
        } //Dump

        public static string FormatColors(Color[] colors) {
            if (colors == null || colors.Length < 1) return "[]";
            string array = string.Join(", ", System.Array.ConvertAll<Color, string>(colors, value => value.ToString()));
            return $"[ {array} ]";
        } //FormatColors

        public static string FormatStrings(string[] values) =>
            values == null || values.Length < 1 ? "[]" : $"[ {string.Join(", ", values)} ]";

    } //class DefinitionSet

}
