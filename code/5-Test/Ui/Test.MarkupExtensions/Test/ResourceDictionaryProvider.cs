namespace SA.Test.Extensions {
    using System.Windows;

    //SA??? Unused, serves as a sample:
    public class ResourceDictionaryProvider : DependencyObject {

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

    } //class ResourceDictionaryProvider

}
