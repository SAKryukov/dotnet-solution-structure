/*
    Copyright (C) 2006-2023 by Sergey A Kryukov
    https://www.SAKryukov.org
    https://github.com/SAKryukov
    https://www.codeproject.com/Members/SAKryukov
*/

namespace SA.Agnostic.UI {
    using System.Windows;

    public interface IApplicationSatelliteAssembly : IRecognizable {
        ResourceDictionary this[string fullTypeName] { get; }
        ResourceDictionary ApplicationResources { get; }
    } //IApplicationSatelliteAssembly

    public interface IResourceDictionaryProvider {
        ResourceDictionary Resources { get; }
    } //interface IResourceDictionaryProvider

}
