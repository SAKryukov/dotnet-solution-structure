/*
    Copyright (C) 2023 by Sergey A Kryukov
    https://www.SAKryukov.org
    https://github.com/SAKryukov
    https://www.codeproject.com/Members/SAKryukov
*/

namespace SA.Test.Markup {

    static class DefinitionSet {

        internal const string localizationRequest = "Test localization?";

        internal static class StringInterpolation {
            internal const string title = "Demonstration of localized string interpolation:";
            internal const string organization = "Code Project";
            internal const ulong numberOfMembers = 15747139;
            internal static System.DateTime numberOfMembersOnTheDay = new(2023, 10, 1);
            internal const string standardLongDateSpecifier = "D";
            internal const string standardCardinalSpecifierWithGroupDelimiters = "N0";
        } //class StringInterpolation

    } //class DefinitionSet

}
