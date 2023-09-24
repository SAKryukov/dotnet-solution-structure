/*
    Copyright (C) 2023 by Sergey A Kryukov
    https://www.SAKryukov.org
    https://github.com/SAKryukov
    https://www.codeproject.com/Members/SAKryukov
*/

namespace SA.Plugin.View {
    using System.Windows.Controls;
    using System.Windows.Media;
    using IUiHost = Semantic.UI.IUiHost;

    public partial class EmbeddedControl : UserControl {

        public EmbeddedControl() {
            InitializeComponent();
            buttonException.Click += (_, _) => {
                object dummy = new();
                string value = (string)dummy;
            }; //buttonException.Click
        } //EmbeddedControl

        protected override void OnRender(DrawingContext drawingContext) {
            base.OnRender(drawingContext);
            System.Windows.Input.Keyboard.Focus(textBox);
        }

        internal IUiHost host;

    } //class EmbeddedControl

}
