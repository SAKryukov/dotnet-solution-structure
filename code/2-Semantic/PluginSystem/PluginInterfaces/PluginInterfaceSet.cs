/*
    Copyright (C) 2006-2023 by Sergey A Kryukov
    https://www.SAKryukov.org
    https://github.com/SAKryukov
    https://www.codeproject.com/Members/SAKryukov
*/

namespace SA.Semantic {
    using Assembly = System.Reflection.Assembly;
    using IRecognizable = Agnostic.IRecognizable;

    public interface IHost {
        void Add(string property, string value);
    } //IHost

    public interface INamedPlugin : IRecognizable {
        string DisplayName { get; }
    } //interface INamedPlugin

    public interface IPropertyPlugin : INamedPlugin {
        void DiscoverProperties(Assembly assembly, IHost presenter);
    } //interface IPropertyPlugin

}
