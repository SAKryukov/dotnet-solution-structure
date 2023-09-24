/*
    Copyright (C) 2023 by Sergey A Kryukov
    https://www.SAKryukov.org
    https://github.com/SAKryukov
    https://www.codeproject.com/Members/SAKryukov
*/

namespace SA.Application.Main {
    using System;
    using System.Windows;
    using System.Windows.Input;

    static class ApplicationStart {

        [STAThread]
        static void Main() {
            ApplicationCommands.Close.InputGestures.Add(new KeyGesture(Key.F4));
            Application app = new Agnostic.UI.AdvancedApplication
                <View.WindowMain, View.ApplicationResourceSource>();
            app.Run();
        } //MainClass

    } //class ApplicationStart

}
