[assembly: SA.Agnostic.PluginManifest(
    typeof(SA.Agnostic.UI.IApplicationSatelliteAssembly),
    typeof(SA.Plugin.Localization.Implementation))]

namespace SA.Plugin.Localization {
    using System.Windows;
    using IApplicationSatelliteAssembly = Agnostic.UI.IApplicationSatelliteAssembly;

    class Implementation : IApplicationSatelliteAssembly {

        ResourceDictionary IApplicationSatelliteAssembly.this[string fullTypeName] {
            get {
                if (fullTypeName == about.GetType().FullName) {
                    return about.Resources;
                } else if (fullTypeName == windowMain.GetType().FullName) {
                    return windowMain.Resources;
                } //if
                return null;
            }
        } //this

        SA.Application.View.WindowMain windowMain = new();
        SA.Application.View.About about = new();

    } //class Implementation

}