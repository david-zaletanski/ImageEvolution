using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WindowsFormsApplication1
{
    class GeneratedEllipse
    {
        public int X, Y, Width, Height;
        public Brush B;

        public GeneratedEllipse(int X, int Y, int Width, int Height, Color Color)
        {
            this.X = X;
            this.Y = Y;
            this.Width = Width;
            this.Height = Height;
            B = new SolidBrush(Color);
        }

        public GeneratedEllipse(GeneratedEllipse copy)
        {
            this.X = copy.X;
            this.Y = copy.Y;
            this.Width = copy.Width;
            this.Height = copy.Height;
            this.B = (Brush)copy.B.Clone();
        }

        public void draw(Graphics g)
        {
            g.FillEllipse(B, X, Y, Width, Height);
        }
    }
}
