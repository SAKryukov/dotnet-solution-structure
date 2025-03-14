﻿/*
    Copyright (C) 2023-2025 by Sergey A Kryukov
    https://www.SAKryukov.org
    https://github.com/SAKryukov
*/

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

        internal static class StringFormat {
            internal const string regularExpression = @"{([^:}]*)(:?[^:}]*)}";
            internal const string toStringSeparator = ", ";
            internal static string FormalParameterDeclaration(string parameters) =>
                $"Formal parameters: {parameters}";
            internal static string BracketParameter(int parameterIndex, string subformat) =>
                $"{{{parameterIndex}{subformat}}}";
            internal static string InvalidParameterNumber(int formalParameters, int actualParameters) =>
                $"Invalid parameter number: required {formalParameters}, provided {actualParameters}";
            internal static string InvalidFormatStringAssignment(int formalParameters) =>
                $"Format string cannot be modified when formal parameters are already defined, number of formal parameters: {formalParameters}";
        } //class StringFormat

    } //class DefinitionSet

}
