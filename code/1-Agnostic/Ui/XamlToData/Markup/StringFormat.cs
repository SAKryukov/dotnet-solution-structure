﻿/*
    Copyright (C) 2023-2025 by Sergey A Kryukov
    https://www.SAKryukov.org
    https://github.com/SAKryukov
*/

namespace SA.Agnostic.UI.Markup {
    using ContentPropertyAttribute =
        System.Windows.Markup.ContentPropertyAttribute;
    using Regex = System.Text.RegularExpressions.Regex;
    using Match = System.Text.RegularExpressions.Match;
    using StringDictionary = System.Collections.Generic.Dictionary<string, int>;

    [ContentProperty(nameof(Format))]
    public class StringFormat {

        public StringFormat() { }
        public StringFormat(string format) { stringFormat = format; }

        public string Format {
            get => stringFormat;
            set {
                ParseXamlFormat(value);
                stringFormat = value;
            } //set Format
        } //Format

        public string Substitute(params object[] actualParameters) {
            this.actualParameters = actualParameters;
            if (actualParameters == null) { // reset
                numberedStringFormat = null;
                return null;
            } //if
            if (formalParameters.Length != actualParameters.Length)
                throw new StringFormatException(DefinitionSet.StringFormat.InvalidParameterNumber(formalParameters.Length, actualParameters.Length));
            return ToString();
        } //Substitute

        public override string ToString() {
            return actualParameters == null || string.IsNullOrWhiteSpace(numberedStringFormat) || actualParameters.Length < 1
                ? DefinitionSet.StringFormat.FormalParameterDeclaration(string.Join(DefinitionSet.StringFormat.toStringSeparator, formalParameters))
                : string.Format(numberedStringFormat, actualParameters);
        } //ToString()

        string[] formalParameters;
        object[] actualParameters;
        string stringFormat;
        string numberedStringFormat;
        readonly StringDictionary dictionary = new();

        void ParseXamlFormat(string value) {
            if (formalParameters != null && formalParameters.Length > 0)
                throw new StringFormatException(DefinitionSet.StringFormat.InvalidFormatStringAssignment(formalParameters.Length));
            Regex regex = new(DefinitionSet.StringFormat.regularExpression);
            var matches = regex.Matches(value);
            dictionary.Clear();
            int dictionaryIndex = 0;
            for (int index = 0; index < matches.Count; ++index) {
                string key = matches[index].Groups[1].Value;
                if (!dictionary.ContainsKey(key))
                    dictionary[key] = dictionaryIndex++;
            } //loop
            formalParameters = new string[dictionary.Count];
            foreach (var pair in dictionary)
                formalParameters[pair.Value] = pair.Key;
            numberedStringFormat = value;
            foreach (Match match in matches) {
                string toReplace = match.Groups[0].Value;
                string key = match.Groups[1].Value;
                string subformat = match.Groups[2].Value;
                numberedStringFormat = 
                    numberedStringFormat.Replace(toReplace, DefinitionSet.StringFormat.BracketParameter(dictionary[key], subformat));
            } //loop
        } //ParseXamlFormat

        class StringFormatException : System.ApplicationException {
            internal StringFormatException(string message) : base(message) { }
        } //class StringFormatException

    } //class StringFormat

}
