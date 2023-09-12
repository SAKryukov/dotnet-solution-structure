namespace SA.Test.CodeGeneration {
    using System.Windows;
    using Console = System.Console;
    using AbbreviationAttribute = Universal.Enumerations.AbbreviationAttribute;
    using CommandLine = Universal.Utilities.CommandLine<CommandLineOptionsBitset, CommandLineOptions>;
    enum CommandLineOptions {[Abbreviation(1)] filename, [Abbreviation(1)] namespaceName, [Abbreviation(1)] typeName }
    enum CommandLineOptionsBitset {[Abbreviation(1)] showHelp }

    class Generator {

        void Execute(string filename, string namespaceName, string typeName) {
            ResourceDictionary dictionary = resourceCollector.Resources;
            generator.Generate(dictionary, filename, namespaceName, typeName);
        } //Execute

        static int Main() {
            CommandLine commandLine = new();
            var filename = commandLine[CommandLineOptions.filename];
            var namespaceName = commandLine[CommandLineOptions.namespaceName];
            var typeName = commandLine[CommandLineOptions.typeName];
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            new Generator().Execute(filename, namespaceName, typeName);
            return 0;
        } //Main

        readonly View.ResourceCollector resourceCollector = new();
        readonly Agnostic.UI.CodeGeneration.DictiionaryCodeGenerator generator = new();

    } //class Generator

}
