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

    public abstract class StringAttribute : Attribute {
        public StringAttribute(string value) { this.value = value; }
        public StringAttribute(Type type) { this.type = type; }
        internal string Value { get { return value; } }
        internal Type Type { get { return type; } }
        #region implementation
        readonly string value;
        readonly Type type;
        #endregion implementation
    } //class StringAttribute

    /// <summary>
    /// This attributes provides StringAttributeUtility with data used to generate human-readable Display Name for enumeration members.
    /// The attribute can be applied to enumeration type and/or to individual enumeration members.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Enum, AllowMultiple = false, Inherited = false)]
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

    } //class DisplayNameAttribute

    /// <summary>
    /// This attributes provides StringAttributeUtility with data used to generate human-readable Description of enumeration members.
    /// The attribute can be applied to enumeration type and/or to individual enumeration members.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Enum, AllowMultiple = false, Inherited = false)]
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

    } //class DisplayNameAttribute

}
