namespace SA.Test.Extensions {
    using Console = System.Console;
    using System.Windows;

    static class Test {

        static void Execute() {
            ResourceSource source = new();
            ResourceDictionary dictionary = source.Resources;
            ResourceTarget target1 = new();
            ResourceTarget targetUntyped = new();
            ResourceTarget2 target2 = new();
            ResourseDictionaryUtility.CollectForInstance(dictionary, target1);
            ResourseDictionaryUtility.CollectForInstance(dictionary, target2);
            ResourseDictionaryUtility.Collect(dictionary, targetUntyped);
            var result = ResourseDictionaryUtility.CollectDictionary(dictionary);
            Console.WriteLine(result.Count);
        } //Execute

        [System.STAThread]
        static void Main() {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Execute();
        } //Main

    } //class Test

}
