namespace SA.Agnostic {
    using Assembly = System.Reflection.Assembly;
    using AssemblyLoadContext = System.Runtime.Loader.AssemblyLoadContext;

    public class PluginLoader<INTERFACE> : PluginFinder<INTERFACE> where INTERFACE : IRecognizable {

        public PluginLoader(string assemblyPath) {
            try {
                Assembly assembly = assemblyLoadContext.LoadFromAssemblyPath(assemblyPath);
                Construct(assembly);
            } catch { //SA!!! sic! this is a rare case when the exception propagation should be blocked
                      // the lack if success is indicated by Instance == null
            } //exception
        } //PluginLoader

        public override void Unload() {
            assemblyLoadContext?.Unload();
        } //Unload

        readonly AssemblyLoadContext assemblyLoadContext = new(null, isCollectible: true);

    } //PluginLoader

}
