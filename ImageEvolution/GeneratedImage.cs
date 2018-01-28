using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace WindowsFormsApplication1
{
    class GeneratedImage
    {
        public List<GeneratedEllipse> GE;
        public Image Source { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        private const int MIN_WIDTH = 15;
        private const int MIN_HEIGHT = 15;
        private const int MAX_WIDTH = 35;
        private const int MAX_HEIGHT = 35;

        private const bool USE_ANTIALIASING = false;

        public GeneratedImage()
        {
            Source = null;
            GE = new List<GeneratedEllipse>();
        }
        public GeneratedImage(GeneratedImage gi)
        {
            this.Source = (Image)gi.Source.Clone();
            this.GE = new List<GeneratedEllipse>();
            for (int i = 0; i < gi.GE.Count; i++)
            {
                this.GE.Add(new GeneratedEllipse(gi.GE[i]));
            }
        }
        ~GeneratedImage()
        {
            if (Source != null)
            {
                Source.Dispose();
                Source = null;
            }
        }

        public void RandomizeSourceImage(int width, int height, int nCircles)
        {
            GE.Clear();
            Random rnd = new Random();
            Bitmap bm = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bm))
            {
                if(USE_ANTIALIASING)
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                for (int i = 0; i < nCircles; i++)
                {
                    int w = rnd.Next(MIN_WIDTH, MAX_WIDTH);
                    int h = rnd.Next(MIN_HEIGHT, MAX_HEIGHT);
                    int x = rnd.Next(width - w);
                    int y = rnd.Next(height - h);
                    int shade = rnd.Next(256);
                    Color c = Color.FromArgb(shade, shade, shade);
                    GeneratedEllipse ge = new GeneratedEllipse(x, y, w, h, c);
                    ge.draw(g);
                    GE.Add(ge);
                }
            }

            Source = (Image)bm;
        }

        public void Mutate(double MutationRate)
        {
            Bitmap bm = new Bitmap(Source.Width, Source.Height);
            Random rnd = new Random();
            using (Graphics g = Graphics.FromImage(bm))
            {
                if(USE_ANTIALIASING)
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                for (int i = 0; i < GE.Count; i++)
                {
                    if (rnd.NextDouble() < MutationRate)
                    {
                        GeneratedEllipse ge = GE[i];
                        ge.Width = rnd.Next(MIN_WIDTH, MAX_WIDTH);
                        ge.Height = rnd.Next(MIN_WIDTH, MAX_WIDTH);
                        int shade = rnd.Next(256);
                        Color c = Color.FromArgb(shade, shade, shade);
                        ge.B = new SolidBrush(c);
                        ge.draw(g);
                    }
                    else
                    {
                        GE[i].draw(g);
                    }
                }
            }

            Source = (Image)bm;
        }

        public static GeneratedImage CreateOffspring(GeneratedImage m, GeneratedImage d)
        {
            GeneratedImage b = new GeneratedImage();
            b.Width = m.Width;
            b.Height = m.Height;

            // Mom and Dad each give half their ellipses.
            Bitmap bm = new Bitmap(b.Width, b.Height);
            using(Graphics g = Graphics.FromImage(bm))
            {
                for (int i = 0; i < m.GE.Count; i++)
                {
                    if (i % 2 == 0)
                        b.GE.Add(m.GE[i]);
                    else
                        b.GE.Add(d.GE[i]);

                    b.GE[b.GE.Count - 1].draw(g);
                }
            }
            b.Source = bm;

            return b;
        }
    }
}
