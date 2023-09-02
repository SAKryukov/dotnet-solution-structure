namespace SA.Plugin.View {
    using System.Windows;

    public partial class Box : Window {
    
        public Box() {
            InitializeComponent();
            grid.Children.Add(new SVG());
        } //Box

    } //class Box

}
