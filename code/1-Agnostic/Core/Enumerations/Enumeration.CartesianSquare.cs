/*
    Enumeration classes:
    Generic class Enumeration provides iteration capabilities based on enumeration or any other type with static fields;
    Generic class EnumerationIndexedArray implements enumeration-indexed arrays
    Generic class CartesianSquareIndexedArray implements indexed arrays indexed by a Cartesian Square based on an enumeration
    
    Copyright (C) 2008-2023 by Sergey A Kryukov
    http://www.SAKryukov.org
*/

namespace SA.Universal.Enumerations {

    /// <summary>
    /// Generic class CartesianSquareIndexedArray implements indexed arrays indexed by a Cartesian Square based on an enumeration
    /// </summary>
    /// <typeparam name="INDEX">Type representing the type for array indices; enum type is recommended, but can be any type with any static fields used for indexing</typeparam>
    /// <typeparam name="ELEMENT">Type representing the type for array values</typeparam>
    public class CartesianSquareIndexedArray<INDEX, ELEMENT> {

        public CartesianSquareIndexedArray() {
            InitializeBody(default, false);
        } //CartesianSquareIndexedArray

        public CartesianSquareIndexedArray(ELEMENT sameInitialValue) {
            InitializeBody(sameInitialValue, true);
        } //CartesianSquareIndexedArray

        /// <summary>
        /// Indexed property used to manipulate array elements.
        /// </summary>
        /// <returns>Element of the array; may cause out-of-range exception:
        /// Indexing only works if the value passed as index is the same as one of the static INDEX values;
        /// otherwise it returns -1. For example if INDEX is System.Int32, this property works
        /// if index == System.Int32.MaxValue or index == System.Int32.MinValue and cause exception otherwise.
        /// </returns>
        public ELEMENT this[INDEX from, INDEX to] {
            get { return body[Enumeration<INDEX>.GetIntegerIndexFromEnumValue(from), Enumeration<INDEX>.GetIntegerIndexFromEnumValue(to)]; }
            set { body[Enumeration<INDEX>.GetIntegerIndexFromEnumValue(from), Enumeration<INDEX>.GetIntegerIndexFromEnumValue(to)] = value; }
        } //this

        #region implementation

        private ELEMENT[,] body;
        private void InitializeBody(ELEMENT sameInitialValue, bool useInitialValue) {
            uint len = Enumeration<INDEX>.CollectionLength;
            body = new ELEMENT[len, len];
            if (!useInitialValue) return;
            for (int xx = 0; xx < len; xx++)
                for (int yy = 0; yy < body.Length; yy++)
                    body[xx, yy] = sameInitialValue;
        } //InitializeBody

        #endregion implementation

    } //CartesianSquareIndexedArray

}
