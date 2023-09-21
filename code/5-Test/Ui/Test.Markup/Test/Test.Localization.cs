namespace SA.Test.Markup {
    using ResourseDictionaryUtility = Agnostic.UI.Markup.ResourseDictionaryUtility;
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
            if (!doLocalize || cultures.Length < 1) {
                ResourseDictionaryUtility.NormalizeDictionary(multiObjectDataSource.Resources);
                return;
            } //if
            System.Threading.Thread.CurrentThread.CurrentCulture = cultures[0];
            System.Threading.Thread.CurrentThread.CurrentUICulture = cultures[0];
            Instance instance = list[0];
            multiObjectDataSource.Resources = instance[typeof(My.MultiObjectDataSource).Name];
            singleObjectDataSource.Resources = instance[typeof(My.SingleObjectDataSource).Name];
            duckTypedDataSource.Resources = instance.ApplicationResources;
            ResourseDictionaryUtility.NormalizeDictionary(multiObjectDataSource.Resources);
        } //Localize

    } //TestLocalization

}
