namespace SA.Agnostic.UI {
    using Path = System.IO.Path;
    using Assembly = System.Reflection.Assembly;
    using MessageBox = System.Windows.MessageBox;

    public static class ConsoleHelperUtility {

        public static void ShowExit(string message = null) {
            if (!System.Diagnostics.Debugger.IsAttached)
                MessageBox.Show(
                    message ?? DefinitionSet.ConsoleHelperUtility.showExit,
                    Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location));
        } //ShowExit

    } //class ConsoleHelperUtility

}
