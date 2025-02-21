/*
    Copyright (C) 2023-2025 by Sergey A Kryukov
    https://www.SAKryukov.org
    https://github.com/SAKryukov
*/

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
