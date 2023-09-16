/*  
    Command Line Utility:
    
    CommandLine is based on Enumeration and EnumerationIndexedArray
    To define command line options, define enumerations for switches and values and instantiate CommandLine parser class
    See CommandLine
    
    Copyright (C) 2004-2023 by Sergey A Kryukov
    http://www.SAKryukov.org
*/

namespace SA.Agnostic.Utilities {

    /// <summary>
    /// Status of the switch-like command line option (formatted as [-|/]option[-|+])
    /// returned by indexed property of <seealso cref="CommandLine"/>
    /// </summary>
    public enum CommandLineSwitchStatus : byte {
        
        /// <summary>
        /// Requested command line switch not found in command line
        /// </summary>
        Absent,
        /// <summary>
        /// Requested command line switch if found in command line in the form [-|/]option
        /// </summary>
        
        Present,
        /// <summary>
        /// Requested command line switch if found in command line in the form [-|/]option- ("switched off")
        /// </summary>
        Minus,

        /// <summary>
        /// Requested command line switch if found in command line in the form [-|/]option+
        /// </summary>
        Plus,

    } //enum CommandLineSwitchStatus

    [System.Flags]
    public enum CommandLineParsingOptions {
        CaseSensitiveKeys = 1,
        CaseSensitiveAbbreviations = 2,
        CaseSensitiveFiles = 4,
        DefaultMicrosoft = CaseSensitiveKeys | CaseSensitiveAbbreviations,
        DefaulUnix = CaseSensitiveKeys | CaseSensitiveAbbreviations | CaseSensitiveFiles,
        CaseInsensitive = 0,
    } //enum CommandLineParsingOptions

    #region implementation

    internal static class CommandLineParserDefinitionSet { //do not globalize! 
        internal static char[] prefixes = new char[] { '-', '/', };
        internal const char keyValueDelimiter = ':';
        internal const char optionOn = '+';
        internal const char optionOff = '-';
        internal const string fmtInvalidEnumType = "{0} must be enumeration type"; //type name
        internal const string fmtUnrecognizedSwitch = "{0}{1}"; //prefix, key
        internal const string fmtUnrecognizedValue = "{0}{1}{2}{3}"; //prefix, key, delimiter, value
    } //class CommandLineParserDefinitionSe

    internal static class CommandLineSimulationUtilityDefinitionSet { //do not globalize! 
        internal const char tokenDelimiter = ' ';
        internal const char tokenQuotation = '"';
        internal const char replacementDelimiter = '\0';
        internal static readonly string preliminaryTermDelimiter = string.Empty + tokenQuotation + tokenDelimiter + tokenQuotation;
        internal static readonly string replacementPreliminaryTermDelimiter = string.Empty + tokenQuotation + replacementDelimiter + tokenQuotation;
        internal static readonly string termDelimiter1 = string.Empty + tokenQuotation + tokenDelimiter;
        internal static readonly string replacementTermDelimiter1 = string.Empty + tokenQuotation + replacementDelimiter;
        internal static readonly string termDelimiter2 = string.Empty + tokenDelimiter + tokenQuotation;
        internal static readonly string replacementTermDelimiter2 = string.Empty + replacementDelimiter + tokenQuotation;
        internal const string fmtQuotedCommandLineLocation = @"""{0}""";
        internal static readonly char[] tokenUnwanted = new char[] { '\n', '\r', };
    } //CommandLineSimulationUtilityDefinitionSet

    #endregion implementation

}
