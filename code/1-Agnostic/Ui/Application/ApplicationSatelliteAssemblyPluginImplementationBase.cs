namespace SA.Agnostic.UI {
    using System.Windows;
    using ResourceFinderDictionary = System.Collections.Generic.Dictionary<string, System.Windows.ResourceDictionary>;

    public abstract class ApplicationSatelliteAssemblyPluginImplementationBase : ResourceFinderDictionary, IApplicationSatelliteAssembly {

        public ApplicationSatelliteAssemblyPluginImplementationBase() {
            applicationResourceDictionary = AddResources();
        } //ApplicationSatelliteAssemblyPluginImplementation

        // Should add the pairs { Name, ResourceDictionary } and return application ResourceDictionary:
        protected abstract ResourceDictionary AddResources(); 

        ResourceDictionary IApplicationSatelliteAssembly.this[string fullTypeName] {
            get {
                if (TryGetValue(fullTypeName, out ResourceDictionary value))
                    return value;
                else
                    return null;
            } //get IApplicationSatelliteAssembly.this
        } //IApplicationSatelliteAssembly.this
        ResourceDictionary IApplicationSatelliteAssembly.ApplicationResources => applicationResourceDictionary;

        ResourceDictionary applicationResourceDictionary;

    } //class ApplicationSatelliteAssemblyPluginImplementationBase

}
