/*
    Enumeration classes:
    Generic class Enumeration provides iteration capabilities based on enumeration or any other type with static fields;
    Generic class EnumerationIndexedArray implements enumeration-indexed arrays
    Generic class CartesianSquareIndexedArray implements indexed arrays indexed by a Cartesian Square based on an enumeration
    
    Copyright (C) 2008-2023 by Sergey A Kryukov
    http://www.SAKryukov.org
*/

namespace SA.Agnostic.Enumerations {

    /// <summary>
    /// Generic class EnumerationIndexedArray implements enumeration-indexed arrays
    /// </summary>
    /// <typeparam name="INDEX">Type representing the type for array indices; enum type is recommended, but can be any type with any static fields used for indexing</typeparam>
    /// <typeparam name="ELEMENT">Type representing the type for array values</typeparam>
    public class EnumerationIndexedArray<INDEX, ELEMENT> {

        public EnumerationIndexedArray() {
            InitializeBody(default, false);
        } //EnumerationIndexedArray

        public EnumerationIndexedArray(ELEMENT sameInitialValue) {
            InitializeBody(sameInitialValue, true);
        } //EnumerationIndexedArray

        /// <summary>
        /// Indexed property used to manipulate array elements.
        /// </summary>
        /// <param name="index">INDEX value used to index the array; must have the same value as one of the static INDEX fields</param>
        /// <returns>Element of the array; may cause out-of-range exception:
        /// Indexing only works if the value passed as index is the same as one of the static INDEX values;
        /// otherwise it returns -1. For example if INDEX is System.Int32, this property works
        /// if index == System.Int32.MaxValue or index == System.Int32.MinValue and cause exception otherwise.
        /// </returns>
        public ELEMENT this[INDEX index] {
            get { return body[Enumeration<INDEX>.GetIntegerIndexFromEnumValue(index)]; }
            set { body[Enumeration<INDEX>.GetIntegerIndexFromEnumValue(index)] = value; }
        } //this

        #region implementation
        
        private ELEMENT[] body;
        private void InitializeBody(ELEMENT sameInitialValue, bool useInitialValue) {
            body = new ELEMENT[Enumeration<INDEX>.CollectionLength];
            if (!useInitialValue) return;
            for (int jj = 0; jj < body.Length; jj++)
                body[jj] = sameInitialValue;
        } //InitializeBody

        #endregion implementation

    } //EnumerationIndexedArray

}
