namespace SA.Plugin.View {
    using System;
    using System.Windows;

    public partial class Box : Window {

        public Box() {
            InitializeComponent();
            svg = new();
            grid.Children.Add(svg);
        } //Box

        protected override void OnContentRendered(EventArgs eventArgs) {
            base.OnContentRendered(eventArgs);
            svg.Animate();
        } //OnContentRendered

        readonly SVG svg;

    } //class Box

}
