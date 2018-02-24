using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace net.dzale.ImageEvolution.Imaging
{
    /// <summary>
    /// Represents a shaded, drawable, 2D ellipse.
    /// </summary>
    class GeneratedEllipse
    {
        public int X, Y, Width, Height;
        public Color Color { get; private set; }

        private Brush coloredBrush;

        public GeneratedEllipse(int x, int y, int width, int height, Color color)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
            this.SetColor(color);
        }

        public GeneratedEllipse(GeneratedEllipse copy)
        {
            this.X = copy.X;
            this.Y = copy.Y;
            this.Width = copy.Width;
            this.Height = copy.Height;
            this.SetColor(copy.Color);
        }

        public void Draw(Graphics g)
        {
            g.FillEllipse(coloredBrush, X, Y, Width, Height);
        }

        public void SetColor(Color c)
        {
            this.Color = c;
            this.coloredBrush = new SolidBrush(this.Color);
        }

    }
}
