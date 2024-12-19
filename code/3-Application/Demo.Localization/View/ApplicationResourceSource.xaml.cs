/*
    Copyright (C) 2023-2024 by Sergey A Kryukov
    https://www.SAKryukov.org
    https://github.com/SAKryukov
*/

namespace SA.Application.View {
    using IResourceDictionaryProvider = Agnostic.UI.IResourceDictionaryProvider;

    public partial class ApplicationResourceSource : System.Windows.FrameworkContentElement, IResourceDictionaryProvider {

        System.Windows.ResourceDictionary IResourceDictionaryProvider.Resources => Resources;
        public ApplicationResourceSource() {
            InitializeComponent();
        } //ApplicationResourceSource

    } //class ApplicationResourceSource

}
