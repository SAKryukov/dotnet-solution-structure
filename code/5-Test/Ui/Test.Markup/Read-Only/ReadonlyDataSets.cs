/*
    Copyright (C) 2023 by Sergey A Kryukov
    https://www.SAKryukov.org
    https://github.com/SAKryukov
    https://www.codeproject.com/Members/SAKryukov
*/

namespace My {

    class ReadonlyDataSet {
        internal ReadonlyDataSet() { }
        public ReadonlyDataSet(string a, string b) { A = a; B = b; }
        internal string A { get; init; }
        internal string B { get; init; }
        internal string DuckTyped { get; private set; }
        public override string ToString() {
            (string name, string value) a = (nameof(A), A);
            (string name, string value) b = (nameof(B), B);
            (string name, string value) duck = (nameof(DuckTyped), DuckTyped);
            return DuckTyped == null
                ? DefinitionSet.Dump(GetType().Name, a, b)
                : DefinitionSet.Dump(GetType().Name, duck, a, b);
        } //ToString
    } //class ReadonlyDataSet

    class PseudoReadonlyDataSet {
        public PseudoReadonlyDataSet() { }
        public string C {
            get => c;
            set {
                if (c == null) c = value;
            }
        } //A
        public string D {
            get => d;
            set {
                if (d == null)
                    d = value;
                else
                    throw new ReadonlyViolationException(GetType(), nameof(D), value);
            }
        } //B
        string c, d;
        public override string ToString() {
            (string name, string value) c = (nameof(C), C);
            (string name, string value) d = (nameof(D), D);
            return DefinitionSet.Dump(GetType().Name, c, d);
        } //ToString
    } //class PseudoReadonlyDataSet

    class ReadonlyViolationException : System.ApplicationException {
        internal ReadonlyViolationException(System.Type type, string propertyName, object newValue) :
            base(@$"Attempt to assing a new value ""{newValue}"" to read-only property {type.FullName}.{propertyName}") { }
    } //class ReadonlyViolationException

}
