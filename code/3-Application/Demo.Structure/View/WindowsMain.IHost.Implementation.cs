namespace SA.Application.View {
    using Semantic;

    public partial class WindowMain : IHost {

        void IHost.Add(string property, string value) {
            AddRow(property, value);
        } //IHost.Add

    } //class WindowMain

}
