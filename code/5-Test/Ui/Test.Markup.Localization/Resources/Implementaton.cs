[assembly: SA.Agnostic.PluginManifest(
    typeof(SA.Agnostic.UI.IApplicationSatelliteAssembly),
    typeof(SA.Test.Markup.Localization.Implementation))]

namespace SA.Test.Markup.Localization {
    using System.Windows;
    using IApplicationSatelliteAssembly = Agnostic.UI.IApplicationSatelliteAssembly;

    class Implementation : IApplicationSatelliteAssembly {

        ResourceDictionary IApplicationSatelliteAssembly.this[string fullTypeName] =>
            fullTypeName == nameof(My.DataSource) ? dataSource : duckTypedDataSource;

        ResourceDictionary IApplicationSatelliteAssembly.ApplicationResources => null;

        readonly ResourceDictionary dataSource = new My.DataSource().Resources;
        readonly ResourceDictionary duckTypedDataSource = new My.DuckTypedDataSource().Resources;

    } //class Implementation

}
