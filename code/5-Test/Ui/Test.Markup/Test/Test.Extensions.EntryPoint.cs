namespace SA.Test.Markup {
    using Console = System.Console;
    using ResourseDictionaryUtility = Agnostic.UI.Markup.ResourseDictionaryUtility;

    static class Test {

        static void Execute() {
            bool? localize = Agnostic.UI.ConsoleHelperUtility.RequestYesNoCancel(DefinitionSet.localizationRequest);
            if (localize == null) return;
            My.DuckTypedDataSource duck = new();
            My.DataSource source = new();
            TestLocalization.Localize(source, duck, localize == true);
            var duckObject = ResourseDictionaryUtility.FindObject<My.Main>(duck.Resources);
            var detailObject = ResourseDictionaryUtility.GetObject<My.Detail>(source.Resources);
            var funObject = ResourseDictionaryUtility.GetObject<My.Fun>(source.Resources);
            Console.WriteLine(duckObject);
            Console.WriteLine(detailObject);
            Console.WriteLine(funObject);
            Agnostic.UI.ConsoleHelperUtility.ShowExit(showUnderDebugger: false);
        } //Execute

        [System.STAThread]
        static void Main() {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Execute();
        } //M

    } //class Test

}
