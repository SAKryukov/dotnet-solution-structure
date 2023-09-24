/*
    Copyright (C) 2023 by Sergey A Kryukov
    https://www.SAKryukov.org
    https://github.com/SAKryukov
    https://www.codeproject.com/Members/SAKryukov
*/

namespace SA.Test.CodeGeneration {
    using System.Windows;

    class Generator {

        void Execute(string filename, string namespaceName, string typeName) {
            ResourceDictionary dictionary = resourceCollector.Resources;
            generator.Generate(dictionary, filename, namespaceName, typeName);
        } //Execute

        static int Main() {
            var (filename, namespaceName, typeName) = Away.DefinitionSet.GetParameters();
            new Generator().Execute(filename, namespaceName, typeName);
            return 0;
        } //Main

        readonly View.ResourceCollector resourceCollector = new();
        readonly Agnostic.UI.CodeGeneration.DictionaryCodeGenerator generator = new();

    } //class Generator

}
