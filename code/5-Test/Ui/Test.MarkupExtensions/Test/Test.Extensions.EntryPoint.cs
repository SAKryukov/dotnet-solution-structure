namespace SA.Test.Extensions {
    using Console = System.Console;
    using System.Windows;
    using Declarations;

    internal class ResourceTarget {
        public string Name = null;
        public string Description { get; set; }
        public int Count { get; set; }
    } //class ResourceTarget

    static class Test {

        static void Execute() {
            ResourceSource source = new();
            ResourceDictionary dictionary = source.Resources;
            ResourceTarget target = new();
            Member.Collect(dictionary, target);
        } //Execute

        [System.STAThread]
        static void Main() {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Execute();
        } //Main

    } //class Test

}
