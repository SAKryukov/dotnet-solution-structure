namespace SA.Agnostic {

    static class DefinitionSet {
        internal static string FormatAssemblyException(string assemblyLocation, string pluginImplementorAttributeName, string reason) =>
            $@"Assembly ""{assemblyLocation}"", {pluginImplementorAttributeName} problem: {reason}";
        internal static string FormatNonInterfaceException(string typeName) =>
            $"implementor interface type {typeName} should be interface";
        internal static string FormatNullInterfaceException(string pluginImplementorAttributeName) =>
            $"the {pluginImplementorAttributeName} implementor interface type cannot be null";
        internal static string FormatNullImplementorException(string typeName) =>
            $"the implementor of the interface {typeName} cannot be null";
        internal static string FormatNonClassImplementorException(string typeName) =>
            $"the implementor type {typeName} should be a class";
        internal static string FormatNonImplementorException(string implementorTypeName, string interfaceTypeName) =>
            $"the type {implementorTypeName} should implement the interface {interfaceTypeName}";
    } //class DefinitionSet

}
