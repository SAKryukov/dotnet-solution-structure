namespace SA.Application.View {
    using IExceptionPresenter = Agnostic.UI.IExceptionPresenter;
    using Visibility = System.Windows.Visibility;
    using Assembly = System.Reflection.Assembly;
    using AdvancedApplication = Agnostic.UI.AdvancedApplicationBase;

    public partial class WindowMain : IExceptionPresenter {

        void IExceptionPresenter.Show(string exceptionTypeName, string exceptionMessage, string exception) {
            lastExceptionDump = exception;
            textBlockException.Text = exception;
            textBlockExceptionHeader.Text = exceptionMessage;
            SetExceptionVisibility(true);
            buttonCopyException.Focus();
        } //IExceptionPresenter.Show

        void SetExceptionVisibility(bool value) {
            borderMain.Visibility = value ? Visibility.Collapsed : Visibility.Visible;
            borderException.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
        } //SetExceptionVisibility

        void CopyLastExceptionDumpToClipboard() {
            System.Windows.Clipboard.SetText(Main.DefinitionSet.FormatExceptionForClipboard(
                AdvancedApplication.Current.ProductName,
                Assembly.GetEntryAssembly().Location,
                lastExceptionDump));
        } //CopyLastExceptionDumpToClipboard

        string lastExceptionDump;

    } //class WindowMain

}
