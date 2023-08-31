using SA.Agnostic;
using SA.Semantic;
[assembly:PluginManifest(typeof(IPropertyPlugin), typeof(SA.Plugin.Implementation))]

namespace SA.Plugin {
    using Assembly = System.Reflection.Assembly;

    class Implementation : IPropertyPlugin {
        
        void IPropertyPlugin.DiscoverProperties(Assembly assembly, IHost presenter) {
            presenter.Add("Location", assembly.Location);
            presenter.Add("Full Name", assembly.FullName);
            presenter.Add("Loaded Modules", assembly.GetLoadedModules().Length.ToString());
            presenter.Add("Referenced Assemblies", assembly.GetReferencedAssemblies().Length.ToString());
            presenter.Add("Custom Attributes", assembly.GetCustomAttributes(false).Length.ToString());
            presenter.Add("Types", assembly.GetTypes().Length.ToString());
            presenter.Add("Exported Types", assembly.GetExportedTypes().Length.ToString());
            presenter.Add("Forwarded Types", assembly.GetForwardedTypes().Length.ToString());
            if (assembly.EntryPoint != null) {
                presenter.Add("Entry Point Name", assembly.EntryPoint.Name);
                presenter.Add("Entry Point Return Type", assembly.EntryPoint.ReturnType.ToString());
                presenter.Add("Entry Point Return Declaring Type", assembly.EntryPoint.DeclaringType.FullName);
                presenter.Add("Entry Point Parameters", assembly.EntryPoint.GetParameters().Length.ToString());
            } //if

        } //IPropertyPlugin.DiscoverProperties

    } //class Implementation

}
