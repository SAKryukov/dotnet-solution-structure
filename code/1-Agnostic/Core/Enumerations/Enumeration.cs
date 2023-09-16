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
    using System.Collections;
    using System.Collections.Generic;
    using System.Reflection;
    using Cardinal = System.UInt32;
    using AbbreviationLength = System.Byte;

    /// <summary>
    /// NonEnumerableAttribute can be applied to an Enum field to exclude it from iteration sequence
    /// when used as a in the generic class <see cref="Enumeration"/>.
    /// This attribute can be applied to any other static field of any type, with the same effect.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class NonEnumerableAttribute : Attribute { }

    /// <summary>
    /// Abbreviation attribute allows to specify number of characters in the abbreviated string representing enumeration member name used in command line
    /// (<seealso cref="Utilities.CommandLine<SWITCHES, VALUES>"/>)
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class AbbreviationAttribute : Attribute {
        public AbbreviationAttribute() { abbreviationLength = 1; }
        public AbbreviationAttribute(AbbreviationLength length) { abbreviationLength = length; }
        public AbbreviationLength AbbreviationLength { get { return abbreviationLength; } }
        readonly AbbreviationLength abbreviationLength;
    } //class AbbreviationAttribute

    /// <summary>
    /// Class supporting interface IEnumerable
    /// to allow of iterations through the set of public static fields of the type parameter in natural order,
    /// that is, in the order the public static fields appear in the declaration (alrernatively, in reverse order)
    /// </summary>
    /// <typeparam name="ENUM">There is no constraint on this type; for most typical application this is an enumeration type</typeparam>
    public class Enumeration<ENUM> : IEnumerable<EnumerationItem<ENUM>> {

        public Enumeration(bool refresh = false) {
            BuildEnumerationCollection(refresh: refresh);
            enumeratorInstance = new Enumerator(this);
        } //Enumeration

        public Enumeration() {
            BuildEnumerationCollection(refresh: false);
            enumeratorInstance = new Enumerator(this);
        } //Enumeration

        public static Cardinal CollectionLength {
            get {
                BuildEnumerationCollection();
                return collectionLength;
            } //get CollectionLength
        } //CollectionLength
        public static ENUM First {
            get {
                BuildEnumerationCollection();
                if (collectionLength < 1)
                    return default;
                else
                    return enumerationCollection[0].EnumValue;
            } //get First
        } //First
        public static ENUM Last {
            get {
                BuildEnumerationCollection();
                if (collectionLength < 1)
                    return default;
                else
                    return enumerationCollection[collectionLength - 1].EnumValue;
            } //get Last
        } //Last

        public EnumerationItem<ENUM> this[Cardinal index] { get { return enumerationCollection[index]; } }
        public bool IsReverse { get { return reverse; } set { reverse = value; } }

        public bool Reverse(bool reset) {
            IsReverse = !reverse;
            if (reset)
                EnumeratorInstance.Reset();
            return reverse;
        } //Reverse

        #region implementation

        IEnumerator<EnumerationItem<ENUM>> IEnumerable<EnumerationItem<ENUM>>.GetEnumerator() { return EnumeratorInstance; }
        IEnumerator IEnumerable.GetEnumerator() { return EnumeratorInstance; }

        readonly IEnumerator<EnumerationItem<ENUM>> enumeratorInstance;
        IEnumerator<EnumerationItem<ENUM>> EnumeratorInstance {
            get {
                enumeratorInstance.Reset();
                return enumeratorInstance;
            } //get EnumeratorInstance
        } //EnumeratorInstance

        bool reverse;

        private class Enumerator : IEnumerator, IEnumerator<EnumerationItem<ENUM>> {
            internal Enumerator(Enumeration<ENUM> owner) { Owner = owner; }
            object IEnumerator.Current { get { return enumerationCollection[Owner.collectionCurrent]; } }
            bool IEnumerator.MoveNext() {
                if (collectionLength < 1) return false;
                if (Owner.reverse) {
                    if (!Owner.belowZero && Owner.collectionCurrent > 0) {
                        Owner.collectionCurrent--;
                        return true;
                    } else {
                        Owner.collectionCurrent = 0;
                        Owner.belowZero = true;
                    } //if
                } else { //forward:
                    if (Owner.belowZero) {
                        Owner.collectionCurrent = 0;
                        Owner.belowZero = false;
                        return true;
                    } else if (Owner.collectionCurrent + 1 < collectionLength) {
                        Owner.collectionCurrent++;
                        return true;
                    } //if
                } //if forward
                return false;
            } //IEnumerator.MoveNext
            void IDisposable.Dispose() { }
            EnumerationItem<ENUM> IEnumerator<EnumerationItem<ENUM>>.Current { get { return enumerationCollection[Owner.collectionCurrent]; } }
            void IEnumerator.Reset() {
                Owner.belowZero = !Owner.reverse;
                if (Owner.reverse)
                    Owner.collectionCurrent = collectionLength;
                else
                    Owner.collectionCurrent = 0;
            } //IEnumerator.Reset
            internal protected Enumeration<ENUM> Owner;
        } //class Enumerator

        delegate void BuildAction();

        static void BuildEnumerationCollection(bool refresh = false) {
            if (!refresh && enumerationCollection != null) return;
            BuildEnumerationCollectionCore();
        } //BuildEnumerationCollection

        static void BuildEnumerationCollectionCore() {
            Type type = typeof(ENUM);
            bool isEnum = type.IsEnum;
            FieldInfo[] fields = GetStaticFields(type);
            List<EnumerationItem<ENUM>> list = new ();
            Cardinal currentIndex = 0;
            for (Cardinal jj = 0; jj < (Cardinal)fields.Length; jj++) {
                FieldInfo field = fields[jj];
                object[] attributes = field.GetCustomAttributes(typeof(NonEnumerableAttribute), false);
                if (attributes.Length > 0) continue;
                object objValue = field.GetValue(null); //boxed if ENUM is primitive
                if (objValue == null) continue;
                ENUM enumValue = default;
                if (isEnum)
                    enumValue = (ENUM)objValue;
                else {
                    if (objValue is ENUM eNUM) //this object-oriented dynamic check always works event of ENUM is primitive type because objValue is boxed object
                        enumValue = eNUM;
                } //if not enum
                string name = field.Name;
                attributes = field.GetCustomAttributes(typeof(AbbreviationAttribute), false);
                AbbreviationLength abbreviationLength = AbbreviationLength.MaxValue;
                if (attributes.Length > 0) {
                    AbbreviationAttribute attr = (AbbreviationAttribute)attributes[0];
                    abbreviationLength = attr.AbbreviationLength;
                } //if Abbreviation works
                (string displayName, string description) = GetDisplay(type, field);
                list.Add(new EnumerationItem<ENUM>(name, abbreviationLength, displayName, description, currentIndex, objValue, enumValue));
                currentIndex++;
            } //loop
            enumerationCollection = list.ToArray();
            collectionLength = (Cardinal)enumerationCollection.Length;
        } //BuildEnumerationCollectionCore

        static (string name, string description) GetDisplay(Type type, FieldInfo field) {
            (string displayName, string description) =
                DisplayTextProviderAttribute.Resolve(type, field);
            string finalDisplayName = DisplayNameAttribute.Resolve(field);
            if (finalDisplayName == null)
                finalDisplayName = displayName;
            if (finalDisplayName == null)
                finalDisplayName = field.Name;
            string finalDescription = DescriptionAttribute.Resolve(field);
            if (finalDescription == null)
                finalDescription = description;
            return (finalDisplayName, finalDescription);
        } //GetDisplay

        /// <summary>
        /// BuildIndexDictionary only used to support EnumerationIndexedArray via GetIntegerIndexFromEnumValue;
        /// If nobody calls GetIntegerIndexFromEnumValue, IndexDictionary remains null
        /// </summary>
        static void BuildIndexDictionary() {
            if (indexDictionary != null) return;
            BuildIndexDictionaryCore();
        } //BuildIndexDictionary

        static void BuildIndexDictionaryCore() {
            BuildEnumerationCollection();  //lazy evaluation: does nothing if already built
            indexDictionary = new Dictionary<ENUM, uint>();
            for (Cardinal jj = 0; jj < collectionLength; jj++) {
                ENUM dictionaryKey = enumerationCollection[jj].EnumValue;
                if (!indexDictionary.ContainsKey(dictionaryKey))
                    indexDictionary.Add(enumerationCollection[jj].EnumValue, jj);
            } //loop
        } //BuildIndexDictionaryCore

        static FieldInfo[] GetStaticFields(Type type) {
            return type.GetFields(BindingFlags.Static | BindingFlags.Public);
        } //GetStaticFields

        Cardinal collectionCurrent;
        bool belowZero = true;
        static Cardinal collectionLength;
        static EnumerationItem<ENUM>[] enumerationCollection; //only used to support EnumerationIndexedArray via GetIntegerIndexFromEnumValue
        static Dictionary<ENUM, Cardinal> indexDictionary;

        #endregion implementation

        #region internal implementation for EnumerationIndexedArray

        /// <summary>
        /// GetIntegerIndexFromEnumValue only used to support EnumerationIndexedArray;
        /// it retrieve integer index of a static INDEX field based on sone INDEX value;
        /// It only works if the value passed as index is the same as one of the static INDEX values;
        /// otherwise it returns -1. For example if INDEX is System.Int32, this function returns 0
        /// if index == System.Int32.MaxValue, 1 if index == System.Int32.MinValue and 0 otherwise.
        /// </summary>
        /// <param name="index">INDEX value used to retrieve the index of a static field in INDEX</param>
        /// <returns>Integer index of INDEX static field</returns>
        internal static int GetIntegerIndexFromEnumValue(ENUM index) {
            BuildIndexDictionary(); //lazy evaluation: does nothing if already built
            if (indexDictionary.TryGetValue(index, out uint intIndex))
                return (int)intIndex;
            else
                return -1;
        } //GetIntegerIndexFromEnumValue

        #endregion internal implementation for EnumerationIndexedArray

    } //class Enumeration

}
