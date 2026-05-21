namespace root-namespace-long-unique-name-not-to-be-repeated.Test.CommandLine.Main {
    using System;
    using System.Windows;

    static class TestStart {
        [STAThread]
        static void Main() {
            Application app = new SA.Agnostic.UI.AdvancedApplication<View.WindowMain>() {};
            app.Run();
        } //MainClass
    } //class TestStart

}
