/*
    Copyright (C) 2023 by Sergey A Kryukov
    https://www.SAKryukov.org
    https://github.com/SAKryukov
    https://www.codeproject.com/Members/SAKryukov
*/

namespace SA.Agnostic.UI.Markup {
    using Type = System.Type;
    using ContentPropertyAttribute =
        System.Windows.Markup.ContentPropertyAttribute;
    using ArgumentList = System.Collections.Generic.List<FormalArgument>;

    [ContentProperty(nameof(Name))]
    public class FormalArgument {
        public FormalArgument() { Type = typeof(string); }
        public string Name { get; set; }
        public Type Type { get; set; }
    } //FormalArgument

    [ContentProperty(nameof(Arguments))]
    public class StringFormat {

        public StringFormat() { Arguments = new(); }

        public string Format { get; set; }
        public ArgumentList Arguments { get; set; }

        public string SubstituteValidated(params object[] arguments) {
            Type stringType = typeof(string);
            if (string.IsNullOrWhiteSpace(Format))
                throw new StringFormatException(
                    DefinitionSet.StringFormat.invalidFormatString);
            if (Arguments == null || Arguments.Count < 1)
                throw new StringFormatException(
                    DefinitionSet.StringFormat.invalidFormalArguments);
            if (arguments == null || arguments.Length < 1)
                throw new StringFormatException(
                    DefinitionSet.StringFormat.invalidActualArguments);
            if (Arguments.Count != arguments.Length)
                throw new StringFormatException(
                    DefinitionSet.StringFormat.ArgumentNumberMismatch(
                        Arguments.Count, arguments.Length));
            for (int index = 0; index < arguments.Length; ++index) {
                if (Arguments[index].Type == stringType) continue;
                if (!Arguments[index].Type.IsAssignableFrom(
                    arguments[index].GetType()))
                    throw new StringFormatException(
                        DefinitionSet.StringFormat.ArgumentTypeMismatch(
                            index,
                            Arguments[index].Type.FullName,
                            arguments[index].GetType().FullName));
            } //loop
            return string.Format(Format, arguments);
        } //SubstituteValidated

        public string Substitute(params object[] arguments) {
            return string.Format(Format, arguments);
        } //Substitute

        class StringFormatException : System.ApplicationException {
            internal StringFormatException(string message) : base(message) { }
        } //class StringFormatException

    } //class StringFormatsystem:UInt64system:UInt64

}
