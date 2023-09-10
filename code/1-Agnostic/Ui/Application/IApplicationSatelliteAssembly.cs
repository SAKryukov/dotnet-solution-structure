namespace SA.Agnostic.UI {
    using System.Windows;

    public interface IApplicationSatelliteAssembly : IRecognizable {
        ResourceDictionary this[string fullTypeName] { get; }
        ResourceDictionary ApplicationResources { get; }
    } //IApplicationSatelliteAssembly

}
