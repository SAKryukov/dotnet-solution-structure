namespace SA.Agnostic.UI.CodeGeneration {

    static class DefinitionSet {

        internal static class Default {
            internal const string generatedClassName = "DefinitionSet";
        } //class Default

        internal static class Comment {
            internal const string top = "// This is auto-generated code, generator:";
            internal static string GeneratorLocation(string location) =>
                $"// {location}";
            internal const string nullRepresentation = "null";
            internal static string DictionarySource(string dictionarySource) =>
                $"// Source: {dictionarySource}";
        } //Comment

        internal static class Content {
            internal static string NamespaceBra(string name) =>
                $"namespace {name} {{";
            internal static string ClassBra(string name) =>
                $"    static class {name} {{";
            internal static string EntryDeclaration(string typeFullName, string validIdentifier, string dictionaryTypeName) =>
                $"        internal static {typeFullName} {validIdentifier}({dictionaryTypeName} dictionary) =>";
            internal static string EntryValue(string typeFullName, string key, string value) =>
                $"            ({typeFullName})dictionary[\"{key}\"]; // {value}";
            internal static string ClassKet(string name) =>
                $"    }} //class {name}";
            internal const string namepaceKet = "}";
        } //Content

        internal static class ValidIdentifier {
            internal const char nonLetterPlaceholder = '_';
            internal static string UniqueName(string candidate, long index) =>
                $"{candidate}{index++}";
        } //ValidIdentifier

    } //class DefinitionSet

}
