/*
    Copyright (C) 2023 by Sergey A Kryukov
    https://www.SAKryukov.org
    https://github.com/SAKryukov
    https://www.codeproject.com/Members/SAKryukov
*/

namespace SA.Test.View {
    using Semantic.UI;

    public partial class WindowMainTest : IUiHost {

        System.Windows.Window IUiHost.MainApplicationWindow { get { return this; } }
        System.Windows.Controls.Decorator IUiHost.PluginHostContainer { get { return borderPluginHostContainer; } }

    } //class WindowMain

}
