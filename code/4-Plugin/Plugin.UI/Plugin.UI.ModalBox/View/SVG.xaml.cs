/*
    Copyright (C) 2023 by Sergey A Kryukov
    https://www.SAKryukov.org
    https://github.com/SAKryukov
    https://www.codeproject.com/Members/SAKryukov
*/

namespace SA.Plugin.View {
    using System.Windows.Media;
    using System.Windows.Media.Animation;
    using Canvas = System.Windows.Controls.Canvas;
    using TimeSpan = System.TimeSpan;
    using Duration = System.Windows.Duration;
    using Path = System.Windows.Shapes.Path;

    public partial class SVG : System.Windows.Controls.UserControl {

        public SVG() {
            InitializeComponent();
        } //SVG

        void Animate(Canvas svg, Path star) {
            DoubleAnimation da = new() {
                From = Main.DefinitionSet.DoubleAnimation.from,
                To = Main.DefinitionSet.DoubleAnimation.to,
                Duration = new Duration(TimeSpan.FromSeconds(Main.DefinitionSet.DoubleAnimation.durationSec)),
                RepeatBehavior = RepeatBehavior.Forever
            };
            RotateTransform rt = new() {
                CenterX = svg.Width / 2,
                CenterY = svg.Height / 2
            };
            star.RenderTransform = rt;
            rt.BeginAnimation(RotateTransform.AngleProperty, da);
        } //Animate

        internal void Animate() {
            Animate(svg, star);
        } //Animate

    } //class SVG

}
