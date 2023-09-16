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
    public 
        class EnumerationItemBase {

        internal protected EnumerationItemBase() { }

        private protected EnumerationItemBase(string name, AbbreviationLength abbreviationLength, string displayName, string description, Cardinal index, object value) {
            this.name = name;
            this.description = description;
            this.displayName = displayName;
            this.index = index;
            this.value = value;
            this.abbreviationLength = abbreviationLength;
            Debug.Assert(this.abbreviationLength > 0, "Abbreviation Length must be greater than zero");
            if (this.abbreviationLength < 1)
                this.abbreviationLength = 1;
        } //EnumerationItem

        /// <summary>
        /// Name of the static field representing enumeration item.
        /// </summary>
        public string Name { get { return name; } }

        /// <summary>
        /// Abbreviated name of the static field representing enumeration item; abbreviation is defined by the AbbreviationAttribute
        /// </summary>
        public string AbbreviatedName {
            get {
                if (abbreviatedName == null) //lazy
                    abbreviatedName = GetAbbreviatedName();
                return abbreviatedName;
            } //get AbbreviatedName
        } //AbbreviatedName

        /// <summary>
        /// Name of the item based on DisplayNameAttribute;
        /// the purpose of this member is to provide human-readable name for an item;
        /// it the attribute is not available or does not resolve the name, default value is used: DisplayName = Name 
        /// </summary>
        public string DisplayName { get { return displayName; } }

        /// <summary>
        /// Description of the item based on DescriptionAttribute;
        /// it the attribute is not available or does not resolve the name, default value is used: Description = null
        /// </summary>
        public string Description { get { return description; } }

        #region implementation

        string GetAbbreviatedName() {
            int len = name.Length;
            if (abbreviationLength >= len)
                return name;
            else
                return name.Substring(0, abbreviationLength);
        } //GetAbbreviatedName

        public object GenericEnumValue { get; private protected set; }

        private protected readonly Cardinal index;
        private protected readonly string name, displayName, description;
        private protected string abbreviatedName;
        private protected readonly AbbreviationLength abbreviationLength;
        private protected readonly object value;
        #endregion implementation

    } //struct EnumerationItem

}
