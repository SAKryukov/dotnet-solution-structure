/*
    Copyright (C) 2023-2025 by Sergey A Kryukov
    https://www.SAKryukov.org
    https://github.com/SAKryukov
*/

namespace My {

    class DuckTypedSample {
        internal string NationalBird { get; set; }
        public static string NationalTree = null;
        internal int WordHeritageSites { get; set; }
        public string NationalAnimal = null;
        internal string Аnthem { get; set; }
        internal string[] CarMakes { get; set; }
        public override string ToString() {
            (string name, string value) bird = (nameof(NationalBird), NationalBird);
            (string name, string value) tree = (nameof(NationalTree), NationalTree);
            (string name, string value) animal = (nameof(NationalAnimal), NationalAnimal);
            (string name, string value) anthem = (nameof(Аnthem), Аnthem);
            (string name, string value) cars = (nameof(CarMakes), DefinitionSet.FormatStrings(CarMakes));
            (string name, string value) wordHeritageSites = (nameof(WordHeritageSites), WordHeritageSites.ToString());
            return DefinitionSet.Dump(GetType().Name, bird, cars, tree, animal, wordHeritageSites, anthem);
        } //ToString
    } //class DuckTypedSample

}
