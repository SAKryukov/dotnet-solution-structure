namespace SA.Test.Extensions {
    using Console = System.Console;
    using System.Windows;
    using System.Windows.Media;

    internal class ResourceTarget {
        public Thickness Thickness { get; set; }
        public Color Color { get; set; }
        public string Name = null;
        public string Description { get; set; }
        public int Count { get; set; }
    } //class ResourceTarget
    
    internal class ResourceTarget2 {
        public Thickness Thickness2 = default;
        public string Name2 = null;
        public string Description2 { get; set; }
        public int Count2 { get; set; }
    } //class ResourceTarget

    static class Test {

        static void Execute() {
            ResourceSource source = new();
            ResourceDictionary dictionary = source.Resources.MergedDictionaries[0];
            ResourceTarget target1type1 = new(), target2type1 = new();
            ResourceTarget2 target1type2 = new();
            ResourseDictionaryUtility.CollectForInstance(dictionary, target1type1);
            ResourseDictionaryUtility.CollectForInstance(dictionary, target1type2);
            var result = ResourseDictionaryUtility.CollectDictionary(dictionary);
            dictionary = source.Resources.MergedDictionaries[1];
            ResourseDictionaryUtility.Collect(dictionary, target2type1);
            Console.WriteLine(result.Count);
        } //Execute

        [System.STAThread]
        static void Main() {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Execute();
        } //Main

    } //class Test

}
