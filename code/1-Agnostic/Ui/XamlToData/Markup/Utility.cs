/*
    Copyright (C) 2023 by Sergey A Kryukov
    https://www.SAKryukov.org
    https://github.com/SAKryukov
    https://www.codeproject.com/Members/SAKryukov
*/

namespace SA.Agnostic.UI.Markup {
    using System;
    using System.Windows;
    using System.Reflection;
    using InstanceDictionary = System.Collections.Generic.Dictionary<System.Type, object>;
    using TypeConverter = System.ComponentModel.TypeConverter;
    using TypeConverterAttribute = System.ComponentModel.TypeConverterAttribute;
    using PatologicalList  = System.Collections.Generic.List<(object, object, System.Windows.ResourceDictionary)>;

    public static class ResourseDictionaryUtility {

        public static T_REQUIRED GetObject<T_REQUIRED>(ResourceDictionary dictionary)
            where T_REQUIRED : new() =>
                (T_REQUIRED)dictionary?[typeof(T_REQUIRED)];

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
                if (key is not string memberName)
                    throw new DataTypeProviderException(DefinitionSet.NonStringKeyMemberName(
                        key.GetType().FullName,
                        typeof(Member).Name));
                object value = dictionary[key];
                if (value is Member memberDeclaration)
                    AssignMember(memberDeclaration, memberName, instance, instanceType, ignoreMissingMembers);
            } //keys loop
            foreach (ResourceDictionary child in dictionary.MergedDictionaries)
                CollectForDuckTypedInstance(child, instance, ignoreMissingMembers);
        } //CollectForDuckTypedInstance

        static void AssignMember(Member memberDeclaration, string memberName, object instance, Type instanceType, bool ignoreMissingMembers = false) {
            MemberKind memberKind = memberDeclaration.MemberKind;
            if (memberName == null) throw new DataTypeProviderException(DefinitionSet.missingMemberName);
            object memberValue = memberDeclaration.Value;
            if (memberValue == null) throw new DataTypeProviderException(DefinitionSet.missingMemberValue);
            Type memberType = memberDeclaration.Type;
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
                if (!memberDeclaration.Static && prefetchField.IsStatic)
                    throw new DataTypeProviderException(DefinitionSet.StaticMismatch.FieldInstanceInDictionaryButStatic(
                        memberName, resourceDictionaryName, instanceType.Name));
                if (memberDeclaration.Static && !prefetchField.IsStatic)
                    throw new DataTypeProviderException(DefinitionSet.StaticMismatch.FieldStaticInDictionaryButInstance(
                        memberName, resourceDictionaryName, instanceType.Name));
                FieldInfo field = instanceType.GetField(memberName, memberDeclaration.Static ? DefaultFlagsStatic : DefaultFlags);
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
                if (!memberDeclaration.Static && isStatic)
                    throw new DataTypeProviderException(DefinitionSet.StaticMismatch.PropertyInstanceInDictionaryButStatic(
                        memberName, resourceDictionaryName, instanceType.Name));
                //"Property: Instance in ResourceDictionary, Static in class");
                if (memberDeclaration.Static && !isStatic)
                    throw new DataTypeProviderException(DefinitionSet.StaticMismatch.PropertyStaticInDictionaryButInstance(
                        memberName, resourceDictionaryName, instanceType.Name));
                PropertyInfo property = instanceType.GetProperty(memberName, memberDeclaration.Static ? DefaultFlagsStatic : DefaultFlags);
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
