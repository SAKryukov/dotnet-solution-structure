namespace SA.Test.Extensions {
    using System.Windows;

    //SA??? Unused, serves as a sample:
    public partial class ResourceDictionaryProvider : DependencyObject {

        public DependencyProperty EnumerationObjectNameProperty = DependencyProperty.Register(
                name: nameof(EnumerationObjectName),
                propertyType: typeof(ResourceDictionary),
                ownerType: typeof(ResourceDictionaryProvider),
                typeMetadata: new FrameworkPropertyMetadata(
                    (sender, eventArgs) => { //PropertyChangedCallback propertyChangedCallback
                        if (sender is ResourceDictionaryProvider resourceDictionaryProvider)
                            resourceDictionaryProvider.EnumerationObjectName = (ResourceDictionary)eventArgs.NewValue;
                    }));
        public ResourceDictionary EnumerationObjectName {
            get => (ResourceDictionary)GetValue(EnumerationObjectNameProperty);
            set => SetValue(EnumerationObjectNameProperty, value);
        } //EnumerationObjectName


        /*
    internal class TargetBase : DependencyObject {
        private protected static DependencyProperty RegisterDependencyProperty(
            string name, Type ownerТype, Type propertyТype, PropertyChangedCallback callback = null) =>
                DependencyProperty.Register(
                    name: nameof(name),
                    propertyType: propertyТype,
                    ownerType: ownerТype,
                    typeMetadata: new FrameworkPropertyMetadata(callback));
    } //class TargetBase

    internal class ResourceTarget : TargetBase {
        public Thickness Thickness { get; set; }
        public Color Color { get; set; }
        public static readonly DependencyProperty NameProperty = RegisterDependencyProperty(nameof(Name), typeof(ResourceTarget), typeof(string),
            (sender, eventArgs) => { if (sender is ResourceTarget resourceTarget) resourceTarget.Name = (string)eventArgs.NewValue; });
        public string Name {
            get => (string)GetValue(NameProperty);
            set => SetValue(NameProperty, value);
        } //EnumerationObjectName
        public string Name { get; set; }
        public string Description = null;
        public int Count { get; set; }
    } //class ResourceTarget

    internal class IndependentResourceTarget {
        public Thickness Thickness2 = default;
        public string Name2 { get; set; }
        public string Description2 = null;
        public int Count2 { get; set; }
    } //class IndependentResourceTarget
         */

    } //class ResourceDictionaryProvider

}
