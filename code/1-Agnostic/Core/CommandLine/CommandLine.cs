/* 
    Command Line Utility:
    
    CommandLine is based on Enumeration and EnumerationIndexedArray
    To define command line options, define enumerations for switches and values and instantiate CommandLine.
    These enumeration types are typically enum, but any other type with static constant fields of the same type will work, such as int, double, etc.,
    because the role of enum members will be played by MinValue, MaxValue, etc.
    
    Command line options take the form:
    
    Switches:
    -key -key+ -key- /key /key+ /key-
    a switch can have CommandLineSwitchStatus obtained from GetSwitch: Absent, Present (no plus or minus at the end), Plus or Minus.
    Simplified API this[<SWITCH ENUM>] returns boolean (true if Present or Plus).
  
    Values:omm
    -key:value /key:value
    a value is a string obtained from this[<VALUE ENUM>]; no requirements are imposed on the values; they can be empty of contain prefixes/delimiters.
    if a -key:value option is missing, the value is null, if it is present in form -key: or /key: the value is empty string.
 
    All other command line options are called "files"; no requirements are imposed on their values except being unique.
  
    All the keys and "files" must be unique, taking into account case sensitivity separate for keys, files and abbreviated keys (see CommandLineParsingOptions).
    Keys of switches, keys of values and "files" are separate sets of options; the unuqiueness is only required withing the set.
    For example, the command line "-file -file:input.txt input.txt file" is completely valid; no options are ignored.
    If a key (of a switch or value type regardless of its status or value) or a "file" is repeated, the first occurence takes precedence
    with all other occurences dubbed "repeated".
    Keys can be abbreviated in their enumeration types using AbbreviationAttribute or ignored using NonEnumerableAttribute.
    A full key always takes precedence over abbreviated: if both full and abbreviated keys present, the abbreviated one is dubbed "repeated".
  
    All user's command line errors are collected in UnrecognizedOptions, RepeatedFiles, RepeatedSwitches and RepeatedValues; no exceptions are thrown.
    
    Copyright (C) 2004-2023 by Sergey A Kryukov
    http://www.SAKryukov.org
*/

namespace SA.Agnostic.Utilities {
    using System;
    using System.Collections.Generic;
    using StringList = System.Collections.Generic.List<string>;
    using StringDictionary = System.Collections.Generic.Dictionary<string, string>;
    using DefinitionSet = CommandLineParserDefinitionSet;
    using SA.Agnostic.Enumerations;
#if DEBUG
    using Debug = System.Diagnostics.Debug;
#endif

    /// <summary>
    /// Command Line Parser controlled by enumeration declarations
    /// </summary>
    /// <typeparam name="SWITCHES">Enumeration type defining switch options using syntax -|/option[-|+]</typeparam>
    /// <typeparam name="VALUES">Enumeration type defining value options using syntax -|/option:[value]</typeparam>
    public sealed class CommandLine<SWITCHES, VALUES> {

        /// <summary>
        /// There is no need to use to use this structure directly, whatsoever;
        /// instead, assign <seealso cref="CommandLine"/> index property indexed by SWITCHES to boolean or <seealso cref="CommandLineSwitchStatus"/> variable;
        /// CommandLineSwitchStatusWrapper uses implicit type conversion operator to convert to any of those types;
        /// converted boolean value retuns True if status is <seealso cref="CommandLineSwitchStatus.Plus"/> or <seealso cref="CommandLineSwitchStatus.Present"/>.
        /// </summary>
        public struct CommandLineSwitchStatusWrapper {
            public static implicit operator CommandLineSwitchStatus(CommandLineSwitchStatusWrapper wrapper) { return wrapper.status; }
            public static implicit operator bool(CommandLineSwitchStatusWrapper wrapper) { return wrapper.status == CommandLineSwitchStatus.Plus || wrapper.status == CommandLineSwitchStatus.Present; }
            internal CommandLineSwitchStatusWrapper(CommandLineSwitchStatus status) { this.status = status; }
            readonly CommandLineSwitchStatus status;
        } //struct CommandLineSwitchStatusWrapper
        
