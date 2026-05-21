/*
    Copyright (C) 2004-2025 by Sergey A Kryukov
    http://www.SAKryukov.org
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
