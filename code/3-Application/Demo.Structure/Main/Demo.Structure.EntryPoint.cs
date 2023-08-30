namespace SA.Application {
    using System;
    using System.Windows;

    static class MainClass {
        [STAThread]
        static void Main() {
            Application app = new SA.Agnostic.UI.AdvancedApplication<View.WindowMain>();
            app.Run();
        } //MainClass
    } //class MainClass

}