namespace SA.Agnostic.UI {
    using System.Windows;
    using ResourceFinderDictionary = System.Collections.Generic.Dictionary<string, System.Windows.ResourceDictionary>;

    public class ApplicationSatelliteAssemblyPluginImplementationHelper : ResourceFinderDictionary {

        public ApplicationSatelliteAssemblyPluginImplementationHelper(FrameworkElement[] elements) {
            foreach (var element in elements) {
                if (element == null) continue;
                string key = element.GetType().FullName;
                if (ContainsKey(key)) continue;
                Add(key, element.Resources);
            } //loop
        } //ApplicationSatelliteAssemblyPluginImplementation

        public new ResourceDictionary this[string fullTypeName] {
            get {
                if (TryGetValue(fullTypeName, out ResourceDictionary value))
                    return value;
                else
                    return null;
            }
        } //this

    } //class ApplicationSatelliteAssemblyPluginImplementation

}


