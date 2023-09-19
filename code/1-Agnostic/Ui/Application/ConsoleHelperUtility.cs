namespace SA.Agnostic.UI {
    using Path = System.IO.Path;
    using Assembly = System.Reflection.Assembly;
    using MessageBox = System.Windows.MessageBox;
    using MessageBoxButton = System.Windows.MessageBoxButton;
    using MessageBoxResult = System.Windows.MessageBoxResult;

    public static class ConsoleHelperUtility {

        public static void ShowExit(string message = null) {
            if (System.Diagnostics.Debugger.IsAttached) return;
            MessageBox.Show(
                message ?? DefinitionSet.ConsoleHelperUtility.showExit,
                Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location));
        } //ShowExit

        public static bool? RequestYesNoCancel(string message = null) {
            if (System.Diagnostics.Debugger.IsAttached) return null;
            MessageBoxResult result = MessageBox.Show(
                    message,
                    Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location),
                    MessageBoxButton.YesNoCancel
                    );
            return result == MessageBoxResult.Cancel
                ? null
                : (result == MessageBoxResult.Yes ? true : false);
        } //RequestYesNoCancel


    } //class ConsoleHelperUtility

}
