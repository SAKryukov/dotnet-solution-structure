/* 
    Enumeration classes:
    Generic class Enumeration provides iteration capabilities based on enumeration or any other type with static fields;
    Generic class EnumerationIndexedArray implements enumeration-indexed arrays
    Generic class CartesianSquareIndexedArray implements indexed arrays indexed by a Cartesian Square based on an enumeration
    
    Copyright (C) 2008-2023 by Sergey A Kryukov
    http://www.SAKryukov.org
*/

namespace SA.Agnostic.Enumerations {
    using Cardinal = System.UInt32;
    using AbbreviationLength = System.Byte;
    using Debug = System.Diagnostics.Debug;

    /// <summary>
    /// EnumerationItem play the same role in <seealso cref="Enumeration">Enumeration</seealso> class as enum members do in their parent enum types.
    /// EnumerationItem provides more comprehensive information and resolves the situation when different enum members have same integer values.
    /// </summary>
    /// <typeparam name="ENUM">Any type; however, only the set of the public static fields of the type are essential</typeparam>
    public sealed class EnumerationItem<ENUM> : EnumerationItemBase {
        
        private EnumerationItem() { }

        internal EnumerationItem(string name, AbbreviationLength abbreviationLength, string displayName, string description, Cardinal index, object value, ENUM enumValue)
        : base(name, abbreviationLength, displayName, description, index, value) {
            this.enumValue = enumValue;
            GenericEnumValue = enumValue;
        } //EnumerationItem

        /// <summary>
        /// Value of the static field representing enumeration member.
        /// If ENUM is not an enumeration type, the type of a static field corresponding to present instance of EnumerationItem may be of the type other then ENUN;
        /// in this case EnumValue is assigned to default(ENUM).
        /// </summary>
        public ENUM EnumValue { get { return enumValue; } }

        #region implementation

        readonly ENUM enumValue;

        #endregion implementation

    } //struct EnumerationItem

}
