namespace SA.Test.Markup {
    using System.Windows;
    using ApplicationSatelliteAssemblyIndex = Agnostic.UI.ApplicationSatelliteAssemblyIndex;
    using CultureInfo = System.Globalization.CultureInfo;
    using Instance = Agnostic.UI.IApplicationSatelliteAssembly;
    using PlugInstanceList = System.Collections.Generic.List<Agnostic.UI.IApplicationSatelliteAssembly>; 

    static class TestLocalization {

        internal static void Localize(My.MultiObjectDataSource dataSource, My.SingleObjectDataSource duckTypedDataSource, bool doLocalize) {
            PlugInstanceList list = new();
            CultureInfo[] cultures = ApplicationSatelliteAssemblyIndex.GetImplementedCultures(list);
            if (!doLocalize || cultures.Length < 1) return;
            Instance instance = list[0];
            dataSource.Resources = instance[typeof(My.MultiObjectDataSource).Name];
            duckTypedDataSource.Resources = instance[typeof(My.SingleObjectDataSource).Name];
        } //Localize

    } //TestLocalization

}
