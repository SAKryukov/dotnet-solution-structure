/*
    Copyright (C) 2023-2025 by Sergey A Kryukov
    https://www.SAKryukov.org
    https://github.com/SAKryukov
*/

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
