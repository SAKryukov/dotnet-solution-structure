namespace SA.Agnostic.UI.Markup {

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

        internal static class StaticMismatch {
            internal static string PropertyStaticInDictionaryButInstance(string memberName, string dictionaryClassName, string targeTypeName) =>
                $"Property {memberName} is declared as static in {dictionaryClassName}, but is an instance property in {targeTypeName}";
            internal static string PropertyInstanceInDictionaryButStatic(string memberName, string dictionaryClassName, string targeTypeName) =>
                $"Property {memberName} is declared as instance in {dictionaryClassName}, but is a static property in {targeTypeName}";
            internal static string FieldStaticInDictionaryButInstance(string memberName, string dictionaryClassName, string targeTypeName) =>
                $"Field {memberName} is declared as static in {dictionaryClassName}, but is an instance field in {targeTypeName}";
            internal static string FieldInstanceInDictionaryButStatic(string memberName, string dictionaryClassName, string targeTypeName) =>
                $"Field {memberName} is declared as instance in {dictionaryClassName}, but is a static field in {targeTypeName}";
        } //StaticMismatch

    } //class DefinitionSet

}
