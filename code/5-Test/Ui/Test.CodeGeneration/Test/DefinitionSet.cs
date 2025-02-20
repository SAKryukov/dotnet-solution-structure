/*
    Copyright (C) 2023-2024 by Sergey A Kryukov
    https://www.SAKryukov.org
    https://github.com/SAKryukov
*/

namespace SA.Test.CodeGeneration.Away {
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
        typeName }
    enum CommandLineOptionsBitset {
        [Description("no console output")]
        [Abbreviation(1)]
        Quiet }

    static class DefinitionSet {

        static string Usage(CommandLine commandLine) {
            System.Text.StringBuilder builder = new();
            void WriteLine(string text, bool newLine = true) => builder.Append($"{text}{(newLine ? "\n" : string.Empty)}");
            if (commandLine[CommandLineOptionsBitset.Quiet]) return null;
            foreach (var item in commandLine.ValueEnumeration) {
                WriteLine($"    -{item.Name}:<{item.DisplayName}>,");
                WriteLine($"    -{item.AbbreviatedName}:<{item.DisplayName}>");
                WriteLine($"            {item.Description}");
            } //loop
            foreach (var item in commandLine.SwitchEnumeration) {
                WriteLine($"    -{item.Name},");
                WriteLine($"    -{item.AbbreviatedName}");
                WriteLine($"            {item.Name}: {item.Description}", newLine: false);
            } //loop
            return builder.ToString();
        } //ShowUsage

        internal static (string filename, string namespaceName, string typeName) GetParameters() {
            CommandLine commandLine = new(Agnostic.Utilities.CommandLineParsingOptions.CaseInsensitive);
            string usage = Usage(commandLine);
            if (usage != null)
                System.Windows.MessageBox.Show(usage, "Code Generator");
            return (
                commandLine[CommandLineOptions.filename],
                commandLine[CommandLineOptions.namespaceName],
                commandLine[CommandLineOptions.typeName]);
        } //GetParameters

    } //class DefinitionSet

}
