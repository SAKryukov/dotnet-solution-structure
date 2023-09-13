namespace SA.Test.CodeGeneration {
    using System.Windows;

    class Generator {

        void Execute(string filename, string namespaceName, string typeName) {
            ResourceDictionary dictionary = resourceCollector.Resources;
            generator.Generate(dictionary, filename, namespaceName, typeName);
        } //Execute

        static int Main() {
            Away.DefinitionSet.GetParameters(out string filename, out string namespaceName, out string typeName);
            new Generator().Execute(filename, namespaceName, typeName);
            return 0;
        } //Main

        readonly View.ResourceCollector resourceCollector = new();
        readonly Agnostic.UI.CodeGeneration.DictiionaryCodeGenerator generator = new();

    } //class Generator

}
