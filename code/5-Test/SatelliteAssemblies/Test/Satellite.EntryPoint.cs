namespace SA.Test.Plugin {
    using Assembly = System.Reflection.Assembly;
    using Console = System.Console;
    using Directory = System.IO.Directory;
    using Path = System.IO.Path;
    using EnumerationOptions = System.IO.EnumerationOptions;
    using CultureInfo = System.Globalization.CultureInfo;
    using PluginLoader = Agnostic.PluginLoader<Agnostic.UI.IApplicationSatelliteAssembly>;

    class Test {

        void Execute() {
            EnumerationOptions enumerationOptions = new() { IgnoreInaccessible = true, RecurseSubdirectories = false, ReturnSpecialDirectories = false };
            var directories = Directory.EnumerateDirectories(
                Path.GetDirectoryName(Assembly.GetEntryAssembly().Location),
                "*",
                enumerationOptions);
            foreach (var directory in directories) {
                Console.WriteLine(directory);
                CultureInfo culture;
                try {
                    culture = new(Path.GetFileName(directory));
                } catch { continue; };
                if (culture == null) continue;
                Console.WriteLine($"\t{culture.Name}, {culture.NativeName}, {culture.EnglishName}, Types: {{{culture.CultureTypes}}}");
                var files = Directory.EnumerateFiles(
                    directory,
                    "*.resources.dll",
                    enumerationOptions);
                bool found = false;
                foreach (var file in files) {
                    using (PluginLoader loader = new(file)) {
                        if (loader.Instance != null) {
                            found = true;
                            break;
                        } else
                            continue;
                    } //using
                } //inner loops
                if (found)
                    Console.WriteLine($"Found plugins: {directory}");
            } //outer loop
            //System.Globalization.CultureInfo.GetCultureInfo("ru-US", true);
            //System.Globalization.CultureInfo culture = new("ru12-34");
            //Console.WriteLine($"{culture.Name}, {culture.NativeName}, {culture.EnglishName}");
            //Console.WriteLine(culture.TwoLetterISOLanguageName);
            //Console.WriteLine("end of test =====================================================================================================");
            Console.WriteLine(DefinitionSet.goodbye);
            Console.ReadKey(true);
        } //Execute

        static void Main() {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            (new Test()).Execute();
        } //Main

    } //class Test

}