        /// <summary>
        /// This constructor without parameters extracts command line strings from System.Environment using default options
        /// (default depends on run-time platform, see <seealso cref="CommandLineParsingOptions"/>)
        /// </summary>
        public CommandLine() { Construct(ExtractCommandLine()); }
        
        /// <summary>
        /// This constructor extracts command line strings from System.Environment
        /// </summary>
        /// <param name="options">Command line options</param>
        public CommandLine(CommandLineParsingOptions options) { Construct(ExtractCommandLine(), options); }

        /// <summary>
        /// This constructor gets command line strings from parameter and uses default options
        /// (default depends on run-time platform, see <seealso cref="CommandLineParsingOptions"/>)
        /// </summary>
        /// <param name="commandLine">Command line</param>
        public CommandLine(string[] commandLine) { Construct(commandLine); }

        /// <summary>
        /// This constructor gets command line strings and options from parameters
        /// </summary>
        /// <param name="commandLine">Command line</param>
        /// <param name="options">Command line options</param>
        public CommandLine(string[] commandLine, CommandLineParsingOptions options) { Construct(commandLine, options); }

        /// <summary>
        /// Property indexed by VALUES key returns a value associated with the option
        /// </summary>
        /// <param name="key">Index of the type VALUES</param>
        /// <returns>Value associated with the generic-parameter option</returns>
        public string this[VALUES key] { get { return values[key]; } }

        /// <summary>
        /// Property indexed by SWITCHES key returns either switch status or boolean value,
        /// using <seealso cref="CommandLineSwitchStatusWrapper"/> and its implicit convertions to <seealso cref="CommandLineSwitchStatus"/> or boolean;
        /// converted boolean value retuns True if status is <seealso cref="CommandLineSwitchStatus.Plus"/> or <seealso cref="CommandLineSwitchStatus.Present"/>.
        /// </summary>
        /// <param name="index">Index of the generic-parameter type SWITCHES</param>
        /// <returns>either switch status or boolean value</returns>
        public CommandLineSwitchStatusWrapper this[SWITCHES index] { get { return new CommandLineSwitchStatusWrapper(switches[index]); } }
        
        /// <summary>
        /// Array of string property returns all the command line parameters without "-" or "/" key;
        /// they are called files because most typical application for this feature is a set of file names.
        /// </summary>
        public string[] Files { get { return files; } }

        /// <summary>
        /// Enumeration property returns Enumeration indexed generic-parameter type SWITCHES
        /// </summary>
        public Enumeration<SWITCHES> SwitchEnumeration { get { return switchEnumeration; } }

        /// <summary>
        /// Enumeration property returns Enumeration indexed generic-parameter type VALUES
        /// </summary>
        public Enumeration<VALUES> ValueEnumeration { get { return valueEnumeration; } }
        
        /// <summary>
        /// Array of string property returns the set of all ill-formatted command line options
        /// </summary>
        public string[] UnrecognizedOptions { get { return unrecognizedOptions; } }

        /// <summary>
        /// Array of string property returns the set of all duplicate command line options without "-" or "/" key
        /// </summary>
        public string[] RepeatedFiles { get { return repeatedFiles; } }

        /// <summary>
        /// Array of string property returns the set of all duplicate switch-like options (formatted as -|/option[-|+])
        /// </summary>
        public string[] RepeatedSwitches { get { return repeatedSwitches; } }

        /// <summary>
        /// Array of string property returns the set of all duplicate value-like options (formatted as -|/option:[value])
        /// </summary>
        public string[] RepeatedValues { get { return repeatedValues; } }

        /// <summary>
        /// Current Command Line Parsing Options
        /// </summary>
        public CommandLineParsingOptions CommandLineParsingOptions { get { return options; } }

        /// <summary>
        /// Default Command Line Parsing Options for current platform
        /// </summary>
        public static CommandLineParsingOptions DefaultCommandLineParsingOptions { get { return GetDefaultCommandLineParsingOptions(); } }

        #region implementation

