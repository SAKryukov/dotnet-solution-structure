/*
    Copyright (C) 2023-2025 by Sergey A Kryukov
    https://www.SAKryukov.org
    https://github.com/SAKryukov
*/

namespace SA.Application.View {
    using System.ComponentModel;
    using System.Windows;

    public partial class About : Window {

        public About() {
            InitializeComponent();
            Title = Agnostic.UI.AdvancedApplicationBase.Current.ProductName;
        } //About

        protected override void OnClosing(CancelEventArgs e) {
            base.OnClosing(e);
            e.Cancel = true;
            Hide();
        } //OnClosing

        internal void ShowAbout(Window owner) {
            Owner = owner;
            ShowDialog();
        } //ShowAbout

    } //class About
}
