namespace SA.Application {
    using INamedPlugin = Semantic.INamedPlugin;
    using IPropertyPlugin = Semantic.IPropertyPlugin;
    using PropertyPluginLoader = Agnostic.PluginLoader<Semantic.IPropertyPlugin>;
    using PluginSet = System.Collections.ObjectModel.ObservableCollection<PluginSetElement>;

    class PluginSetExtension : PluginSet {
        internal bool Add(string assemblyPath) {
            PropertyPluginLoader loader = new(assemblyPath);
            if (loader.Instance == null) return false;
            PluginSetElement element = new() { Plugin = loader.Instance };
            Items.Add(element);
            return true;
        } //Add
        internal void RemovePluginAt(int index) {
            this[index].Loader.Unload();
            RemoveAt(index);
        } //RemovePluginAt
    } //PluginSetExtension

    public class PluginSetElement {
        internal Agnostic.PluginFinderBase Loader { get; set; }
        internal INamedPlugin Plugin { get; set; }
        internal IPropertyPlugin PropertyPlugin { get { return Plugin == null ? null : Plugin as IPropertyPlugin; } }
        internal string Name { get { return Plugin?.DisplayName; } }
        public override string ToString() {
            return Name;
        } //ToString
    } //PluginSetElement

}