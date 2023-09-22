namespace SA.Agnostic.UI.CodeGeneration {
    using System.Windows;
    using StringList = System.Collections.Generic.List<string>;
    using File = System.IO.File;
    using Path = System.IO.Path;
    using KeySet = System.Collections.Generic.HashSet<string>;

    public class DictiionaryCodeGenerator {

        public void Generate(ResourceDictionary dictionary, string filename, string namespaceName, string typeName) {
            if (filename == null) return;
            destination.Clear();
            keySet.Clear();
            string dictionaryTypeName = dictionary.GetType().FullName;
            destination.Add(DefinitionSet.Comment.top);
            destination.Add(DefinitionSet.Comment.GeneratorLocation(System.Reflection.Assembly.GetEntryAssembly().Location));
            string dictionarySource = dictionary.Source == null
                ? DefinitionSet.Comment.nullRepresentation
                : dictionary.Source.ToString();
            destination.Add(DefinitionSet.Comment.DictionarySource(dictionarySource));
            destination.Add(string.Empty);
            string generatedNamespace = Path.GetFileNameWithoutExtension(GetType().FullName);
            string generatedClassName = DefinitionSet.Default.generatedClassName;
            if (!string.IsNullOrEmpty(namespaceName))
                generatedNamespace = namespaceName;
            if (!string.IsNullOrEmpty(typeName))
                generatedClassName = typeName;
            destination.Add(DefinitionSet.Content.NamespaceBra(generatedNamespace));
            destination.Add(string.Empty);
            destination.Add(DefinitionSet.Content.ClassBra(generatedClassName));
            FindAllKeys(dictionary);
            foreach (string key in keySet) {
                if (key is not string) continue;
                string valid = MakeValidIdentifier(key);
                object value = dictionary[key];
                System.Type type = value.GetType();
                destination.Add(DefinitionSet.Content.EntryDeclaration(type.FullName, valid, dictionaryTypeName));
                destination.Add(DefinitionSet.Content.EntryValue(type.FullName, key, value.ToString()));
                identifierSet.Add(valid);
            } //loop
            destination.Add(DefinitionSet.Content.ClassKet(generatedClassName));
            destination.Add(string.Empty);
            destination.Add(DefinitionSet.Content.namepaceKet);
            File.WriteAllLines(filename, destination.ToArray());
        } //Generate

        void FindAllKeys(ResourceDictionary dictionary) {
            foreach (object key in dictionary.Keys) {
                if (key is not string) continue;
                string stringKey = key.ToString();
                if (keySet.Contains(stringKey)) continue;
                keySet.Add(stringKey);
            }; //loop
            foreach (ResourceDictionary mergedDictionary in dictionary.MergedDictionaries)
                FindAllKeys(mergedDictionary);
        } //FindAllKeys

        string MakeValidIdentifier(string value) {
            stringBuilder.Clear();
            foreach (char letter in value)
                stringBuilder.Append(char.IsLetter(letter) ? letter : DefinitionSet.ValidIdentifier.nonLetterPlaceholder);
            string newValue = stringBuilder.ToString();
            if (newValue != value) {
                long index = 1;
                string uniqueValue = newValue;
                while (identifierSet.Contains(uniqueValue))
                    uniqueValue = DefinitionSet.ValidIdentifier.UniqueName(newValue, index++);
                newValue = uniqueValue;
            } //if
            return newValue;
        } //MakeValidIdentifier

        readonly System.Text.StringBuilder stringBuilder = new();
        readonly StringList destination = new();
        readonly KeySet keySet = new();
        readonly KeySet identifierSet = new();

    } //DictiionaryCodeGenerator

}
