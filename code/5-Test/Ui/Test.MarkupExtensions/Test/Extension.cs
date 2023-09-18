namespace SA.Test.Extensions.Declarations {
    using System;
    using System.Windows;
    using System.Windows.Markup;
    using System.Reflection;
    using InstanceDictionary = System.Collections.Generic.Dictionary<System.Type, object>;

    public enum MemberKind { Field, Property }
    public class MemberSetter {
        public MemberKind MemberKind { get; set; }
        public Type MemberType { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }
    } //class MemberSetter

    public class EKey : TypeExtension {
        public Type TargetType { get; set; }
        public override object ProvideValue(IServiceProvider serviceProvider) {
            return this;
        } //ResourSet
    }
    public class Member : MarkupExtension {
        public Type Type { get; set; }
        public MemberKind MemberKind { get; set; }
        public string Name { get; set; }
        public ResourceDictionary Setters { get; set; }
        public object Value { get; set; }
        public override object ProvideValue(IServiceProvider serviceProvider) {
            return this;
        } //ProvideValue
        public static InstanceDictionary Collect(ResourceDictionary dictionary) {
            if (dictionary == null) return null;
            InstanceDictionary instanceDictionary = new();
            foreach (var key in dictionary.Keys) {
                object value = dictionary[key];
                if (value is not Member resourceSource) continue;
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
                if (value is not Member resourceSource) continue;
                if (key is not EKey targetKey) continue;
                Type targetType = targetKey.TargetType;
                if (!targetType.IsAssignableTo(instanceType))
                    continue;
                Assign(resourceSource, targetType, instance);
            } //keys loop
        } //Collect
        static void Assign(Member resourceSource, Type targetType, object instance) {
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
    } //class Member

}
