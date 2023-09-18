namespace SA.Test.Extensions {
    using SA.Agnostic.IO.Extensions;
    using System;
    using System.Reflection;
    using System.Windows;
    using InstanceDictionary = System.Collections.Generic.Dictionary<System.Type, object>;

    static class ResourseDictionaryUtility {

        public static InstanceDictionary Collect(ResourceDictionary dictionary) {
            if (dictionary == null) return null;
            InstanceDictionary instanceDictionary = new();
            foreach (var key in dictionary.Keys) {
                object value = dictionary[key];
                if (value is not KeyedMember resourceSource) continue;
                if (key is not EKey targetKey) continue;
                Type targetType = targetKey.TargetType;
                if (!instanceDictionary.TryGetValue(targetType, out object instance)) {
                    instance = Activator.CreateInstance(targetType);
                    instanceDictionary.Add(targetType, instance);
                } //if
                Assign(resourceSource, targetType, instance);
            } //keys loop
            return instanceDictionary;
        } //Collect

        public static void Collect(ResourceDictionary dictionary, object instance) {
            if (dictionary == null) return;
            Type instanceType = instance.GetType();
            foreach (var key in dictionary.Keys) {
                object value = dictionary[key];
                if (value is not KeyedMember resourceSource) continue;
                if (key is not EKey targetKey) continue;
                Type targetType = targetKey.TargetType;
                if (!targetType.IsAssignableTo(instanceType))
                    continue;
                Assign(resourceSource, targetType, instance);
            } //keys loop
        } //Collect

        public static void SimpleCollect(ResourceDictionary dictionary, object instance) {
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
                SimpleAssign(resourceSource, targetType, memberName, instance);
            } //keys loop
        } //SimpleCollect

        static void SimpleAssign(Member resourceSource, Type targetType, string memberName, object instance) {
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
                FieldInfo field = targetType.GetField(memberName);
                if (field == null) Raise($"Field {memberName} cannot be null");
                field.SetValue(instance, memberValue);
            } else {
                PropertyInfo property = targetType.GetProperty(memberName);
                if (property == null) Raise($"Property {memberName} cannot be null");
                property.SetValue(instance, memberValue);
            } //if
        } //SimpleAssign

        static void Assign(KeyedMember resourceSource, Type targetType, object instance) {
            if (targetType == null) Raise("Target Type cannot be null");
            MemberKind memberKind = resourceSource.MemberKind;
            string memberName = resourceSource.Name;
            if (memberName == null) Raise("Member Name cannot be null");
            object memberValue = resourceSource.Value;
            if (memberValue == null) Raise("Member Value cannot be null");
            Type memberType = resourceSource.Type;
            if (memberType == null)
                memberType = typeof(string);
            if (memberValue.GetType() != memberType)
                memberValue = Convert.ChangeType(memberValue, memberType);
            if (memberKind == MemberKind.Field) {
                FieldInfo field = targetType.GetField(memberName);
                if (field == null) Raise($"Field {memberName} cannot be null");
                field.SetValue(instance, memberValue);
            } else {
                PropertyInfo property = targetType.GetProperty(memberName);
                if (property == null) Raise($"Property {memberName} cannot be null");
                property.SetValue(instance, memberValue);
            } //if
        } //Assign

        static void Raise(string message) {
            throw new SystemException(message);
        } //Raise

    } //ResourseDictionaryUtility

}
