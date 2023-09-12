/* 
    Command Line Simulation Utility:
   
    This Utility is used to provide convenience of testing CommandLine utility in a single UI session.
    It provides unparsed command line taken from Environment with removed entry assembly location.
    This command line can be used to simulate parsing of the command line into command line arguments
    the way it is done by System.Environment.GetCommandLineArgs() or provided to application enty point (usually "Main" function).
    The returned array can be used to test CommandLine utility.
   
    Copyright (C) 2023 by Sergey A Kryukov
    http://www.SAKryukov.org
*/
namespace SA.Universal.Utilities {
    using Assembly = System.Reflection.Assembly;
    using StringBuilder = System.Text.StringBuilder;
    using StringList = System.Collections.Generic.List<string>;

    /// <summary>
    /// Utility used to provide convenience of testing <seealso cref="CommandLine"/> utility in a single UI session
    /// </summary>
    public static class CommandLineSimulationUtility {

        /// <summary>
        /// Unparsed command line taken from Environment with removed entry assembly location; works for different runtime hosts
        /// </summary>
        public static string UnparsedCommandLine {
            get {
                string commandLine = System.Environment.CommandLine;
                string zeroParameter = System.Environment.GetCommandLineArgs()[0];
                string quotedZeroParameter = string.Format(CommandLineSimulationUtilityDefinitionSet.fmtQuotedCommandLineLocation, zeroParameter);
                string location = Assembly.GetEntryAssembly().Location;
                string quotedLocation = string.Format(CommandLineSimulationUtilityDefinitionSet.fmtQuotedCommandLineLocation, location);
                //the following cases of replacement are needed to exclude the occurences of entry assembly locations from command line;
                //the way to do it depends on how the running code is hosted, so different options are attempted:
                if (commandLine.StartsWith(quotedZeroParameter))
                    commandLine = commandLine.Replace(quotedZeroParameter, string.Empty);
                else if (commandLine.StartsWith(zeroParameter))
                    commandLine = commandLine.Replace(zeroParameter, string.Empty);
                else if (commandLine.StartsWith(quotedLocation))
                    commandLine = commandLine.Replace(quotedLocation, string.Empty);
                else if (commandLine.StartsWith(location))
                    commandLine = commandLine.Replace(location, string.Empty);
                commandLine = commandLine.Trim();
                return commandLine;
            } //get UnparsedCommandLine
        } //UnparsedCommandLine

        /// <summary>
        /// Simulates parsing of the command line into command line arguments
        /// the way it is done by System.Environment.GetCommandLineArgs() or provided to application enty point (usually "Main" function)
        /// </summary>
        /// <param name="commandLine"></param>
        /// <returns>Array of command line argumens to be used to test <seealso cref="CommandLine"/> utility</returns>
        public static string[] SimulateCommandLineParsingIntoArray(string commandLine) {
            return new CommandLineTokenizer(commandLine).CommandLine;
        } //SimulateCommandLineParsingIntoArray

        #region implementation

        /// <summary>
        /// This class simulates cutting raw single-string command line into tokens (hopefully) same way .NET does to obtain string[] commandLine
        /// Key issue: special use of """ to allow for spaces in file names, etc.
        /// 
        /// Still, this is not exact simulation: I don't understand .NET tokenizing logic:
        /// for example [aaa"b"""""bb] is parsed to [aaab""bb] (square brackets are not part of strings but used as a notational delimiter
        /// </summary>
        internal class CommandLineTokenizer {

            internal CommandLineTokenizer(string rawCommandLine) { this.rawCommandLine = rawCommandLine.Trim() + CommandLineSimulationUtilityDefinitionSet.tokenDelimiter; }
            internal string[] CommandLine { get { return GetCommandLine(); } }

            //enum Mode { Starting, Adding, SkippingEmpty, InQuotation, }

            string[] GetCommandLine() {
                string simplifiedCommandLine = rawCommandLine.Trim();
                foreach (char toReplace in CommandLineSimulationUtilityDefinitionSet.tokenUnwanted)
                    simplifiedCommandLine = simplifiedCommandLine.Replace(toReplace, CommandLineSimulationUtilityDefinitionSet.tokenDelimiter);
                StringList result = new();
                simplifiedCommandLine = simplifiedCommandLine.Replace(CommandLineSimulationUtilityDefinitionSet.preliminaryTermDelimiter, CommandLineSimulationUtilityDefinitionSet.replacementPreliminaryTermDelimiter);
                simplifiedCommandLine = simplifiedCommandLine.Replace(CommandLineSimulationUtilityDefinitionSet.termDelimiter1, CommandLineSimulationUtilityDefinitionSet.replacementTermDelimiter1);
                simplifiedCommandLine = simplifiedCommandLine.Replace(CommandLineSimulationUtilityDefinitionSet.termDelimiter2, CommandLineSimulationUtilityDefinitionSet.replacementTermDelimiter2);
                string[] terms = simplifiedCommandLine.Split(new char[] { CommandLineSimulationUtilityDefinitionSet.replacementDelimiter }, System.StringSplitOptions.RemoveEmptyEntries);
                foreach (string term in terms) {
                    string trimmed = term.Trim();
                    if (trimmed.Length > 0) {
                        trimmed = TrimQuotation(trimmed, out bool isQuotation);
                        if (isQuotation)
                            result.Add(trimmed);
                        else {
                            string[] subterms = trimmed.Split(new char[] { CommandLineSimulationUtilityDefinitionSet.tokenDelimiter }, System.StringSplitOptions.RemoveEmptyEntries);
                            foreach (string subterm in subterms) {
                                trimmed = subterm.Trim();
                                if (trimmed.Length > 0)
                                    result.Add(trimmed);
                            } //loop subterms
                        } //if isQuotation
                    } //if
                } //loop terms
                return result.ToArray();
            } //GetCommandLine

            static string TrimQuotation(string input, out bool isQuotation) {
                isQuotation = false;
                int length = input.Length;
                if (length < 2)
                    return input;
                string result = input;
                if (result[0] == CommandLineSimulationUtilityDefinitionSet.tokenQuotation && result[length - 1] == CommandLineSimulationUtilityDefinitionSet.tokenQuotation) {
                    isQuotation = true;
                    result = result.Substring(1, length - 2);
                } //if
                return result;
            } //TrimQuotation

            /* //SA???
            void Flush(ref StringBuilder source, StringList sink) {
                sink.Add(source.ToString());
                source = new StringBuilder();
            } //Flush
            */

            readonly string rawCommandLine;

        } //class CommandLineTokenizer
        
        #endregion implementation

    } //CommandLineSimulationUtility

}
