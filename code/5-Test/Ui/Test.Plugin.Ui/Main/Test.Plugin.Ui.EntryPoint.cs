namespace SA.Test.Main {
    using System;
    using System.Windows;

    static class TestStart {
        [STAThread]
        static void Main() {
            Application app = new Agnostic.UI.AdvancedApplication<View.WindowMainTest>() {};
            app.Run();
        } //MainClass
    } //class TestStart

}
