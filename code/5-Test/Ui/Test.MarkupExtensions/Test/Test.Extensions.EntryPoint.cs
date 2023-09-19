namespace SA.Test.Extensions {
    using Console = System.Console;
    using System.Windows;

    static class Test {

        static void Execute() {
            ResourceSource source = new();
            ResourceDictionary dictionary = source.Resources.MergedDictionaries[0];
            //------------------------------------------------------------------------------------
            foreach(var key in dictionary.Keys) {
                object value = dictionary[key];
                if (value is not Agnostic.UI.Extensions.TestParent testParent) continue;
                var children = testParent.Children;
                foreach(var child in children) {
                    Console.WriteLine(child.Descr);
                } //loop
            }
            //------------------------------------------------------------------------------------
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
