[assembly: SA.Agnostic.PluginManifest(
    typeof(SA.Agnostic.UI.IApplicationSatelliteAssembly),
    typeof(SA.Plugin.Localization.Implementation))]

namespace SA.Plugin.Localization {
    using System.Windows;
    using IApplicationSatelliteAssembly = Agnostic.UI.IApplicationSatelliteAssembly;

    class Implementation : IApplicationSatelliteAssembly {

        ResourceDictionary IApplicationSatelliteAssembly.this[string fullTypeName] {
            get {
                if (fullTypeName == "SA.Application.View.About") {
                    return about.Resources;
                } else if (fullTypeName == "SA.Application.View.WindowMain") {
                    return windowMain.Resources;
                } //if
                return null;
            }
        } //this

        View.WindowMain windowMain = new();
        View.About about = new();

    } //class Implementation

}