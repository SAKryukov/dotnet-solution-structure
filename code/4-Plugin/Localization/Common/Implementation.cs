[assembly: SA.Agnostic.PluginManifest(
    typeof(SA.Agnostic.UI.IApplicationSatelliteAssembly),
    typeof(SA.Plugin.Localization.Implementation))]

namespace SA.Plugin.Localization {
    using System.Windows;
    using IApplicationSatelliteAssembly = Agnostic.UI.IApplicationSatelliteAssembly;

    class Implementation : IApplicationSatelliteAssembly {

        Implementation() {
            helper = new(new FrameworkElement[] { windowMain, about });
        } //Implementation

        ResourceDictionary IApplicationSatelliteAssembly.this[string fullTypeName] {
            get => helper[fullTypeName];
        } //IApplicationSatelliteAssembly.this

        readonly Agnostic.UI.ApplicationSatelliteAssemblyPluginImplementationHelper helper;
        SA.Application.View.WindowMain windowMain = new();
        SA.Application.View.About about = new();

    } //class Implementation

}