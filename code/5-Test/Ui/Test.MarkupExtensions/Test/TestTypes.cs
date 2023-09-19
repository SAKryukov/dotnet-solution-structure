﻿namespace SA.Test.Extensions {
    using System.Windows;
    using System.Windows.Media;

    internal class ResourceTarget {
        public Thickness Thickness { get; set; }
        public Color Color { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Count = default;
    } //class ResourceTarget

    internal class ResourceTarget2 {
        public Thickness Thickness2 = default;
        public string Name2 { get; set; }
        public string Description2 { get; set; }
        readonly int count2 = default;
        public int PublicCount => count2;
    } //class ResourceTarget

}
