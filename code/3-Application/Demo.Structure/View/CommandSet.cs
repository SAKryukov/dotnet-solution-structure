/*
    Copyright (C) 2023 by Sergey A Kryukov
    https://www.SAKryukov.org
    https://github.com/SAKryukov
    https://www.codeproject.com/Members/SAKryukov
*/

namespace SA.Application.View {
    using System.Windows.Input;

    public static class CommandSet {
        public static readonly RoutedCommand TestException = new();
        public static readonly RoutedCommand ExecuteUiPlugin = new();
        public static readonly RoutedCommand ExecutePropertyPluginWithEntryAssembly = new();
        public static readonly RoutedCommand ExecutePropertyPluginWithPluginAssembly = new();
        public static readonly RoutedCommand ExecutePropertyPluginWithAssembly = new();
        public static readonly RoutedCommand UnloadPlugin = new();
    } //class CommandSet

}
