/*
    Copyright (C) 2023-2024 by Sergey A Kryukov
    https://www.SAKryukov.org
    https://github.com/SAKryukov
*/

namespace My {
    using System.Diagnostics;
    using System.Reflection;

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

    class SimplerReadonlyDataSet {
        internal SimplerReadonlyDataSet() { }
        internal string A { get; private set; }
        internal string B { get; private set; }
        // unused, for the article only
    } //class SimplerReadonlyDataSet

    class PseudoReadonlyDataSet {
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

    abstract class StackTraceValidator {
        private protected void Validate(string propertyName, object newValue) {
            StackTrace stackTrace = new();
            int count = stackTrace.FrameCount;
            for (int level = 0; level < count; level++) {
                StackFrame frame = stackTrace.GetFrame(level);
                MethodBase method = frame.GetMethod();
                System.Type declaringType = method.DeclaringType;
                if (!declaringType.IsAssignableTo(typeof(StackTraceValidator))) {
                        if (declaringType.Assembly != typeof(System.IntPtr).Assembly)
                        throw new ReadonlyViolationException(GetType(), propertyName, newValue);
                    break;
                }
            } //loop
        } //Validate
    } //class StackTraceValidator

    class PseudoReadonlyDataSetXamlOnly : StackTraceValidator {
        public string E {
            get => e;
            set {
                if (e == null) e = value;
            }
        } //A
        public string F {
            get => f;
            set {
                Validate(nameof(F), value);
                f = value;
            }
        } //B
        string e, f;
        public override string ToString() {
            (string name, string value) e = (nameof(E), E);
            (string name, string value) d = (nameof(F), F);
            return DefinitionSet.Dump(GetType().Name, e, d);
        } //ToString
    } //class PseudoReadonlyDataSetXamlOnly

    class ReadonlyViolationException : System.ApplicationException {
        internal ReadonlyViolationException(System.Type type, string propertyName, object newValue) :
            base(@$"Attempt to assing a new value ""{newValue}"" to read-only property {type.FullName}.{propertyName}") { }
    } //class ReadonlyViolationException

}
