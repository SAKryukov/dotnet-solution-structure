namespace SA.Agnostic.UI.Markup {
    using System;
    using System.Windows;
    using System.Reflection;
    using InstanceDictionary = System.Collections.Generic.Dictionary<System.Type, object>;
    using TypeConverter = System.ComponentModel.TypeConverter;
    using TypeConverterAttribute = System.ComponentModel.TypeConverterAttribute;

    public static class ResourseDictionaryUtility {

        class DataTypeProviderException : System.SystemException {
            internal DataTypeProviderException(string message) : base(message) { }
        } //class DataTypeProviderException

        public static T_REQUIRED FindObject<T_REQUIRED>(ResourceDictionary dictionary) where T_REQUIRED : new() {
            if (dictionary == null) return default;
            foreach (object key in dictionary.Keys)
                if (dictionary[key] is T_REQUIRED required)
                    return required;
            foreach (ResourceDictionary child in dictionary.MergedDictionaries)
                return FindObject<T_REQUIRED>(child);
            return default;
        } //FindObject

        public static T_REQUIRED GetObject<T_REQUIRED>(ResourceDictionary dictionary) where T_REQUIRED : new() =>
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
                System.Type objectType = @object.GetType();
                if (instanceDictionary.ContainsKey(objectType)) return;
                instanceDictionary.Add(objectType, @object);
            } //CollectWithKey
            if (dictionary == null) return default;
            InstanceDictionary instanceDictionary = new();
            CollectDictionary(dictionary, instanceDictionary);
            return instanceDictionary;
        } //CollectDictionary

        static BindingFlags DefaultFlags =>
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        static BindingFlags DefaultFlagsStatic =>
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
        static BindingFlags DefaultFlagsPrefetch =>
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

        public static void CollectForInstance(ResourceDictionary dictionary, object instance) {
            if (dictionary == null) return;
            Type instanceType = instance.GetType();
            foreach (var key in dictionary.Keys) {
                object value = dictionary[key];
                if (value is not DataSetter resourceSource) continue;
                if (key is not Type targetType) continue;
                if (!targetType.IsAssignableTo(instanceType))
                    continue;
                AssignInstanceMembers(resourceSource, targetType, instance);
            } //keys loop
        } //CollectForInstance

        public static InstanceDictionary CollectDictionaryOldWay(ResourceDictionary dictionary) {
            if (dictionary == null) return null;
            InstanceDictionary instanceDictionary = new();
            foreach (var key in dictionary.Keys) {
                object value = dictionary[key];
                if (value is not DataSetter resourceSource) continue;
                if (key is not Type targetType) continue;
                if (!instanceDictionary.TryGetValue(targetType, out object instance)) {
                    instance = Activator.CreateInstance(targetType);
                    instanceDictionary.Add(targetType, instance);
                } //
                AssignInstanceMembers(resourceSource, targetType, instance);
            } //keys loop
            return instanceDictionary;
        } //CollectDictionaryOldWay //SA???

        public static void CollectForDuckTypedInstance(ResourceDictionary dictionary, object instance) {
            if (dictionary == null || instance == null) return;
            Type instanceType = instance.GetType();
            foreach (var key in dictionary.Keys) {
                object value = dictionary[key];
                if (value is not Member resourceSource) continue;
                string memberName = resourceSource.Name;
                if (memberName == null && key is string keyMemberName)
                    memberName = keyMemberName;
                if (resourceSource.TargetType == null)
                    resourceSource.TargetType = instanceType;
                Type targetType = resourceSource.TargetType;
                if (targetType == null)
                    targetType = instance.GetType();
                if (!targetType.IsAssignableTo(instanceType))
                    continue;
                AssignMember(resourceSource, targetType, memberName, instance);
            } //keys loop
        } //CollectForDuckTypedInstance

        static void AssignMember(Member resourceSource, Type targetType, string memberName, object instance) {
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
                FieldInfo prefetchField = targetType.GetField(memberName, DefaultFlagsPrefetch);
                if (prefetchField == null) throw new DataTypeProviderException(DefinitionSet.MissingField(memberName));
                if (!resourceSource.Static && prefetchField.IsStatic)
                    throw new DataTypeProviderException(DefinitionSet.StaticMismatch.FieldInstanceInDictionaryButStatic(
                        memberName, resourceDictionaryName, targetType.Name));
                if (resourceSource.Static && !prefetchField.IsStatic)
                    throw new DataTypeProviderException(DefinitionSet.StaticMismatch.FieldStaticInDictionaryButInstance(
                        memberName, resourceDictionaryName, targetType.Name));
                FieldInfo field = targetType.GetField(memberName, resourceSource.Static ? DefaultFlagsStatic : DefaultFlags);
                if (field == null) throw new DataTypeProviderException(DefinitionSet.MissingField(memberName));
                if (isString)
                    memberValue = TryTypeConverter(field, memberType, stringMemberValue, memberValue);
                field.SetValue(instance, memberValue);
            } else {
                PropertyInfo prefetchProperty = targetType.GetProperty(memberName, DefaultFlagsPrefetch);
                if (prefetchProperty == null) throw new DataTypeProviderException(DefinitionSet.MissingProperty(memberName));
                if (!prefetchProperty.CanWrite)
                    throw new DataTypeProviderException(DefinitionSet.ReadOnlyProperty(prefetchProperty.Name));
                bool isStatic = prefetchProperty.SetMethod.IsStatic;
                if (!resourceSource.Static && isStatic)
                    throw new DataTypeProviderException(DefinitionSet.StaticMismatch.PropertyInstanceInDictionaryButStatic(
                        memberName, resourceDictionaryName, targetType.Name));
                //"Property: Instance in ResourceDictionary, Static in class");
                if (resourceSource.Static && !isStatic)
                    throw new DataTypeProviderException(DefinitionSet.StaticMismatch.PropertyStaticInDictionaryButInstance(
                        memberName, resourceDictionaryName, targetType.Name));
                PropertyInfo property = targetType.GetProperty(memberName, resourceSource.Static ? DefaultFlagsStatic : DefaultFlags);
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


        static void AssignInstanceMembers(DataSetter resourceSource, Type targetType, object instance) {
            foreach (object child in resourceSource.Members) {
                if (child is not Member member)
                    throw new DataTypeProviderException(
                        DefinitionSet.WrongChildrenCollectionMember(typeof(Member).Name, child.ToString()));
                AssignMember(member, targetType, member.Name, instance);
            } //loop
        } //AssignInstanceMembers

    } //ResourseDictionaryUtility

}
