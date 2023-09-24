/*
    Copyright (C) 2006-2023 by Sergey A Kryukov
    https://www.SAKryukov.org
    https://github.com/SAKryukov
    https://www.codeproject.com/Members/SAKryukov
*/

namespace SA.Semantic.UI {

    public interface IUiHost {
        System.Windows.Window MainApplicationWindow { get { return null; } }
        System.Windows.Controls.Decorator PluginHostContainer { get { return null; } }
        void InitializePluginHostContainer() { }
    } //interface IUiHost

    public interface IUiPlugin : INamedPlugin {
        void Create(IUiHost host) { }
        void Execute() { }
        void Destroy() { }
    } //interface IUiPlugin

} //SA.Semantic
