namespace SA.Agnostic {
    using Assembly = System.Reflection.Assembly;
    using AssemblyLoadContext = System.Runtime.Loader.AssemblyLoadContext;

    public class PluginLoader<INTERFACE> : PluginFinder<INTERFACE> where INTERFACE : IRecognizable {

        public PluginLoader(string assemblyPath) {
            Assembly assembly = assemblyLoadContext.LoadFromAssemblyPath(assemblyPath);
            Construct(assembly);
        } //PluginLoader

        public override void Unload() {
            assemblyLoadContext?.Unload();
        } //Unload

        readonly AssemblyLoadContext assemblyLoadContext = new(null, isCollectible: true);

    } //PluginLoader

}
