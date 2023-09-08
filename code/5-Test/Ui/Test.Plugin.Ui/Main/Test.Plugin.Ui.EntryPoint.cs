namespace SA.Test.Main {
    using System;
    using System.Windows;

    static class MainClass {
        [STAThread]
        static void Main() {
            Application app = new Agnostic.UI.AdvancedApplication<View.WindowMainTest>() {
                Resources = new PrimitiveResourceSet().Resources
            };
            app.Run();
        } //MainClass
    } //class MainClass

}