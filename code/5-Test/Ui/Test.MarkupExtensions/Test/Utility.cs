namespace SA.Test.Extensions {
    using SA.Agnostic.UI.Extensions;
    using System;
    using System.Reflection;
    using System.Windows;
    using InstanceDictionary = System.Collections.Generic.Dictionary<System.Type, object>;
    using TypeConverter = System.ComponentModel.TypeConverter;
    using TypeConverterAttribute = System.ComponentModel.TypeConverterAttribute;

    static class ResourseDictionaryUtility {

        class DataTypeProviderException : SystemException {
            internal DataTypeProviderException(string message) : base(message) { }
        } //class DataTypeProviderException

        static BindingFlags DefaultFlags =>
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        static BindingFlags DefaultFlagsStatic =>
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

        public static InstanceDictionary CollectDictionary(ResourceDictionary dictionary) {
            if (dictionary == null) return null;
            InstanceDictionary instanceDictionary = new();
            foreach (var key in dictionary.Keys) {
                object value = dictionary[key];
                if (value is not DataTypeProvider resourceSource) continue;
                if (key is not Type targetType) continue;
                if (!instanceDictionary.TryGetValue(targetType, out object instance)) {
                    instance = Activator.CreateInstance(targetType);
                    instanceDictionary.Add(targetType, instance);
                } //
                AssignInstanceMembers(resourceSource, targetType, instance);
            } //keys loop
            return instanceDictionary;
        } //CollectDictionary

        public static void CollectForInstance(ResourceDictionary dictionary, object instance) {
            if (dictionary == null) return;
            Type instanceType = instance.GetType();
            foreach (var key in dictionary.Keys) {
                object value = dictionary[key];
                if (value is not DataTypeProvider resourceSource) continue;
                if (key is not Type targetType) continue;
                if (!targetType.IsAssignableTo(instanceType))
                    continue;
                AssignInstanceMembers(resourceSource, targetType, instance);
            } //keys loop
        } //CollectForInstance

        public static void Collect(ResourceDictionary dictionary, object instance) {
            if (dictionary == null || instance == null) return;
            Type instanceType = instance.GetType();
            foreach (var key in dictionary.Keys) {
                object value = dictionary[key];
                if (value is not Member resourceSource) continue;
                if (key is not string memberName) continue;
                if (resourceSource.TargetType == null)
                    resourceSource.TargetType = instanceType;
                Type targetType = resourceSource.TargetType;
                if (targetType == null)
                    targetType = instance.GetType();
                if (!targetType.IsAssignableTo(instanceType))
                    continue;
                AssignMember(resourceSource, targetType, memberName, instance);
            } //keys loop
        } //Collect

        static void AssignMember(Member resourceSource, Type targetType, string memberName, object instance) {
            //Type converterType = typeof(TypeConverter);
            //TypeConverter converter = new();
            MemberKind memberKind = resourceSource.MemberKind;
            if (memberName == null) throw new DataMisalignedException("Member Name cannot be null");
            object memberValue = resourceSource.Value;
            if (memberValue == null) throw new DataMisalignedException("Member Value cannot be null");
            Type memberType = resourceSource.Type;
            ////SA???
            string stringMemberValue = null;
            if (memberValue is string stringCandidate) 
                stringMemberValue = stringCandidate;
            bool isString = stringMemberValue != null;
            ////SA???
            if (memberType == null)
                memberType = typeof(string);
            if (memberValue.GetType() != memberType && memberType.IsPrimitive)
                memberValue = Convert.ChangeType(memberValue, memberType);
            if (memberKind == MemberKind.Field) {
                FieldInfo field = targetType.GetField(memberName, resourceSource.Static ? DefaultFlagsStatic : DefaultFlags);
                if (field == null) throw new DataMisalignedException($"Field {memberName} cannot be null");
                if (isString)
                    memberValue = TryTypeConverter(field, memberType, stringMemberValue, memberValue);
                field.SetValue(instance, memberValue);
            } else {
                PropertyInfo property = targetType.GetProperty(memberName, resourceSource.Static ? DefaultFlagsStatic : DefaultFlags);
                if (property == null) throw new DataMisalignedException($"Property {memberName} cannot be null");
                if (!property.CanWrite)
                    throw new DataMisalignedException($"Property {property.Name} is read-only, and it cannot be populated");
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
                var converter = (TypeConverter)Activator.CreateInstance(converterType);
                return converter.ConvertFromString(stringMemberValue);
            } //if
            return memberValue;
        } //TryTypeConverter

        static void AssignInstanceMembers(DataTypeProvider resourceSource, Type targetType, object instance) {
            foreach (object childKey in resourceSource.Members.Keys) {
                if (childKey is not string memberName) continue;
                object childValue = resourceSource.Members[childKey];
                if (childValue is not Member member) throw new DataMisalignedException($"Members should have the type {typeof(Member).Name}, a member cannot be {childValue}");
                AssignMember(member, targetType, memberName, instance);
            } //loop
        } //AssignInstanceMembers

    } //ResourseDictionaryUtility

}
