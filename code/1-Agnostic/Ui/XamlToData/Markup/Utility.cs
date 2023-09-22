﻿namespace SA.Agnostic.UI.Markup {
    using System;
    using System.Windows;
    using System.Reflection;
    using InstanceDictionary = System.Collections.Generic.Dictionary<System.Type, object>;
    using TypeConverter = System.ComponentModel.TypeConverter;
    using TypeConverterAttribute = System.ComponentModel.TypeConverterAttribute;
    using PatologicalList  = System.Collections.Generic.List<(object, object, System.Windows.ResourceDictionary)>;

    public static class ResourseDictionaryUtility {

        public static T_REQUIRED FindObject<T_REQUIRED>(ResourceDictionary dictionary) where T_REQUIRED : new() {
            if (dictionary == null) return default;
            foreach (object key in dictionary.Keys)
                if (dictionary[key] is T_REQUIRED required)
                    return required;
            foreach (ResourceDictionary child in dictionary.MergedDictionaries) {
                var found = FindObject<T_REQUIRED>(child);
                if (found != null)
                    return found;
            } //loop
            return default;
        } //FindObject

        public static T_REQUIRED GetObject<T_REQUIRED>(ResourceDictionary dictionary, TypeKeyKind keyKind = default)
            where T_REQUIRED : new() =>
                (T_REQUIRED)dictionary?[TypeKey.UserKey(typeof(T_REQUIRED), keyKind)];

        public static InstanceDictionary CollectDictionary(ResourceDictionary dictionary) {
            static void CollectDictionary(ResourceDictionary dictionary, InstanceDictionary instanceDictionary) {
                foreach (object key in dictionary.Keys)
                    CollectWithKey(dictionary, key, instanceDictionary);
                foreach (ResourceDictionary child in dictionary.MergedDictionaries)
                    CollectDictionary(child, instanceDictionary);
            } //CollectDictionary
            static void CollectWithKey(ResourceDictionary dictionary, object key, InstanceDictionary instanceDictionary) {
                object @object = dictionary[key];
                if (@object == null) return;
                Type objectType = @object.GetType();
                if (instanceDictionary.ContainsKey(objectType)) return;
                instanceDictionary.Add(objectType, @object);
            } //CollectWithKey
            if (dictionary == null) return default;
            InstanceDictionary instanceDictionary = new();
            CollectDictionary(dictionary, instanceDictionary);
            return instanceDictionary;
        } //CollectDictionary

        public static void NormalizeDictionary(ResourceDictionary dictionary) {
            PatologicalList list = new();
            static void GetAllKeys(ResourceDictionary top, PatologicalList list) {
                foreach (object key in top.Keys) {
                    if (key is not Type) continue;
                    object value = top[key];
                    if (value == null) continue;
                    Type valueType = value.GetType();
                    if ((Type)key == valueType) continue;
                    list.Add((key, value, top));
                } //loop
                foreach (ResourceDictionary child in top.MergedDictionaries)
                    GetAllKeys(child, list);
            } //GetAllKeys
            GetAllKeys(dictionary, list);
            foreach ((object key, object _, ResourceDictionary container) in list)
                container.Remove(key);
            foreach ((object _, object value, ResourceDictionary container) in list)
                container.Add(value.GetType(), value);
        } //NormalizeDictionary

        public static void CollectForDuckTypedInstance(ResourceDictionary dictionary, object instance, bool ignoreMissingMembers = false) {
            if (dictionary == null || instance == null) return;
            Type instanceType = instance.GetType();
            foreach (var key in dictionary.Keys) {
                    object value = dictionary[key];
                if (value is Member resourceSource)
                    CollectMember(resourceSource, key, instanceType, instance, ignoreMissingMembers);
                else if (value is DataSetter dataSetter)
                    CollectDataSetter(dataSetter, instance, instanceType, ignoreMissingMembers);
            } //keys loop
            foreach (ResourceDictionary child in dictionary.MergedDictionaries)
                CollectForDuckTypedInstance(child, instance, ignoreMissingMembers);
        } //CollectForDuckTypedInstance

        static void CollectDataSetter(DataSetter dataSetter, object instance, Type instanceType, bool ignoreMissingMembers) {
            foreach (Member member in dataSetter)
                AssignMember(member, member.Name, instance, instanceType, ignoreMissingMembers: ignoreMissingMembers);
        } //CollectDataSetter

        static void CollectMember(Member resourceSource, object key, Type instanceType, object instance, bool ignoreMissingMembers) {
            string memberName = resourceSource.Name;
            if (memberName == null && key is string keyMemberName)
                memberName = keyMemberName;
            AssignMember(resourceSource, memberName, instance, instanceType, ignoreMissingMembers: ignoreMissingMembers);
        } //CollectMember

        static void AssignMember(Member resourceSource, string memberName, object instance, Type instanceType, bool ignoreMissingMembers = false) {
            MemberKind memberKind = resourceSource.MemberKind;
            if (memberName == null) throw new DataTypeProviderException(DefinitionSet.missingMemberName);
            object memberValue = resourceSource.Value;
            if (memberValue == null) throw new DataTypeProviderException(DefinitionSet.missingMemberValue);
            Type memberType = resourceSource.Type;
            string stringMemberValue = null;
            if (memberValue is string stringCandidate)
                stringMemberValue = stringCandidate;
            bool isString = stringMemberValue != null;
            if (memberType == null)
                memberType = typeof(string);
            if (memberValue.GetType() != memberType && memberType.IsPrimitive)
                memberValue = Convert.ChangeType(memberValue, memberType);
            string resourceDictionaryName = typeof(ResourceDictionary).Name;
            if (memberKind == MemberKind.Field) {
                FieldInfo prefetchField = instanceType.GetField(memberName, DefaultFlagsPrefetch);
                if (prefetchField == null)
                    if (ignoreMissingMembers)
                        return;
                    else
                        throw new DataTypeProviderException(DefinitionSet.MissingField(memberName));
                if (!resourceSource.Static && prefetchField.IsStatic)
                    throw new DataTypeProviderException(DefinitionSet.StaticMismatch.FieldInstanceInDictionaryButStatic(
                        memberName, resourceDictionaryName, instanceType.Name));
                if (resourceSource.Static && !prefetchField.IsStatic)
                    throw new DataTypeProviderException(DefinitionSet.StaticMismatch.FieldStaticInDictionaryButInstance(
                        memberName, resourceDictionaryName, instanceType.Name));
                FieldInfo field = instanceType.GetField(memberName, resourceSource.Static ? DefaultFlagsStatic : DefaultFlags);
                if (field == null) throw new DataTypeProviderException(DefinitionSet.MissingField(memberName));
                if (isString)
                    memberValue = TryTypeConverter(field, memberType, stringMemberValue, memberValue);
                field.SetValue(instance, memberValue);
            } else {
                    PropertyInfo prefetchProperty = instanceType.GetProperty(memberName, DefaultFlagsPrefetch);
                if (prefetchProperty == null)
                    if (ignoreMissingMembers)
                        return;
                    else
                        throw new DataTypeProviderException(DefinitionSet.MissingProperty(memberName));
                if (!prefetchProperty.CanWrite)
                    throw new DataTypeProviderException(DefinitionSet.ReadOnlyProperty(prefetchProperty.Name));
                bool isStatic = prefetchProperty.SetMethod.IsStatic;
                if (!resourceSource.Static && isStatic)
                    throw new DataTypeProviderException(DefinitionSet.StaticMismatch.PropertyInstanceInDictionaryButStatic(
                        memberName, resourceDictionaryName, instanceType.Name));
                //"Property: Instance in ResourceDictionary, Static in class");
                if (resourceSource.Static && !isStatic)
                    throw new DataTypeProviderException(DefinitionSet.StaticMismatch.PropertyStaticInDictionaryButInstance(
                        memberName, resourceDictionaryName, instanceType.Name));
                PropertyInfo property = instanceType.GetProperty(memberName, resourceSource.Static ? DefaultFlagsStatic : DefaultFlags);
                if (property == null) throw new DataTypeProviderException(DefinitionSet.MissingProperty(memberName));
                if (isString)
                    memberValue = TryTypeConverter(property, memberType, stringMemberValue, memberValue);
                property.SetValue(instance, memberValue);
            } //if
        } //AssignMember

        static object TryTypeConverter(MemberInfo member, Type memberType, string stringMemberValue, object memberValue) {
            TypeConverterAttribute typeConverterAttribute = (TypeConverterAttribute)Attribute.GetCustomAttribute(memberType, typeof(TypeConverterAttribute));
            TypeConverterAttribute propertyConvertedAttribute = (TypeConverterAttribute)Attribute.GetCustomAttribute(member, typeof(TypeConverterAttribute));
            TypeConverterAttribute converterAttribute = typeConverterAttribute ?? propertyConvertedAttribute;
            if (converterAttribute != null) {
                Type converterType = Type.GetType(converterAttribute.ConverterTypeName);
                TypeConverter converter = (TypeConverter)Activator.CreateInstance(converterType);
                return converter.ConvertFromString(stringMemberValue);
            } //if
            return memberValue;
        } //TryTypeConverter

        class DataTypeProviderException : SystemException {
            internal DataTypeProviderException(string message) : base(message) { }
        } //class DataTypeProviderException

        static BindingFlags DefaultFlags =>
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        static BindingFlags DefaultFlagsStatic =>
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
        static BindingFlags DefaultFlagsPrefetch =>
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

    } //ResourseDictionaryUtility

}
