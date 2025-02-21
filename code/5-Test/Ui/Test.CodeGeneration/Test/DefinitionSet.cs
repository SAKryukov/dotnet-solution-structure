/*
    Copyright (C) 2023-2025 by Sergey A Kryukov
    https://www.SAKryukov.org
    https://github.com/SAKryukov
*/

namespace SA.Test.CodeGeneration.Away {
    using Console = System.Console;
    using DisplayNameAttribute = Agnostic.Enumerations.DisplayNameAttribute;
    using DescriptionAttribute = Agnostic.Enumerations.DescriptionAttribute;
    using AbbreviationAttribute = Agnostic.Enumerations.AbbreviationAttribute;
    using CommandLine = Agnostic.Utilities.CommandLine<CommandLineOptionsBitset, CommandLineOptions>;

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
        typeName
    }
    enum CommandLineOptionsBitset {
        [Description("no console output")]
        [Abbreviation(1)]
        Quiet
    }

    static class DefinitionSet {

        static void ShowUsage(CommandLine commandLine) {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
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

        internal static (string filename, string namespaceName, string typeName) GetParameters() {
            CommandLine commandLine = new(Agnostic.Utilities.CommandLineParsingOptions.CaseInsensitive);
            ShowUsage(commandLine);
            return (
                commandLine[CommandLineOptions.filename],
                commandLine[CommandLineOptions.namespaceName],
                commandLine[CommandLineOptions.typeName]);
        } //GetParameters

    } //class DefinitionSet

}
