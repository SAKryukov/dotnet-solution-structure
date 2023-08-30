namespace SA.Semantic {
    using Assembly = System.Reflection.Assembly;
    using IRecognizable = Agnostic.IRecognizable;

    public interface IHost {
        void Add(string property, string value);
    } //IHost

    public interface IPropertyPlugin : IRecognizable {
        void DiscoverProperties(Assembly assembly, IHost presenter);
    } //interface IPropertyPlugin

}
