/* 
    Enumeration classes:
    Generic class Enumeration provides iteration capabilities based on enumeration or any other type with static fields;
    Generic class EnumerationIndexedArray implements enumeration-indexed arrays
    Generic class CartesianSquareIndexedArray implements indexed arrays indexed by a Cartesian Square based on an enumeration
    
    Copyright (C) 2008-2023 by Sergey A Kryukov
    http://www.SAKryukov.org
*/

namespace SA.Universal.Enumerations {
    using System;
    using System.Reflection;
    using StringBuilder = System.Text.StringBuilder;

    /// <summary>
    /// Utility class used to extract Display Name and Description information for enumeration members from enumeration metadata
    /// based on DisplayNameAttribute and DescriptionAttribute
    /// </summary>
    public static class StringAttributeUtility {

        /// <summary>
        /// Extract Description from enumeration meta-data;
        /// if System.Flags is applied and bitwise enumeration member combination is used, returns string representation of bitwise combination or underlying integer value
        /// </summary>
        /// <param name="value">Value of the type of any enumeration type</param>
        /// <returns>Enumeration member human-readable Display Name; if Display Name is not resolved, returns default name of enumeration member or underlying integer value</returns>
        public static string GetDisplayName(Enum value) {
            Type type = value.GetType();
            if (IsFlags(type))
                return GetFlaggedDisplayName(type, value);
            else
                return GetSimpleDisplayName(value);
        } //GetDisplayName

        /// <summary>
        /// Extract Description from enumeration meta-data
        /// </summary>
        /// <param name="value">Value of the type of any enumeration type</param>
        /// <returns>Description; if Description is not resolved, returns null</returns>
        public static string GetDescription(Enum value) {
            return ResolveValue<DescriptionAttribute>(value.GetType().GetField(value.ToString()));
        } //GetDescription

        /// <summary>
        /// Used internally to generate a container of EnumerationItem instances used for iteration through enumeration values
        /// </summary>
        /// <typeparam name="ATTRIBUTE_TYPE">DisplayNameAttribute and DescriptionAttribute</typeparam>
        internal static string ResolveValue<ATTRIBUTE_TYPE>(FieldInfo field) where ATTRIBUTE_TYPE : StringAttribute {
            if (field == null)
                return null;
            string value = ResolveValue<ATTRIBUTE_TYPE>(field.GetCustomAttributes(typeof(ATTRIBUTE_TYPE), false), field.Name);
            if (!string.IsNullOrEmpty(value))
                return value;
            //field attribute not found, looking for it type's attributes:
            return ResolveValue<ATTRIBUTE_TYPE>(field.DeclaringType.GetCustomAttributes(typeof(ATTRIBUTE_TYPE), false), field.Name);
        } //ResolveValue

        #region implementation

        static StringAttributeUtility() {
            flagDelimiter = (FlagDelimiterTest.Left | FlagDelimiterTest.Right).ToString().Replace(FlagDelimiterTest.Left.ToString(), string.Empty).Replace(FlagDelimiterTest.Right.ToString(), string.Empty);
        } //StringAttributeUtility

        static bool IsFlags(Type enumType) {
            object[] attributes = enumType.GetCustomAttributes(typeof(System.FlagsAttribute), false);
            return attributes.Length > 0;
        } //IsFlags

        static string GetSimpleDisplayName(Enum value) {
            string rawName = value.ToString();
            string name = ResolveValue<DisplayNameAttribute>(value.GetType().GetField(rawName));
            if (string.IsNullOrEmpty(name))
                return rawName;
            return name;
        } //GetSimpleDisplayName

        static string GetFlaggedDisplayName(Type enumType, Enum value) {
            object[] attributes = enumType.GetCustomAttributes(typeof(DisplayNameAttribute), false);
            string defaultString = value.ToString();
            string[] members = defaultString.Split(new string[] { flagDelimiter }, StringSplitOptions.RemoveEmptyEntries);
            for (int index = 0; index < members.Length; index++) {
                if (char.IsDigit(members[index][0]))
                    continue;
                string newValue = ResolveValue<DisplayNameAttribute>(attributes, members[index]);
                if (!string.IsNullOrEmpty(newValue))
                    members[index] = newValue;
            } //loop
            StringBuilder sb = new();
            for (int index = 0; index < members.Length; index++) {
                sb.Append(members[index]);
                if (index < members.Length - 1)
                    sb.Append(flagDelimiter);
            } //loop
            return sb.ToString();
        } //GetFlaggedDisplayName

        static string ResolveValue<ATTRIBUTE_TYPE>(object[] attributes, string memberName) where ATTRIBUTE_TYPE : StringAttribute {
            if (attributes == null) return null;
            if (attributes.Length < 1) return null;
            ATTRIBUTE_TYPE attribute = (ATTRIBUTE_TYPE)attributes[0];
            return ResolveValue(attribute, memberName);
        } //ResolveValue

        static string ResolveValue(StringAttribute attribute, string memberName) {
            string value = attribute.Value;
            if (!string.IsNullOrEmpty(value))
                return value;
            //immediate (hardcoded string) value not found, try using resources:
            Type resourceType = attribute.Type;
            if (resourceType == null)
                return null;
            BindingFlags bindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.GetProperty;
            PropertyInfo pi = resourceType.GetProperty(memberName, bindingFlags);
            if (pi == null)
                return null;
            object stringValue = pi.GetValue(null, Array.Empty<object>());
            if (stringValue == null)
                return null;
            return stringValue as string;
        } //ResolveValue

        readonly static string flagDelimiter;
        [Flags]
        private enum FlagDelimiterTest { Left = 1, Right = 2, } //used to calculate FlagDelimiter from sample

        #endregion implementation

    } //class StringAttributeUtility

}