        void Construct(string[] commandLine, CommandLineParsingOptions options)  {
            this.options = options;
#if DEBUG
            ValidateEnumerations();
#endif
            ParseCommandLine(commandLine);
        } //Construct
        void Construct(string[] commandLine) {
            CommandLineParsingOptions defaultCase = GetDefaultCommandLineParsingOptions();
            Construct(commandLine, defaultCase);
        } //Construct

        private static CommandLineParsingOptions GetDefaultCommandLineParsingOptions() {
            switch (Environment.OSVersion.Platform) {
                case PlatformID.Win32NT:
                case PlatformID.Win32S:
                case PlatformID.Win32Windows:
                case PlatformID.Xbox:
                    return CommandLineParsingOptions.DefaultMicrosoft;
                case PlatformID.WinCE:
                    break;
                case PlatformID.Unix:
                    break;
                case PlatformID.MacOSX:
                    break;
                case PlatformID.Other:
                    break;
                default: //this way, if Microsoft later recognized yet another platform and adds it to this enumeration type, it will still compile and fall into Unix category:
                    return CommandLineParsingOptions.DefaulUnix;
            } //switch
            return CommandLineParsingOptions.DefaulUnix; 
        } //GetDefaultCommandLineParsingOptions

        static string[] ExtractCommandLine() {
            string[] commandLine = System.Environment.GetCommandLineArgs();
            int len = commandLine.Length;
            if (len < 2) return Array.Empty<string>();
            string[] destination = new string[len - 1];
            Array.Copy(commandLine, 1, destination, 0, len - 1);
            return destination;
        } //ExtractCommandLine

        bool ParseTerm(string term, out bool isFile, out string key, out CommandLineSwitchStatus status, out string value) {
            value = null;
            key = null;
            status = CommandLineSwitchStatus.Present;
            string keyedValue = ExtractPrefix(term);            
            isFile = keyedValue == null;
            if (keyedValue == null) {
                isFile = true;
                value = term;
                return true;
            } else if (keyedValue.Length < 1)
                return false;
            string[] keyValuePair = keyedValue.Split(new char[] { DefinitionSet.keyValueDelimiter }, 2);
            if (keyValuePair.Length == 1) {
                key = keyValuePair[0];
                int len = key.Length;
                if (key.Length > 1) {
                    char last = key[len - 1];
                    if (last == DefinitionSet.optionOff) status = CommandLineSwitchStatus.Minus;
                    else if (last == DefinitionSet.optionOn) status = CommandLineSwitchStatus.Plus;
                    else status = CommandLineSwitchStatus.Present;
                    if (status != CommandLineSwitchStatus.Present) {
                        key = key.Substring(0, len - 1);
                        if (key.Contains(string.Empty + DefinitionSet.optionOff) || key.Contains(string.Empty + DefinitionSet.optionOn))
                            return false;
                    } //if
                } else {
                    if (key[0] == DefinitionSet.optionOff || key[0] == DefinitionSet.optionOn) return false;
                } //if
            } else if (keyValuePair.Length == 2) {
                key = keyValuePair[0];
                value = keyValuePair[1];
            } else
                return false;
            if (key != null && ((options & CommandLineParsingOptions.CaseSensitiveKeys) == 0))
                key = key.ToLower();
            return true;
        } //ParseTerm

        static string ExtractPrefix(string value) {
            if (string.IsNullOrEmpty(value)) return null; // this is pathological case if command line contains ""
            bool found = false;
            foreach (char prefix in DefinitionSet.prefixes) {
                found = value[0] == prefix;
                if (found) break;
            } //loop
            if (!found) return null;
            return value[1..];
        } //ExtractPrefix

