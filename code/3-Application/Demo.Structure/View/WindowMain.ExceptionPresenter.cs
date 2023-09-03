namespace SA.Application.View {
    using IExceptionPresenter = Agnostic.UI.IExceptionPresenter;
    using Time = System.DateTime;
    using TimeZoneInfo = System.TimeZoneInfo;

    public partial class WindowMain : IExceptionPresenter {

        void IExceptionPresenter.Show(string exceptionTypeName, string exceptionMessage, string exception) {
            lastExceptionInformationInstance = new LastExceptionInformation() {
                utc = Time.UtcNow,
                timeZone = TimeZoneInfo.Local,
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
            var time = lastExceptionInformationInstance.utc;
            var localTime = time.ToLocalTime();
            string filename = Main.DefinitionSet.ExceptionReport.FormatFilename(
                Main.DefinitionSet.ExceptionReport.FormatTimeFile(localTime),
                advancedApplication.EntryAssembly.ManifestModule.Name);
            saveExceptionReportDialog.FileName = filename;
            if (saveExceptionReportDialog.ShowDialog() != true) return;
            string report = Main.DefinitionSet.ExceptionReport.FormatReport(
                Main.DefinitionSet.ExceptionReport.FormatTime(time),
                lastExceptionInformationInstance.timeZone.ToString(),
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
            internal Time utc;
            internal TimeZoneInfo timeZone;
            internal string typeName;
            internal string message;
            internal string dump;
        } //LastExceptionInformation

        LastExceptionInformation lastExceptionInformationInstance;

    } //class WindowMain

}
