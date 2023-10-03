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
    } //class ReadonlyDataSet

    class PseudoReadonlyDataSet {
        public PseudoReadonlyDataSet() { }
        public string A {
            get => a;
            set {
                if (a == null) a = value;
            }
        } //A
        public string B {
            get => b;
            set {
                if (b == null)
                    b = value;
                else
                    throw new ReadonlyViolationException(GetType(), nameof(B));
            }
        } //B
        string a, b;
    } //class PseudoReadonlyDataSet

    class ReadonlyViolationException : System.ApplicationException {
        internal ReadonlyViolationException(System.Type type, string propertyName) :
            base($"Attempt to assing a value to read-only property {type.FullName}.propertyName") { }
    } //class ReadonlyViolationException

}
