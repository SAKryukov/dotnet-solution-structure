namespace SA.Test.Markup {
    using Console = System.Console;
    using ResourseDictionaryUtility = Agnostic.UI.Markup.ResourseDictionaryUtility;

    static class Test {

        static void Execute(bool localize) {
            My.DuckTypedDataSource duckTypedDataSource = new();
            My.DataSource dataSource = new();
            My.DuckTyped duck = new();
            TestLocalization.Localize(dataSource, duckTypedDataSource, localize);
            ResourseDictionaryUtility.CollectForDuckTypedInstance(duckTypedDataSource.Resources, duck);
            Console.WriteLine(duck);
            My.Detail detail = new();
            ResourseDictionaryUtility.CollectForInstance(dataSource.Resources, detail);
            Console.WriteLine(detail);
            My.Fun fun = new();
            ResourseDictionaryUtility.CollectForInstance(dataSource.Resources, fun);
            Console.WriteLine(fun);
            Agnostic.UI.ConsoleHelperUtility.ShowExit();
        } //Execute

        [System.STAThread]
        static void Main(string[] args) {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Execute(args.Length > 0);
        } //Main

    } //class Test

}
