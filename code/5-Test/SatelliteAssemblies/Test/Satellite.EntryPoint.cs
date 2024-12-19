/*
    Copyright (C) 2023-2024 by Sergey A Kryukov
    https://www.SAKryukov.org
    https://github.com/SAKryukov
*/

namespace SA.Test.Plugin {
    using Assembly = System.Reflection.Assembly;
    using Console = System.Console;
    using Directory = System.IO.Directory;
    using Path = System.IO.Path;
    using EnumerationOptions = System.IO.EnumerationOptions;
    using CultureInfo = System.Globalization.CultureInfo;
    using PluginLoader = Agnostic.PluginLoader<Agnostic.UI.IApplicationSatelliteAssembly>;

    class Test {

        static void Execute() {
            EnumerationOptions enumerationOptions = new() { IgnoreInaccessible = true, RecurseSubdirectories = false, ReturnSpecialDirectories = false };
            var directories = Directory.EnumerateDirectories(
                Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
                DefinitionSet.maskAllFiles,
                enumerationOptions);
            foreach (var directory in directories) {
                Console.WriteLine(directory);
                CultureInfo culture;
                try {
                    culture = new(Path.GetFileName(directory));
                } catch { continue; };
                if (culture == null) continue;
                Console.WriteLine(DefinitionSet.FormatCulture(
                    culture.Name,
                    culture.NativeName,
                    culture.EnglishName,
                    culture.CultureTypes.ToString()));
                var files = Directory.EnumerateFiles(
                    directory,
                    DefinitionSet.maskSatelliteAssemblyNames,
                    enumerationOptions);
                bool found = false;
                foreach (var file in files) {
                    using PluginLoader loader = new(file);
                    if (loader.Instance != null) {
                        found = true;
                        break;
                    } else
                        continue;
                } //inner loops
                if (found)
                    Console.WriteLine(DefinitionSet.FormatFoundIn(directory));
            } //outer loop
            Console.WriteLine(DefinitionSet.goodbye);
            Console.ReadKey(true);
        } //Execute

        [System.STAThread]
        static void Main() {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Test.Execute();
        } //Main

    } //class Test

}
