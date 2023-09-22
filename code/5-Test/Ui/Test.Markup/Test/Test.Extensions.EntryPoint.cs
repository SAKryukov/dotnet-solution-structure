namespace SA.Test.Markup {
    using Console = System.Console;
    using ResourseDictionaryUtility = Agnostic.UI.Markup.ResourseDictionaryUtility;

    static class Test {

        static void Execute() {
            bool? localize = Agnostic.UI.ConsoleHelperUtility.RequestYesNoCancel(DefinitionSet.localizationRequest);
            if (localize == null) return;
            My.DuckTypedDataSource duckTypedDataSource = new();
            My.SingleObjectDataSource singleObjectDataSource = new();
            My.MultiObjectDataSource multiObjectDataSource = new();
            TestLocalization.Localize(singleObjectDataSource, multiObjectDataSource, duckTypedDataSource, localize == true);
            var main = ResourseDictionaryUtility.FindObject<My.Main>(singleObjectDataSource.Resources);
            var detail = ResourseDictionaryUtility.GetObject<My.Detail>(multiObjectDataSource.Resources);
            var funObject = ResourseDictionaryUtility.GetObject<My.Fun>(multiObjectDataSource.Resources);
            var duck = new My.DuckTypedSample();
            ResourseDictionaryUtility.CollectForDuckTypedInstance(duckTypedDataSource.Resources, duck, ignoreMissingMembers: false);
            Console.WriteLine(main);
            Console.WriteLine(detail);
            Console.WriteLine(funObject);
            Console.WriteLine(duck);
            Console.WriteLine();
            Console.WriteLine();
            var dictionary = ResourseDictionaryUtility.CollectDictionary(singleObjectDataSource.Resources);
            foreach (var pair in dictionary)
                Console.WriteLine(pair.Value);
            dictionary = ResourseDictionaryUtility.CollectDictionary(multiObjectDataSource.Resources);
            foreach (var pair in dictionary)
                Console.WriteLine(pair.Value);
            Agnostic.UI.ConsoleHelperUtility.ShowExit(showUnderDebugger: false);
        } //Execute

        [System.STAThread]
        static void Main() {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Execute();
        } //Main

    } //class Test

}
