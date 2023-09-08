namespace SA.Application.Main {
    using System;
    using System.Windows;

    static class MainClass {
        [STAThread]
        static void Main() {
            Application app = new Agnostic.UI.AdvancedApplication<View.WindowMain>();
            app.Run();
        } //MainClass
    } //class MainClass

}