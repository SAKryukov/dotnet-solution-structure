namespace SA.Test.CodeGeneration.Away {
    using Console = System.Console;
    using DisplayNameAttribute = Universal.Enumerations.DisplayNameAttribute;
    using DescriptionAttribute = Universal.Enumerations.DescriptionAttribute;
    using AbbreviationAttribute = Universal.Enumerations.AbbreviationAttribute;
    using CommandLine = Universal.Utilities.CommandLine<CommandLineOptionsBitset, CommandLineOptions>;

    enum CommandLineOptions {
        [DisplayName("output file name")]
        [Description("output C# file name")]
        [Abbreviation(1)]
        filename,
        [Description("namespace for generated code")]
        [DisplayName("generated namespace name")]
        [Abbreviation(1)]
        namespaceName,
        [DisplayName("class name")]
        [Description("name for the static class with generated declarations")]
        [Abbreviation(1)]
        typeName }
    enum CommandLineOptionsBitset {
        [Description("no console output")]
        [Abbreviation(1)]
        Quiet }

    static class DefinitionSet {

        static void ShowUsage(CommandLine commandLine) {
            if (commandLine[CommandLineOptionsBitset.Quiet]) return;
            Console.WriteLine($"Command-line usage of {System.Reflection.Assembly.GetEntryAssembly().Location}:");
            foreach (var item in commandLine.ValueEnumeration) {
                Console.WriteLine($"    -{item.Name}:<{item.DisplayName}>,");
                Console.WriteLine($"    -{item.AbbreviatedName}:<{item.DisplayName}>");
                Console.WriteLine($"            {item.Description}");
            } //loop
            foreach (var item in commandLine.SwitchEnumeration) {
                Console.WriteLine($"    -{item.Name},");
                Console.WriteLine($"    -{item.AbbreviatedName}");
                Console.WriteLine($"            {item.Name}: {item.Description}");
            } //loop
        } //ShowUsage

        internal static void GetParameters(out string filename, out string namespaceName, out string typeName) {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            CommandLine commandLine = new(Universal.Utilities.CommandLineParsingOptions.CaseInsensitive);
            ShowUsage(commandLine);
            filename = commandLine[CommandLineOptions.filename];
            namespaceName = commandLine[CommandLineOptions.namespaceName];
            typeName = commandLine[CommandLineOptions.typeName];
        } //GetParameters

    } //class DefinitionSet

}
