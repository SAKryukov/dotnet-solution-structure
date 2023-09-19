namespace SA.Agnostic.UI.Extensions {

    class DefinitionSet {

        internal const string missingMemberName = "Member Name cannot be null";
        internal const string missingMemberValue = "Member Value cannot be null";

        internal static string MissingField(string memberName) =>
            $"Field {memberName} cannot be null";
        internal static string MissingProperty(string memberName) =>
            $"Property {memberName} cannot be null";
        internal static string ReadOnlyProperty(string propertyName) =>
            $"Property {propertyName} is read-only, and it cannot be populated";

        internal static string WrongChildrenCollectionMember(string memberName, string childName) =>
            $"Members should have the type {memberName}, a member cannot be {childName}";

    } //class DefinitionSet

}
