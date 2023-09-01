namespace SA.Test.Plugin {
    using Assembly = System.Reflection.Assembly;
    using Path = System.IO.Path;
    using Directory = System.IO.Directory;
    using Console = System.Console;

    class Test : Semantic.IHost {

        void Execute(bool entryAssembly = true) {
            string executablePath = Path.GetDirectoryName(assembly.Location);
            string[] files = Directory.GetFiles(executablePath, DefinitionSet.pluginFileSearchPattern);
            foreach(var file in files) {
                Agnostic.PluginLoader<Semantic.IPropertyPlugin> plugin = new(file);
                if (plugin.Instance == null) continue;
                Assembly exploredAssembly = entryAssembly ? assembly : plugin.Assembly;
                Console.WriteLine(DefinitionSet.FormatPluginData(Path.GetFileName(file), plugin.Instance.DisplayName, exploredAssembly.FullName));
                plugin.Instance.DiscoverProperties(exploredAssembly, this);
                plugin.Unload();
            } //loop
            Console.WriteLine(DefinitionSet.goodbye);
            Console.ReadKey(true);
        } //Execute

        void Semantic.IHost.Add(string property, string value) {
            Console.WriteLine(DefinitionSet.FormatHost(property, value));
        } //Semantic.IHost.Add

        readonly Assembly assembly = Assembly.GetEntryAssembly();

        static void Main(string[] comamndLine) {
            (new Test()).Execute(comamndLine.Length == 0);
        } //Main

    } //class Test

}
