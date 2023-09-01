namespace SA.Application {
    using INamedPlugin = Semantic.INamedPlugin;
    using IPropertyPlugin = Semantic.IPropertyPlugin;
    using IUiPlugin = Semantic.IUiPlugin;
    using PropertyPluginLoader = Agnostic.PluginLoader<Semantic.IPropertyPlugin>;
    using UiPluginFinder = Agnostic.PluginFinder<Semantic.IUiPlugin>;
    using PluginSet = System.Collections.ObjectModel.ObservableCollection<PluginSetElement>;

    class PluginSetExtension : PluginSet {
        internal bool Add(string assemblyPath) {
            PluginSetElement element;
            PropertyPluginLoader loader = new(assemblyPath);
            if (loader.Instance == null && loader.Assembly == null)
                return false;
            else if (loader.Instance == null && loader.Assembly != null) {
                UiPluginFinder uiFinder = new(loader.Assembly);
                if (uiFinder.Instance == null)
                    return false;
                element = new() { Plugin = uiFinder.Instance, Classifier = PluginSetElementClassifier.Ui };
            } else
                element = new() { Plugin = loader.Instance, Classifier = PluginSetElementClassifier.Property };
            if (element != null)
                Items.Add(element);
            return true;
        } //Add
        internal void RemovePluginAt(int index) {
            this[index].Loader.Unload();
            RemoveAt(index);
        } //RemovePluginAt
    } //PluginSetExtension

    enum PluginSetElementClassifier { Ui, Property }

    public class PluginSetElement {
        internal Agnostic.PluginFinderBase Loader { get; set; }
        internal INamedPlugin Plugin { get; set; }
        internal IPropertyPlugin PropertyPlugin { get { return Plugin == null ? null : Plugin as IPropertyPlugin; } }
        internal string Name { get { return Plugin?.DisplayName; } }
        internal PluginSetElementClassifier Classifier { get; set; }
        public override string ToString() {
            return Name;
        } //ToString
    } //PluginSetElement

}