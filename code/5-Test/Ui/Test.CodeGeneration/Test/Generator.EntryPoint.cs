namespace SA.Test.CodeGeneration {
    using System.Windows;
    using Console = System.Console;
    using StringList = System.Collections.Generic.List<string>;
    using File = System.IO.File;
    using Path = System.IO.Path;

    class Generator {

        void Execute(string filename) {
            ResourceDictionary dictionary = resourceCollector.Resources;
            if (dictionary.MergedDictionaries.Count > 0)
                dictionary = dictionary.MergedDictionaries[0];
            GenerateFileFromResourceDictionary(dictionary, filename);
        } //Execute

        void GenerateFileFromResourceDictionary(ResourceDictionary dictionary, string filename) {
            destination.Clear();
            string dictionaryTypeName = dictionary.GetType().FullName;
            destination.Add($"// This is auto-generated code, generator:");
            destination.Add($"// {System.Reflection.Assembly.GetEntryAssembly().Location}");
            destination.Add($"// Source: {dictionary.Source}");
            destination.Add(string.Empty);
            string generatedNamespace = Path.GetFileNameWithoutExtension(GetType().FullName);
            destination.Add($"namespace {generatedNamespace}.GeneratedResourceSet {{");
            destination.Add(string.Empty);
            destination.Add("    static class DefinitionSet {");
            foreach (var key in dictionary.Keys) {
                string valid = MakeValidIdentifier(key.ToString());
                object value = dictionary[key];
                System.Type type = value.GetType();
                destination.Add($"        internal static {type.FullName} {valid}({dictionaryTypeName} dictionary) =>");
                destination.Add($"            ({type.FullName})dictionary[\"{key}\"]; // {value}");
            } //loop
            destination.Add("    } //class DefinitionSet");
            destination.Add(string.Empty);
            destination.Add("}");
            File.WriteAllLines(filename, destination.ToArray());
        } //GenerateFileFromResourceDictionary

        string MakeValidIdentifier(string value) {
            stringBuilder.Clear();
            foreach (char letter in value)
                stringBuilder.Append(char.IsLetter(letter) ? letter : '_');
            return stringBuilder.ToString();
        } //MakeValidIdentifier

        static int Main(string[] args) {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            new Generator().Execute(args.Length > 0 ? args[0] : null);
            return 0;
        } //Main

        readonly System.Text.StringBuilder stringBuilder = new();
        readonly View.ResourceCollector resourceCollector = new();
        readonly StringList destination = new();

    } //class Generator

}
