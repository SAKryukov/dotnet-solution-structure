namespace SA.Test.CodeGeneration {
    using System.Windows;
    using Console = System.Console;
    using DisplayNameAttribute = Universal.Enumerations.DisplayNameAttribute;
    using DescriptionAttribute = Universal.Enumerations.DescriptionAttribute;
    using AbbreviationAttribute = Universal.Enumerations.AbbreviationAttribute;
    using CommandLine = Universal.Utilities.CommandLine<CommandLineOptionsBitset, CommandLineOptions>;

    enum CommandLineOptions {
        [DisplayName("output file name")]
        [Description("Generated C# file name")]
        [Abbreviation(1)]
        filename,

        [Description("Namespace for generated code")]
        [DisplayName("namespace name")]
        [Abbreviation(1)]
        namespaceName,

        [DisplayName("class name")]
        [Description("Name for the static class with generated declarations")]
        [Abbreviation(1)]
        typeName }
    enum CommandLineOptionsBitset {[Abbreviation(1)] Quiet }

    class Generator {

        void Execute(string filename, string namespaceName, string typeName) {
            ResourceDictionary dictionary = resourceCollector.Resources;
            generator.Generate(dictionary, filename, namespaceName, typeName);
        } //Execute

        static void ShowUsage(CommandLine commandLine) {
            if (commandLine[CommandLineOptionsBitset.Quiet]) return;
            Console.WriteLine($"Command-line usage of {System.Reflection.Assembly.GetEntryAssembly().Location}:");
            foreach (var item in commandLine.ValueEnumeration) {
                Console.WriteLine($"    -{item.Name}:<{item.DisplayName}>,");
                Console.WriteLine($"    -{item.AbbreviatedName}:<{item.DisplayName}>");
                Console.WriteLine($"            {item.Description}");
            } //loop
        } //ShowUsage

        static int Main() {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            CommandLine commandLine = new(Universal.Utilities.CommandLineParsingOptions.CaseInsensitive);
            ShowUsage(commandLine);
            var filename = commandLine[CommandLineOptions.filename];
            var namespaceName = commandLine[CommandLineOptions.namespaceName];
            var typeName = commandLine[CommandLineOptions.typeName];
            new Generator().Execute(filename, namespaceName, typeName);
            return 0;
        } //Main

        readonly View.ResourceCollector resourceCollector = new();
        readonly Agnostic.UI.CodeGeneration.DictiionaryCodeGenerator generator = new();

    } //class Generator

}
