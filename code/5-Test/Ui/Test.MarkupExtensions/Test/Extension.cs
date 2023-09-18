namespace SA.Test.Extensions.Declarations {
    using System;
    using System.Windows;
    using System.Windows.Markup;
    using SetterList = System.Collections.Generic.List<MemberSetter>;

    public enum MemberKind { Field, Property }
    public class MemberSetter {
        public MemberKind MemberKind { get; set; }
        public Type MemberType { get; set; }
        public string Name { get; set; }
        public object Value { get; set; }
    } //class MemberSetter

    public class ResourceSource : MarkupExtension, IAddChild {
        void IAddChild.AddChild(object value) {
            //Setters.Add((MemberSetter)value);
        }
        void IAddChild.AddText(string text) { }
        public ResourceSource() {
            Setters = new();
        }
        public Type TargetType { get; set; }
        public ResourceDictionary Setters { get; set; }
        public override object ProvideValue(IServiceProvider serviceProvider) {
            return Setters;
        } //ResourSet
    } //class ResourceSource

    internal class ResourceSet {
        //ResourceDictionary = 
    } //class ResourceSet


}