namespace SA.Application.Main {
    using System;
    using System.Windows;

    static class ApplicationStart {
        [STAThread]
        static void Main() {
            Application app = new Agnostic.UI.AdvancedApplication
                <View.WindowMain>();
            app.Run();
        } //MainClass
    } //class ApplicationStart

}