[assembly: SA.Agnostic.PluginManifest(
    typeof(SA.Semantic.IPropertyPlugin),
    typeof(SA.Plugin.Implementation))]

namespace SA.Plugin {
    using SA.Semantic;
    using Assembly = System.Reflection.Assembly;

    class Implementation : IPropertyPlugin {

        string INamedPlugin.DisplayName { get { return DefinitionSet.propertyPluginName; } }
        
        void IPropertyPlugin.DiscoverProperties(Assembly assembly, IHost presenter) {
            presenter.Add(DefinitionSet.assemblyLocation, assembly.Location);
            presenter.Add(DefinitionSet.assemblyFullName, assembly.FullName);
            presenter.Add(DefinitionSet.assemblyLoadedModules, assembly.GetLoadedModules().Length.ToString());
            presenter.Add(DefinitionSet.assemblyReferencedAssemblies, assembly.GetReferencedAssemblies().Length.ToString());
            presenter.Add(DefinitionSet.assemblyCustomAttributes, assembly.GetCustomAttributes(false).Length.ToString());
            presenter.Add(DefinitionSet.assemblyTypes, assembly.GetTypes().Length.ToString());
            presenter.Add(DefinitionSet.assemblyExportedTypes, assembly.GetExportedTypes().Length.ToString());
            presenter.Add(DefinitionSet.assemblyForwardedTypes, assembly.GetForwardedTypes().Length.ToString());
            if (assembly.EntryPoint != null) {
                presenter.Add(DefinitionSet.assemblyEntryPointName, assembly.EntryPoint.Name);
                presenter.Add(DefinitionSet.assemblyEntryPointReturnType, assembly.EntryPoint.ReturnType.ToString());
                presenter.Add(DefinitionSet.assemblyEntryPointDeclaringType, assembly.EntryPoint.DeclaringType.FullName);
                presenter.Add(DefinitionSet.assemblyEntryPointParameters, assembly.EntryPoint.GetParameters().Length.ToString());
            } //if
        } //IPropertyPlugin.DiscoverProperties

    } //class Implementation

}
