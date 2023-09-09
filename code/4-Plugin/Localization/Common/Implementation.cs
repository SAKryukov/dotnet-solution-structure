[assembly: SA.Agnostic.PluginManifest(
    typeof(SA.Agnostic.UI.IApplicationSatelliteAssembly),
    typeof(SA.Plugin.Localization.Implementation))]

namespace SA.Plugin.Localization {
    using System.Windows;
    using IApplicationSatelliteAssembly = Agnostic.UI.IApplicationSatelliteAssembly;

    class Implementation : Agnostic.UI.ApplicationSatelliteAssemblyPluginImplementationBase, IApplicationSatelliteAssembly {

        protected override FrameworkElement[] GetResourceSources() {
            return new FrameworkElement[] { windowMain, about };
        } //GetResourceSources()

        SA.Application.View.WindowMain windowMain = new();
        SA.Application.View.About about = new();

    } //class Implementation

}