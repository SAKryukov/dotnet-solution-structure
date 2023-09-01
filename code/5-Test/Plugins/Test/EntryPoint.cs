namespace SA.Test.Plugin {
    using Assembly = System.Reflection.Assembly;
    using Path = System.IO.Path;
    using Directory = System.IO.Directory;
    using Console = System.Console;

    class Test : Semantic.IHost {

        Test() {
            string executablePath = Path.GetDirectoryName(assembly.Location);
            string[] files = Directory.GetFiles(executablePath, DefinitionSet.pluginFileSearchPattern);
            foreach(var file in files) {
                Agnostic.PluginLoader<Semantic.IPropertyPlugin> plugin = new(file);
                if (plugin.Instance == null) continue;
                Console.WriteLine(DefinitionSet.FormatPluginData(Path.GetFileName(file), plugin.Instance.DisplayName));
                plugin.Instance.DiscoverProperties(assembly, this);
            } //loop
            Console.WriteLine(DefinitionSet.goodbye);
            Console.ReadKey(true);
        } //Test

        void Semantic.IHost.Add(string property, string value) {
            Console.WriteLine(DefinitionSet.FormatHost(property, value));
        } //Semantic.IHost.Add

        Assembly assembly = Assembly.GetEntryAssembly();

        static void Main() {
            new Test();
        } //Main

    } //class Test

}
