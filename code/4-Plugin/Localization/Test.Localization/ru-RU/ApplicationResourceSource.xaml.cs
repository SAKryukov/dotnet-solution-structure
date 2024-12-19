/*
    Copyright (C) 2023-2024 by Sergey A Kryukov
    https://www.SAKryukov.org
    https://github.com/SAKryukov
*/

[assembly: SA.Agnostic.PluginManifest(
    typeof(SA.Agnostic.UI.IApplicationSatelliteAssembly),
    typeof(SA.ApplicationResourceSource))]

namespace SA {
    using System.Windows;
    using Agnostic.UI;

    public partial class ApplicationResourceSource : FrameworkContentElement, IApplicationSatelliteAssembly {
        
        public ApplicationResourceSource() {
            InitializeComponent();
        } //ApplicationResourceSource

        ResourceDictionary IApplicationSatelliteAssembly.ApplicationResources => Resources;
        ResourceDictionary IApplicationSatelliteAssembly.this[string _] => null;

    } //class ApplicationResourceSource

}
