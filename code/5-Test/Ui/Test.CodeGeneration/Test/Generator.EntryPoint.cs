namespace SA.Test.CodeGeneration {
    using System.Windows;
    using Console = System.Console;
    using StringList = System.Collections.Generic.List<string>;
    using File = System.IO.File;
    using Path = System.IO.Path;
    using AbbreviationAttribute = Universal.Enumerations.AbbreviationAttribute;
    using CommandLine = Universal.Utilities.CommandLine<CommandLineOptionsBitset, CommandLineOptions>;

    enum CommandLineOptions { [Abbreviation(1)] filename, [Abbreviation(1)] namespaceName, [Abbreviation(1)] typeName }
    enum CommandLineOptionsBitset {[Abbreviation(1)] showHelp }

    class Generator {

        void Execute(string filename, string namespaceName, string typeName) {
            ResourceDictionary dictionary = resourceCollector.Resources;
            if (dictionary.MergedDictionaries.Count > 0)
                dictionary = dictionary.MergedDictionaries[0];
            GenerateFileFromResourceDictionary(dictionary, filename, namespaceName, typeName);
        } //Execute

        void GenerateFileFromResourceDictionary(ResourceDictionary dictionary, string filename, string namespaceName, string typeName) {
            if (filename == null) return;
            destination.Clear();
            string dictionaryTypeName = dictionary.GetType().FullName;
            destination.Add($"// This is auto-generated code, generator:");
            destination.Add($"// {System.Reflection.Assembly.GetEntryAssembly().Location}");
            destination.Add($"// Source: {dictionary.Source}");
            destination.Add(string.Empty);
            string generatedNamespace = Path.GetFileNameWithoutExtension(GetType().FullName);
            string generatedClassName = "DefinitionSet";
            if (namespaceName != null)
                generatedNamespace = namespaceName;
            if (typeName != null)
                generatedClassName = typeName;
            destination.Add($"namespace {generatedNamespace}.GeneratedResourceSet {{");
            destination.Add(string.Empty);
            destination.Add($"    static class {generatedClassName} {{");
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

        static int Main() {
            CommandLine commandLine = new();
            var filename = commandLine[CommandLineOptions.filename];
            var namespaceName = commandLine[CommandLineOptions.namespaceName];
            var typeName = commandLine[CommandLineOptions.typeName];
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            new Generator().Execute(filename, namespaceName, typeName);
            return 0;
        } //Main

        readonly System.Text.StringBuilder stringBuilder = new();
        readonly View.ResourceCollector resourceCollector = new();
        readonly StringList destination = new();

    } //class Generator

}
