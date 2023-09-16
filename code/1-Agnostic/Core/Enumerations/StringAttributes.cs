/* 
    Enumeration classes:
    Generic class Enumeration provides iteration capabilities based on enumeration or any other type with static fields;
    Generic class EnumerationIndexedArray implements enumeration-indexed arrays
    Generic class CartesianSquareIndexedArray implements indexed arrays indexed by a Cartesian Square based on an enumeration
    
    Copyright (C) 2008-2023 by Sergey A Kryukov
    http://www.SAKryukov.org
*/

namespace SA.Agnostic.Enumerations {
    using System;
    using FieldInfo = System.Reflection.FieldInfo;

    public interface IStringAttribute {
        (string name, string description) this[string name] { get; }
    } //interface IStringAttribute

    public abstract class StringAttribute : Attribute {
        public StringAttribute(string value) { this.value = value; }
        public StringAttribute(Type type) { this.type = type; }
        internal string Value { get { return value; } }
        internal Type Type { get { return type; } }
        #region implementation
        readonly string value;
        readonly Type type;
        #endregion implementation
        internal static string Resolve<ATTR>(FieldInfo fieldInfo, bool isName = false) where ATTR : StringAttribute {
            ATTR attribute = (ATTR)GetCustomAttribute(fieldInfo, typeof(ATTR));
            if (attribute == null) return null;
            if (attribute.Type == null && attribute.Value == null) return null;
            if (attribute.Type != null) {
                Type resourceType = attribute.Type;
                if (!resourceType.IsAssignableTo(typeof(IStringAttribute)))
                    return null;
                IStringAttribute implementor = (IStringAttribute)Activator.CreateInstance(resourceType);
                return isName ? implementor[fieldInfo.Name].name : implementor[fieldInfo.Name].description;
            } //if
            return attribute.Value;
        } //Resolve
    } //class StringAttribute

    /// <summary>
    /// This attributes provides StringAttributeUtility with data used to generate human-readable Display Name for enumeration members.
    /// The attribute can be applied to enumeration type and/or to individual enumeration members.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class DisplayNameAttribute : StringAttribute {

        /// <summary>
        /// This constructor should be used to specify Display Name for individual enumeration member
        /// </summary>
        /// <param name="value">Display Name for an individual enumeration member</param>
        public DisplayNameAttribute(string value) : base(value) { }

        /// <summary>
        /// This constructor should be used to specify Display Name for all or some members of enumeration or type an individual enumeration member
        /// </summary>
        /// <param name="type">Expects type of the resource class auto-generated when XML resource (.RESX) is created</param>
        public DisplayNameAttribute(Type type) : base(type) { }

        internal static string Resolve(FieldInfo fieldInfo) {
            return Resolve<DisplayNameAttribute>(fieldInfo, isName: true);
        } //Resolve

    } //class DisplayNameAttribute

    /// <summary>
    /// This attributes provides StringAttributeUtility with data used to generate human-readable Description of enumeration members.
    /// The attribute can be applied to enumeration type and/or to individual enumeration members.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class DescriptionAttribute : StringAttribute {

        /// <summary>
        /// This constructor should be used to specify Description for individual enumeration member
        /// </summary>
        /// <param name="value">Description for an individual enumeration member</param>
        public DescriptionAttribute(string value) : base(value) { }

        /// <summary>
        /// This constructor should be used to specify Description for all or some members of enumeration or type an individual enumeration member
        /// </summary>
        /// <param name="type">Expects type of the resource class auto-generated when XML resource (.RESX) is created</param>
        public DescriptionAttribute(Type type) : base(type) { }

        internal static string Resolve(FieldInfo fieldInfo) {
            return Resolve<DisplayNameAttribute>(fieldInfo, isName: true);
        } //Resolve

    } //class DisplayNameAttribute

    /// <summary>
    /// This attributes provides StringAttributeUtility with data used to generate human-readable Description of enumeration members.
    /// The attribute can be applied to enumeration type and/or to individual enumeration members.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Enum, AllowMultiple = false, Inherited = false)]
    public class DisplayTextProviderAttribute : StringAttribute {

        /// <summary>
        /// This constructor should be used to specify Description for all or some members of enumeration or type an individual enumeration member
        /// </summary>
        /// <param name="type">Expects type of the resource class auto-generated when XML resource (.RESX) is created</param>
        public DisplayTextProviderAttribute(Type type) : base(type) { }

        internal static (string displayName, string description) Resolve(Type enumType, FieldInfo fieldInfo) {
            DisplayTextProviderAttribute typeAttribute = (DisplayTextProviderAttribute)GetCustomAttribute(enumType, typeof(DisplayTextProviderAttribute));
            DisplayTextProviderAttribute fieldAttribute = (DisplayTextProviderAttribute)GetCustomAttribute(fieldInfo, typeof(DisplayTextProviderAttribute));
            if (typeAttribute == null && fieldAttribute == null)
                return (null, null);
            (string displayName, string description) valueByType = default, valueByField = default;
            if (typeAttribute != null)
                valueByType = GetFromResourceType(typeAttribute.Type, fieldInfo.Name);
            else
                valueByField = GetFromResourceType(fieldAttribute.Type, fieldInfo.Name);
            string finalDisplayName = valueByField.displayName ?? valueByType.displayName;
            string finalDescription = valueByField.description ?? valueByType.description;
            return (finalDisplayName, finalDescription);
            static (string displayName, string description) GetFromResourceType(Type resourceType, string fieldName) {
                if (resourceType == null)
                    return (null, null);
                if (!resourceType.IsAssignableTo(typeof(IStringAttribute)))
                    return (null, null);
                IStringAttribute implementor = (IStringAttribute)Activator.CreateInstance(resourceType);
                return (implementor[fieldName].name, implementor[fieldName].description);
            } //Type resourceType
        } //Resolve

    } //class DisplayTextProviderAttribute

}
