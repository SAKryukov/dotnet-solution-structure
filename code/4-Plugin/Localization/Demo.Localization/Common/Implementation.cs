/*
    Copyright (C) 2023-2025 by Sergey A Kryukov
    https://www.SAKryukov.org
    https://github.com/SAKryukov
*/

[assembly: SA.Agnostic.PluginManifest(
    typeof(SA.Agnostic.UI.IApplicationSatelliteAssembly),
    typeof(SA.Plugin.Localization.Implementation))]

namespace SA.Plugin.Localization {
    using System.Windows;
    using IApplicationSatelliteAssembly = Agnostic.UI.IApplicationSatelliteAssembly;

    class Implementation : Agnostic.UI.ApplicationSatelliteAssemblyPluginImplementationBase, IApplicationSatelliteAssembly {

        protected override ResourceDictionary AddResources() {
            SA.Application.View.WindowMain windowMain = new();
            Add(windowMain.GetType().FullName, windowMain.Resources);
            SA.Application.View.About about = new();
            Add(about.GetType().FullName, about.Resources);
            View.ApplicationResourceSource applicationResourceSource = new();
            return applicationResourceSource.Resources;
        } //AddResources

    } //class Implementation

}
