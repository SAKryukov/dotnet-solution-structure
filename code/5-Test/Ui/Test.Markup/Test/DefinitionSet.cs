/*
    Copyright (C) 2023-2025 by Sergey A Kryukov
    https://www.SAKryukov.org
    https://github.com/SAKryukov
*/

namespace SA.Test.Markup {

    static class DefinitionSet {

        internal const string localizationRequest = "Test localization?";

        internal static class StringInterpolation {
            internal const string title = "Demonstration of localized string interpolation:";
            internal const string organization = "Code Project";
            internal const ulong numberOfMembers = 15747139;
            internal static System.DateTime numberOfMembersOnTheDay = new(2023, 10, 1);
        } //class StringInterpolation

        internal static class ReadonlyAccess {
            internal const string title = "Demonstration of read-only and pseudo-read-only properties:";
            internal const string attemptedNewValueAssignmentC = "new value";
            internal const string attemptedNewValueAssignmentD = "another new value";
            internal const string attemptedNewValueAssignmentF = "new value, stack validation";
        } //class ReadonlyAccess

    } //class DefinitionSet

}
