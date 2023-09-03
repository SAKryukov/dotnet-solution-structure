namespace SA.Application.View {
    using IExceptionPresenter = Agnostic.UI.IExceptionPresenter;

    public partial class WindowMain : IExceptionPresenter {

        void IExceptionPresenter.Show(string exceptionTypeName, string exceptionMessage, string exception) {
            lastExceptionInformationInstance = new LastExceptionInformation() {
                typeName = exceptionTypeName,
                message = exceptionMessage,
                dump = exception
            };
            textBlockException.Text = exception;
            textBlockExceptionHeader.Text = exceptionMessage;
            SetStateVisibility(state: VisibilityState.Exception);
            buttonSaveExceptionAndClose.Focus();
        } //IExceptionPresenter.Show

        void SaveExceptionAndClose() {
            var time = System.DateTime.Now;
            string filename = Main.DefinitionSet.ExceptionReport.FormatFilename(
                Main.DefinitionSet.ExceptionReport.FormatTimeFile(time),
                advancedApplication.EntryAssembly.ManifestModule.Name);
            saveExceptionReportDialog.FileName = filename;
            if (saveExceptionReportDialog.ShowDialog() != true) return;
            string report = Main.DefinitionSet.ExceptionReport.FormatReport(
                Main.DefinitionSet.ExceptionReport.FormatTime(time),
                advancedApplication.ProductName,
                advancedApplication.EntryAssembly.FullName,
                advancedApplication.EntryAssembly.Location,
                lastExceptionInformationInstance.typeName,
                lastExceptionInformationInstance.message,
                lastExceptionInformationInstance.dump);
            System.IO.File.WriteAllText(saveExceptionReportDialog.FileName, report);
            SetStateVisibility();
        } //SaveExceptionAndClose

        struct LastExceptionInformation {
            internal string typeName;
            internal string message;
            internal string dump;
        } //LastExceptionInformation

        LastExceptionInformation lastExceptionInformationInstance;

    } //class WindowMain

}