        void ParseCommandLine(string[] terms) {
            StringDictionary fileDictionary = new();
            StringList fileList = new();
            StringList unrecognizedList = new();
            StringList repeatedFileList = new();
            StringList repeatedSwitches = new();
            StringList repeatedValues = new();
            Dictionary<string, SwitchDictionaryValue> switchDictionary = new();
            Dictionary<string, ValueDictionaryValue> valueDictionary = new();
            foreach (string term in terms) {
                if (ParseTerm(term, out bool isFile, out string key, out CommandLineSwitchStatus status, out string value)) {
                    if (isFile) { //file
                        if (value != null && ((options & CommandLineParsingOptions.CaseSensitiveFiles) == 0))
                            value = value.ToLower();
                        if (!fileDictionary.ContainsKey(value)) {
                            fileDictionary.Add(value, term);
                            fileList.Add(term);
                        } else
                            repeatedFileList.Add(term);
                    } else if (value == null) { //switch
                        if (!switchDictionary.ContainsKey(key))
                            switchDictionary.Add(key, new SwitchDictionaryValue(status, term));
                        else
                            repeatedSwitches.Add(term);
                    } else { //value
                        if (!valueDictionary.ContainsKey(key))
                            valueDictionary.Add(key, new ValueDictionaryValue(value, term));
                        else
                            repeatedValues.Add(term);
                    } //end if switch
                } else
                    unrecognizedList.Add(term);
            } //loop
            List<EnumerationItem<SWITCHES>> switchKeyList = new();
            List<EnumerationItem<VALUES>> valueKeyList = new();
            foreach (EnumerationItem<SWITCHES> enumerationItem in switchEnumeration) {
                switchKeyList.Add(enumerationItem);
                PrepareKeys(enumerationItem, out string key, out string abbreviatedKey);
                ProcessDictionary(switchDictionary, enumerationItem, key, abbreviatedKey);
            } //loop switches
            foreach (EnumerationItem<VALUES> enumerationItem in valueEnumeration) {
                valueKeyList.Add(enumerationItem);
                PrepareKeys(enumerationItem, out string key, out string abbreviatedKey);
                ProcessDictionary(valueDictionary, enumerationItem, key, abbreviatedKey);
            } //loop values
            foreach (SwitchDictionaryValue dictionaryValue in switchDictionary.Values)
                if (dictionaryValue.repeated)
                    repeatedValues.Add(dictionaryValue.originalTerm);
                else
                    unrecognizedList.Add(dictionaryValue.originalTerm);
            foreach (ValueDictionaryValue dictionaryValue in valueDictionary.Values)
                if (dictionaryValue.repeated)
                    repeatedValues.Add(dictionaryValue.originalTerm);
                else
                    unrecognizedList.Add(dictionaryValue.originalTerm);
            unrecognizedOptions = unrecognizedList.ToArray();
            files = fileList.ToArray();
            repeatedFiles = repeatedFileList.ToArray();
            this.repeatedSwitches = repeatedSwitches.ToArray();
            this.repeatedValues = repeatedValues.ToArray();
        } //ParseCommandLine

