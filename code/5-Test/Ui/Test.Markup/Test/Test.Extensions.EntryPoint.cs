namespace SA.Test.Markup {
    using Console = System.Console;
    using System.Windows;
    using ResourseDictionaryUtility = Agnostic.UI.Markup.ResourseDictionaryUtility;

    static class Test {

        static void Execute() {
            ResourceSource source = new();
            ResourceDictionary dictionary = source.Resources;
            ResourceTarget target1 = new();
            ResourceTarget targetUntyped = new();
            ResourceTarget2 target2 = new();
            ResourseDictionaryUtility.CollectForInstance(dictionary, target1);
            ResourseDictionaryUtility.CollectForInstance(dictionary, target2);
            ResourseDictionaryUtility.CollectForDuckTypedInstance(dictionary, targetUntyped);
            var result = ResourseDictionaryUtility.CollectDictionary(dictionary);
            Console.WriteLine(result.Count);
            Console.WriteLine($"{target1}");
            Console.WriteLine($"{targetUntyped}");
        } //Execute

        [System.STAThread]
        static void Main() {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Execute();
        } //Main

    } //class Test

}
