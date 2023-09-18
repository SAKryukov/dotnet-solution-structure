namespace SA.Test.Extensions {
    using Console = System.Console;
    using System.Windows;

    internal class ResourceTarget {
        public string Name = null;
        public string Description { get; set; }
        public int Count { get; set; }
    } //class ResourceTarget

    static class Test {

        static void Execute() {
            ResourceSource source = new();
            ResourceDictionary dictionary = source.Resources.MergedDictionaries[0];
            ResourceTarget target1 = new(), target2 = new();
            ResourseDictionaryUtility.CollectForInstance(dictionary, target1);
            dictionary = source.Resources.MergedDictionaries[1];
            ResourseDictionaryUtility.Collect(dictionary, target2);
        } //Execute

        [System.STAThread]
        static void Main() {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Execute();
        } //Main

    } //class Test

}
