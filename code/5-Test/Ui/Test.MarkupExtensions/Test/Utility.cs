namespace SA.Test.Extensions {
    using SA.Agnostic.IO.Extensions;
    using System;
    using System.Reflection;
    using System.Windows;
    using InstanceDictionary = System.Collections.Generic.Dictionary<System.Type, object>;

    static class ResourseDictionaryUtility {

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
            MemberKind memberKind = resourceSource.MemberKind;
            if (memberName == null) Raise("Member Name cannot be null");
            object memberValue = resourceSource.Value;
            if (memberValue == null) Raise("Member Value cannot be null");
            Type memberType = resourceSource.Type;
            if (memberType == null)
                memberType = typeof(string);
            if (memberValue.GetType() != memberType)
                memberValue = Convert.ChangeType(memberValue, memberType);
            if (memberKind == MemberKind.Field) {
                FieldInfo field = targetType.GetField(memberName, resourceSource.Static ? DefaultFlagsStatic : DefaultFlags);
                if (field == null) Raise($"Field {memberName} cannot be null");
                field.SetValue(instance, memberValue);
            } else {
                PropertyInfo property = targetType.GetProperty(memberName, resourceSource.Static ? DefaultFlagsStatic : DefaultFlags);
                if (property == null) Raise($"Property {memberName} cannot be null");
                property.SetValue(instance, memberValue);
            } //if
        } //AssignMember

        static void AssignInstanceMembers(DataTypeProvider resourceSource, Type targetType, object instance) {
            foreach (object childKey in resourceSource.Children.Keys) {
                if (childKey is not string memberName) continue;
                object childValue = resourceSource.Children[childKey];
                if (childValue == null) continue; //SA???
                if (childValue is not Member member) continue; //SA???
                AssignMember(member, targetType, memberName, instance);
            } //loop
        } //AssignInstanceMembers

        static void Raise(string message) {
            throw new SystemException(message);
        } //Raise

    } //ResourseDictionaryUtility

}
