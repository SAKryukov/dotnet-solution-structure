namespace SA.Test.Markup {
    using ApplicationSatelliteAssemblyIndex = Agnostic.UI.ApplicationSatelliteAssemblyIndex;
    using CultureInfo = System.Globalization.CultureInfo;
    using Instance = Agnostic.UI.IApplicationSatelliteAssembly;
    using PlugInstanceList = System.Collections.Generic.List<Agnostic.UI.IApplicationSatelliteAssembly>; 

    static class TestLocalization {

        internal static void Localize(My.DataSource dataSource, My.DuckTypedDataSource duckTypedDataSource, bool doLocalize) {
            PlugInstanceList list = new();
            CultureInfo[] cultures = ApplicationSatelliteAssemblyIndex.GetImplementedCultures(list);
            if (!doLocalize || cultures.Length < 1) return;
            Instance instance = list[0];
            dataSource.Resources = instance[typeof(My.DataSource).Name];
            duckTypedDataSource.Resources = instance[typeof(My.DuckTypedDataSource).Name];
        } //Localize

    } //TestLocalization

}
