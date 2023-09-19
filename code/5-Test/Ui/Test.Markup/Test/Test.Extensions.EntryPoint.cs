namespace SA.Test.Markup {
    using Console = System.Console;
    using System.Windows;
    using ResourseDictionaryUtility = Agnostic.UI.Markup.ResourseDictionaryUtility;

    static class Test {

        static void Execute() {
            My.DuckTypedDataSource duckTypedDataSource = new();
            My.DataSource DataSource = new();
            My.DuckTyped duck = new();
            ResourseDictionaryUtility.CollectForDuckTypedInstance(duckTypedDataSource.Resources, duck);
            Console.WriteLine(duck);
            My.Detail detail = new();
            ResourseDictionaryUtility.CollectForInstance(DataSource.Resources, detail);
            Console.WriteLine(detail);
            My.Fun fun = new();
            ResourseDictionaryUtility.CollectForInstance(DataSource.Resources, fun);
            Console.WriteLine(fun);
        } //Execute

        [System.STAThread]
        static void Main() {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Execute();
        } //Main

    } //class Test

}
