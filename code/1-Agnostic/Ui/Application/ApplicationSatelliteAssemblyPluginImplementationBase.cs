namespace SA.Agnostic.UI {
    using System.Windows;
    using ResourceFinderDictionary = System.Collections.Generic.Dictionary<string, System.Windows.ResourceDictionary>;

    public abstract class ApplicationSatelliteAssemblyPluginImplementationBase : ResourceFinderDictionary, IApplicationSatelliteAssembly {

        public ApplicationSatelliteAssemblyPluginImplementationBase() {
            FrameworkElement[] elements = GetResourceSources();
            if (elements == null) return;
            foreach (var element in elements) {
                if (element == null) continue;
                string key = element.GetType().FullName;
                if (ContainsKey(key)) continue;
                Add(key, element.Resources);
            } //loop
        } //ApplicationSatelliteAssemblyPluginImplementation

        protected abstract FrameworkElement[] GetResourceSources();

        ResourceDictionary IApplicationSatelliteAssembly.this[string fullTypeName] {
            get {
                if (TryGetValue(fullTypeName, out ResourceDictionary value))
                    return value;
                else
                    return null;
            } //get IApplicationSatelliteAssembly.this
        } //IApplicationSatelliteAssembly.this

    } //class ApplicationSatelliteAssemblyPluginImplementationBase

}


