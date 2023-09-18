namespace SA.Test.Extensions {
    using Console = System.Console;
    using System;
    using System.Windows;
    using ResourceDictionarySet = System.Collections.Generic.HashSet<System.Windows.ResourceDictionary>;
    using SnapshotDictionary = System.Collections.Generic.Dictionary<System.Windows.FrameworkElement, System.Windows.ResourceDictionary>;
    using FrameworkElementCollection = System.Collections.Generic.IEnumerable<System.Windows.FrameworkElement>;
    using System.Windows.Markup;


    static class Test {

        /*
        static void Raise(string message) {
            throw new SystemException(message);
        }
        */

        static void Execute() {
            //ResourceSource source = new();
            //ResourceDictionary dictionary = source.Resources;
            //foreach (var key in dictionary.Keys) {
                //object value = dictionary[key];
                //if (value is not Declarations.ResourceSource resourceSource) continue;
                //foreach(var setter in resourceSource.Setters) {
                    //if (setter.MemberType == null)
                    //    Raise("Setter cannot be null");
                //} //setters loop
            //} //keys loop
        } //Execute

        [System.STAThread]
        static void Main() {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Execute();
        } //Main

    } //class Test

}