        void PrepareKeys(EnumerationItem<SWITCHES> item, out string key, out string abbreviatedKey) {
            key = item.Name;
            if (key != null && ((options & CommandLineParsingOptions.CaseSensitiveKeys) == 0))
                key = key.ToLower();
            abbreviatedKey = item.AbbreviatedName;
            if (abbreviatedKey != null && ((options & CommandLineParsingOptions.CaseSensitiveAbbreviations) == 0))
                abbreviatedKey = abbreviatedKey.ToLower();
        } //PrepareKeys
        void PrepareKeys(EnumerationItem<VALUES> item, out string key, out string abbreviatedKey) {
            key = item.Name;
            if (key != null && ((options & CommandLineParsingOptions.CaseSensitiveKeys) == 0))
                key = key.ToLower();
            abbreviatedKey = item.AbbreviatedName;
            if (abbreviatedKey != null && ((options & CommandLineParsingOptions.CaseSensitiveAbbreviations) == 0))
                abbreviatedKey = abbreviatedKey.ToLower();
        } //PrepareKeys
        void ProcessDictionary(Dictionary<string, SwitchDictionaryValue> dictionary, EnumerationItem<SWITCHES> item, string key, string abbreviatedKey) {
            bool foundAsFull = false;
            if (dictionary.TryGetValue(key, out SwitchDictionaryValue dictionaryValue)) {
                switches[item.EnumValue] = dictionaryValue.status;
                dictionary.Remove(key);
                foundAsFull = true;
            } //if
            if (key == abbreviatedKey) return;
            if (dictionary.TryGetValue(abbreviatedKey, out dictionaryValue)) {
                if (!foundAsFull) {
                    switches[item.EnumValue] = dictionaryValue.status;
                    dictionary.Remove(abbreviatedKey);
                } else
                    dictionaryValue.repeated = true;
            } //if
        } //ProcessDictionary
        void ProcessDictionary(Dictionary<string, ValueDictionaryValue> dictionary, EnumerationItem<VALUES> item, string key, string abbreviatedKey) {
            bool foundAsFull = false;
            if (dictionary.TryGetValue(key, out ValueDictionaryValue dictionaryValue)) {
                values[item.EnumValue] = dictionaryValue.value;
                dictionary.Remove(key);
                foundAsFull = true;
            } //if
            if (key == abbreviatedKey) return;
            if (dictionary.TryGetValue(abbreviatedKey, out dictionaryValue)) {
                if (!foundAsFull) {
                    values[item.EnumValue] = dictionaryValue.value;
                    dictionary.Remove(abbreviatedKey);
                } else
                    dictionaryValue.repeated = true;
            } //if
        } //ProcessDictionary

#if DEBUG
        void ValidateEnumerations() {
            StringDictionary dictionary = new();
            StringDictionary abbreviatedDictionary = new();
            foreach (EnumerationItem<SWITCHES> enumerationItem in switchEnumeration) {
                PrepareKeys(enumerationItem, out string key, out string abbreviatedKey);
                ValidateBothKeys(dictionary, key, abbreviatedDictionary, abbreviatedKey);
            } //loop
            dictionary.Clear(); abbreviatedDictionary.Clear(); //important: switched and values are separate sets of options, uniqueness not required
            foreach (EnumerationItem<VALUES> enumerationItem in valueEnumeration) {
                PrepareKeys(enumerationItem, out string key, out string abbreviatedKey);
                ValidateBothKeys(dictionary, key, abbreviatedDictionary, abbreviatedKey);
            } //loop
        } //ValidateEnumerations
        static void ValidateKey(StringDictionary dictionary, string key) {
            Debug.Assert(!dictionary.ContainsKey(key), string.Format("Ambiguous command line: key {0} already found", key));
            dictionary.Add(key, key);
        } //ValidateKey
        static void ValidateBothKeys(StringDictionary dictionary, string key, StringDictionary abbreviatedDictionary, string abbreviatedKey) {
            ValidateKey(dictionary, key);
            ValidateKey(abbreviatedDictionary, abbreviatedKey);
        } //ValidateBothKeys
#endif 

        abstract class DictionaryValue {
            internal DictionaryValue(string originalTerm) { this.originalTerm = originalTerm; }
            internal string originalTerm;
            internal bool repeated;
        } //class DictionaryValue
        class SwitchDictionaryValue : DictionaryValue {
            internal SwitchDictionaryValue(CommandLineSwitchStatus status, string originalTerm) : base(originalTerm) { this.status = status; }
            internal CommandLineSwitchStatus status;
        } //class SwitchDictionaryValue
        class ValueDictionaryValue : DictionaryValue {
            internal ValueDictionaryValue(string value, string originalTerm) : base(originalTerm) { this.value = value; }
            internal string value;
        } //class SwitchDictionaryValue

        string[] files;
        string[] unrecognizedOptions;
        string[] repeatedFiles;
        string[] repeatedSwitches;
        string[] repeatedValues;

        readonly Enumeration<SWITCHES> switchEnumeration = new();
        readonly Enumeration<VALUES> valueEnumeration = new();

        readonly EnumerationIndexedArray<SWITCHES, CommandLineSwitchStatus> switches = new();
        readonly EnumerationIndexedArray<VALUES, string> values = new();

        CommandLineParsingOptions options;

        #endregion implementation

    } //class CommandLine

}
