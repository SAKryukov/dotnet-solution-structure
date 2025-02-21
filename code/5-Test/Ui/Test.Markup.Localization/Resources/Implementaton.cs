/*
    Copyright (C) 2023-2025 by Sergey A Kryukov
    https://www.SAKryukov.org
    https://github.com/SAKryukov
*/

[assembly: SA.Agnostic.PluginManifest(
    typeof(SA.Agnostic.UI.IApplicationSatelliteAssembly),
    typeof(SA.Test.Markup.Localization.Implementation))]

namespace SA.Test.Markup.Localization {
    using System.Windows;
    using IApplicationSatelliteAssembly = Agnostic.UI.IApplicationSatelliteAssembly;

    class Implementation : IApplicationSatelliteAssembly {

        ResourceDictionary IApplicationSatelliteAssembly.this[string fullTypeName] =>
            fullTypeName == nameof(My.MultiObjectDataSource) ? multiObjectDataSource : singleObjectDataSource;

        ResourceDictionary IApplicationSatelliteAssembly.ApplicationResources => duckTypedDataSource;

        readonly ResourceDictionary multiObjectDataSource = new My.MultiObjectDataSource().Resources;
        readonly ResourceDictionary singleObjectDataSource = new My.SingleObjectDataSource().Resources;
        readonly ResourceDictionary duckTypedDataSource = new My.DuckTypedDataSource().Resources;

    } //class Implementation

}
