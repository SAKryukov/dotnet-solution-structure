namespace SA.Plugin.View {
    using System.Windows.Controls;
    using IUiHost = Semantic.IUiHost;

    public partial class EmbeddedControl : UserControl {

        public EmbeddedControl() {
            InitializeComponent();
            textBox.TextChanged += (sender, _) => {
                if (sender is TextBox textBoxSender) {
                    if (textBoxSender.Text.Contains(DefinitionSet.testExceptionHandlingByHostCommand))
                        try {
                            int x = 0;
                            int y = 1;
                            y /= x;
                        } catch (System.Exception e) {
                            if (host != null)
                                host.HandleException(e);
                        } //exception
                } //if
            }; //textBox.TextChanged
        } //EmbeddedControl

        internal IUiHost host;

    } //class EmbeddedControl

}
