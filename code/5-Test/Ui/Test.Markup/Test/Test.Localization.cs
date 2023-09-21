namespace SA.Test.Markup {
    using System.Windows;
    using ApplicationSatelliteAssemblyIndex = Agnostic.UI.ApplicationSatelliteAssemblyIndex;
    using CultureInfo = System.Globalization.CultureInfo;
    using Instance = Agnostic.UI.IApplicationSatelliteAssembly;
    using PlugInstanceList = System.Collections.Generic.List<Agnostic.UI.IApplicationSatelliteAssembly>; 

    static class TestLocalization {

        internal static void Localize(
            My.SingleObjectDataSource singleObjectDataSource,
            My.MultiObjectDataSource multiObjectDataSource,
            My.DuckTypedDataSource duckTypedDataSource,
            bool doLocalize) {
            PlugInstanceList list = new();
            CultureInfo[] cultures = ApplicationSatelliteAssemblyIndex.GetImplementedCultures(list);
            if (!doLocalize || cultures.Length < 1) return;
            Instance instance = list[0];
            multiObjectDataSource.Resources = instance[typeof(My.MultiObjectDataSource).Name];
            singleObjectDataSource.Resources = instance[typeof(My.SingleObjectDataSource).Name];
            duckTypedDataSource.Resources = instance.ApplicationResources;
        } //Localize

    } //TestLocalization

}
