namespace SA.Test.Markup {
    using Console = System.Console;
    using ResourseDictionaryUtility = Agnostic.UI.Markup.ResourseDictionaryUtility;

    static class Test {

        static void Execute() {
            bool? localize = Agnostic.UI.ConsoleHelperUtility.RequestYesNoCancel(DefinitionSet.localizationRequest);
            if (localize == null) return;
            //My.DuckTypedDataSource ducduckduck = new();
            My.SingleObjectDataSource main = new();
            My.MultiObjectDataSource source = new();
            TestLocalization.Localize(source, main, localize == true);
            var duckObject = ResourseDictionaryUtility.FindObject<My.Main>(main.Resources);
            var detailObject = ResourseDictionaryUtility.GetObject<My.Detail>(source.Resources);
            var funObject = ResourseDictionaryUtility.GetObject<My.Fun>(source.Resources);
            Console.WriteLine(duckObject);
            Console.WriteLine(detailObject);
            Console.WriteLine(funObject);
            Console.WriteLine();
            Console.WriteLine();
            var dictionary = ResourseDictionaryUtility.CollectDictionary(main.Resources);
            foreach (var pair in dictionary)
                Console.WriteLine(pair.Value);
            Agnostic.UI.ConsoleHelperUtility.ShowExit(showUnderDebugger: false);
            dictionary = ResourseDictionaryUtility.CollectDictionary(source.Resources);
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
