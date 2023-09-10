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
