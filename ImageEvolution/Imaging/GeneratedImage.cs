using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace net.dzale.ImageEvolution.Imaging
{
    /// <summary>
    /// Represents an image (creature) composed of zero or more GeneratedEllipses (genes) which can be mutated or combined to generate offspring.
    /// </summary>
    class GeneratedImage
    {
        public List<GeneratedEllipse> Ellipses;
        public Image Source { get; set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        // Mutation Constraints
        private const int MIN_ELLIPSE_WIDTH = 15;
        private const int MIN_ELLIPSE_HEIGHT = 15;
        private const int MAX_ELLIPSE_WIDTH = 35;
        private const int MAX_ELLIPSE_HEIGHT = 35;
        private const int MAX_ELLIPSE_SHADE_RGB_VALUE = 255;

        // Image Drawing Configuration
        private const bool USE_ANTIALIASING = false;

        /// <summary>
        /// Creates a new empty GeneratedImage.
        /// </summary>
        public GeneratedImage()
        {
            this.Source = null;
            this.Ellipses = new List<GeneratedEllipse>();
        }
        /// <summary>
        /// Creates a new GeneratedImage and initializes it with randomly generated ellipses.
        /// </summary>
        /// <param name="width">width of image</param>
        /// <param name="height">height of image</param>
        /// <param name="nEllipses">number of ellipses</param>
        public GeneratedImage(int width, int height, int nEllipses)
        {
            RandomizeSourceImage(width, height, nEllipses);
        }
        /// <summary>
        /// Duplicates an existing GeneratedImage by copying it.
        /// </summary>
        /// <param name="gi">existing GeneratedImage</param>
        public GeneratedImage(GeneratedImage gi)
        {
            this.Width = gi.Width;
            this.Height = gi.Height;
            this.Source = (Image)gi.Source.Clone();
            this.Ellipses = new List<GeneratedEllipse>();
            for (int i = 0; i < gi.Ellipses.Count; i++)
            {
                this.Ellipses.Add(new GeneratedEllipse(gi.Ellipses[i]));
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

        /// <summary>
        /// Clears the current content of the GeneratedImage and randomly generates new values based on parameters.
        /// </summary>
        /// <param name="width">image width</param>
        /// <param name="height">image height</param>
        /// <param name="nEllipses">number of random ellipses to generate</param>
        public void RandomizeSourceImage(int width, int height, int nEllipses)
        {
            Width = width;
            Height = height;
            Ellipses = GetRandomEllipses(width, height, nEllipses);
            RedrawImage();
        }

        /// <summary>
        /// Clears the backing Image and redraws it using the current Ellipses.
        /// </summary>
        public void RedrawImage()
        {
            Source.Dispose();
            Source = (Image)CreateBitmapFromEllipses(Width, Height, Ellipses);
        }

        private Bitmap CreateBitmapFromEllipses(int bitmapWidth, int bitmapHeight, List<GeneratedEllipse> ellipses)
        {
            var bm = new Bitmap(bitmapWidth, bitmapHeight);
            using (Graphics g = Graphics.FromImage(bm))
            {
                if (USE_ANTIALIASING)
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                foreach (GeneratedEllipse ge in ellipses)
                    ge.Draw(g);
            }
            return bm;
        }

        private List<GeneratedEllipse> GetRandomEllipses(int maxX, int maxY, int nEllipses)
        {
            var ellipses = new List<GeneratedEllipse>();
            var rnd = new Random();

            for (int i = 0; i < nEllipses; i++)
            {
                int w = rnd.Next(MIN_ELLIPSE_WIDTH, MAX_ELLIPSE_WIDTH);
                int h = rnd.Next(MIN_ELLIPSE_HEIGHT, MAX_ELLIPSE_HEIGHT);
                int x = rnd.Next(maxX - w);
                int y = rnd.Next(maxY - h);
                ellipses.Add(new GeneratedEllipse(x, y, w, h, GetRandomGreyscaleShadeOfColor(rnd)));
            }

            return ellipses;
        }

        private Color GetRandomGreyscaleShadeOfColor(Random rnd)
        {
            int shade = rnd.Next(MAX_ELLIPSE_SHADE_RGB_VALUE);
            return Color.FromArgb(shade, shade, shade);
        }

        /// <summary>
        /// Mutates the collection of ellipses that make up this GeneratedImage and redraws the image.
        /// </summary>
        /// <param name="mutationRate">A double representing the percent distribution of ellipses to mutate</param>
        public void Mutate(double mutationRate)
        {
            Ellipses = MutateEllipses(Ellipses, mutationRate);
            RedrawImage();
        }

        private List<GeneratedEllipse> MutateEllipses(List<GeneratedEllipse> ellipses, double mutationRate)
        {
            Random rnd = new Random();
            foreach(GeneratedEllipse ellipse in ellipses)
            {
                if (rnd.NextDouble() < mutationRate)
                {
                    ellipse.Width = rnd.Next(MIN_ELLIPSE_WIDTH, MAX_ELLIPSE_WIDTH);
                    ellipse.Height = rnd.Next(MIN_ELLIPSE_WIDTH, MAX_ELLIPSE_WIDTH);
                    ellipse.SetColor(GetRandomGreyscaleShadeOfColor(rnd));
                }
            }

            return ellipses;
        }

        /// <summary>
        /// Combines the genes (ellipses) of two parents and generates an offspring image.
        /// </summary>
        /// <param name="mom">momma image</param>
        /// <param name="dad">poppa image</param>
        /// <returns>a new image with combined genes</returns>
        public static GeneratedImage CreateGeneratedImageOffspringFromParents(GeneratedImage mom, GeneratedImage dad)
        {
            GeneratedImage offspring = new GeneratedImage(mom);
            offspring.Ellipses = CombineParentsGenes(mom.Ellipses, dad.Ellipses);
            offspring.RedrawImage();
            return offspring;
        }

        private static List<GeneratedEllipse> CombineParentsGenes(List<GeneratedEllipse> moms, List<GeneratedEllipse> dads)
        {
            var offspringGenes = new List<GeneratedEllipse>();

            for (int i = 0; i < moms.Count; i++)
            {
                if (i % 2 == 0)
                    offspringGenes.Add(moms[i]);
                else
                    offspringGenes.Add(dads[i]);
            }

            return offspringGenes;
        }
    }
}
