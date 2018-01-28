using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class ImageEvolutionChamber
    {
        #region Class Variables

        private string PathToSourceImage = "";
        public Image SourceImage { get; set; }
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }

        private double MutationRate;
        private int NumChildren;

        private bool SaveImages = false;
        private string SaveImageLocation = "";
        private int SaveFrequency = 0;

        #endregion

        #region Constructor

        public ImageEvolutionChamber()
        {
            SourceImage = null;
        }
        public ImageEvolutionChamber(int imgWidth, int imgHeight, string src)
        {
            LoadSourceImage(src,imgWidth,imgHeight);
        }

        #endregion

        #region Thread Controls

        private Thread t;
        private bool running = false;
        private object locker = new object();

        public void Begin(double mutationRate, int numChildren, string saveImageLocation, int saveFrequency, bool saveImages)
        {
            if (SourceImage == null)
            {
                IEvo.Output("ERR: Cannot begin evolution without a source image!");
                return;
            }
            IEvo.Output("Beginning image evolution simulator:");
            IEvo.Output("Mutation Rate:\t" + mutationRate.ToString());
            IEvo.Output("Number of Children:\t" + numChildren.ToString());
            MutationRate = mutationRate;
            NumChildren = numChildren;

            if (saveImages)
            {
                IEvo.Output("Saving images every "+saveFrequency.ToString()+" generations to:");
                IEvo.Output("'" + saveImageLocation + "'");
            }
            SaveImages = saveImages;
            SaveImageLocation = saveImageLocation;
            SaveFrequency = saveFrequency;

            currentGeneration = 0;
            bestImage = null;

            UpdateStatus("Running");
            t = new Thread(new ThreadStart(run));
            t.Start();
        }

        public void Stop()
        {
            lock (locker)
                running = false;

            UpdateStatus("Stopped");
            if (t != null)
            {
                t.Abort();
            }
        }

        // Thread
        private void run()
        {
            running = true;
            bool keepthread = running;
            while (keepthread)
            {
                NextGeneration();

                lock (locker)
                    keepthread = running;
            }
        }

        #endregion

        #region Evolution Functions

        private GeneratedImage bestImage;
        private int currentGeneration = 0;
        /// <summary>
        /// Every time this is executed a generation is created and the best fit (most like the source image) is chosen to move on to the next generation.
        /// </summary>
        private void NextGeneration()
        {
            try
            {
                DateTime start = DateTime.Now;
                if (currentGeneration == 0)
                    bestImage = CreateInitialGenerationParallel(NumChildren, SourceImage);
                else
                    bestImage = PerformMutationAndFindFittestParallel(bestImage, NumChildren, MutationRate, SourceImage);
                DateTime end = DateTime.Now;
                IEvo.Output("Completed generation " + currentGeneration + " in " + end.Subtract(start).TotalMilliseconds + " ms.");

                if (SaveImages && (currentGeneration % SaveFrequency == 0))
                {
                    bestImage.Source.Save(SaveImageLocation + "\\" + currentGeneration.ToString() + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                }

                currentGeneration++;
                UpdateFittestGeneratedImage(bestImage.Source);
                UpdateGeneration(currentGeneration);
            }
            catch (Exception ex)
            {
                IEvo.ReportException(ex, "NextGeneration");
            }
        }

        private GeneratedImage CreateInitialGeneration(int numSeeds, Image Template)
        {
            GeneratedImage fittestImage = null;
            try
            {
                fittestImage = new GeneratedImage();
                fittestImage.RandomizeSourceImage(ImageWidth, ImageHeight, 1000);
                double fittestDist = GetEuclideanDistance(fittestImage.Source, Template);
                int complete = 0;
                for(int i = 0; i < numSeeds; i++)
                {
                    GeneratedImage t = new GeneratedImage();
                    t.RandomizeSourceImage(ImageWidth, ImageHeight, 1000);
                    Image ti = new Bitmap(Template);
                    double dist = BitmapImageComparison.GetEuclideanDistance(t.Source, ti);
                    if (dist < fittestDist)
                    {
                        fittestDist = dist;
                        fittestImage = t;
                    }
                    complete++;
                    UpdateGenerationProgress(complete, numSeeds);
                }
            }
            catch (Exception ex)
            {
                IEvo.ReportException(ex, "CreateInitialGeneration");
            }

            return fittestImage;
        }

        private GeneratedImage PerformMutationAndFindFittest(GeneratedImage previousFittest, int numChildren, double mutationRate, Image Template)
        {
            GeneratedImage fittestImage = null;
            try
            {
                fittestImage = previousFittest;
                double fittestDist = GetEuclideanDistance(previousFittest.Source, Template);
                for(int i = 0; i < numChildren; i++)
                {
                    GeneratedImage t = new GeneratedImage(previousFittest);
                    t.Mutate(mutationRate);
                    Image ti = new Bitmap(Template);;
                    double dist = BitmapImageComparison.GetEuclideanDistance(t.Source, ti);
                        if (dist < fittestDist)
                        {
                            fittestDist = dist;
                            fittestImage = t;
                        }
                    UpdateGenerationProgress(i, numChildren);
                }
            }
            catch (Exception ex)
            {
                IEvo.ReportException(ex, "PerformMutationAndFindFittest");
            }

            return fittestImage;
        }

        #endregion

        #region Evolution Functions Parallel

        /// <summary>
        /// Responsible for creating the initial generation of images.
        /// </summary>
        /// <param name="numSeeds">Number images to start with.</param>
        /// <param name="Template">The fittest image (to compare the seeds to)</param>
        /// <returns>The fittest of the randomly generated images.</returns>
        private GeneratedImage CreateInitialGenerationParallel(int numSeeds, Image Template)
        {
            GeneratedImage fittestImage = null;

            try
            {
                object locker = new object();
                fittestImage = new GeneratedImage();
                fittestImage.RandomizeSourceImage(ImageWidth, ImageHeight, 1000);
                double fittestDist = GetEuclideanDistance(fittestImage.Source, Template);
                int complete = 0;
                Parallel.For(0, numSeeds, i =>
                {
                    GeneratedImage t = new GeneratedImage();
                    t.RandomizeSourceImage(ImageWidth, ImageHeight, 1000);
                    Image ti;
                    lock (Template)
                    {
                        ti = (Image)new Bitmap(Template);
                    }
                    //double dist = GetEuclideanDistance(t.Source, ti);
                    double dist = BitmapImageComparison.GetEuclideanDistance(t.Source, ti);
                    lock (locker)
                    {
                        if (dist < fittestDist)
                        {
                            fittestDist = dist;
                            fittestImage = t;
                        }
                        complete++;
                    }
                    UpdateGenerationProgress(complete, numSeeds);
                });
            }
            catch (Exception ex)
            {
                IEvo.ReportException(ex, "CreateInitialGeneration");
            }

            return fittestImage;
        }

        /// <summary>
        /// Creates children based off the previously most fit image, mutates them with a given chance, and chooses the most fit to move on to
        /// the next generation.
        /// </summary>
        /// <param name="previousFittest">The previosly most fit image</param>
        /// <param name="numChildren">The number of children to simulate it having</param>
        /// <param name="mutationRate">The chance that a mutation occurs among each child</param>
        /// <param name="Template">The fittest image to compare it to</param>
        /// <returns>The most fit image of this generation</returns>
        private GeneratedImage PerformMutationAndFindFittestParallel(GeneratedImage previousFittest, int numChildren, double mutationRate, Image Template)
        {
            GeneratedImage fittestImage = null;

            try
            {
                object locker = new object();
                fittestImage = previousFittest;
                double fittestDist = GetEuclideanDistance(previousFittest.Source, Template);
                int complete = 0;
                Parallel.For(0, numChildren, i =>
                {
                    GeneratedImage t;
                    lock (previousFittest)
                        t = new GeneratedImage(previousFittest);
                    double tmr;
                    lock (locker)
                        tmr = mutationRate;
                    t.Mutate(tmr);
                    Image ti;
                    lock (Template)
                    {
                        ti = (Image)new Bitmap(Template);
                    }
                    //double dist = GetEuclideanDistance(t.Source, ti);
                    double dist = BitmapImageComparison.GetEuclideanDistance(t.Source, ti);
                    lock (locker)
                    {
                        if (dist < fittestDist)
                        {
                            fittestDist = dist;
                            fittestImage = t;
                        }
                        complete++;
                    }
                    UpdateGenerationProgress(complete, numChildren);
                });
            }
            catch (Exception ex)
            {
                IEvo.ReportException(ex, "PerformMutationAndFindFittest");
            }

            return fittestImage;
        }

        #endregion

        #region Events

        public event ImageUpdateDelegate FittestGeneratedImageUpdate;
        public delegate void ImageUpdateDelegate(Image img);

        public event GenerationUpdateDelegate GenerationUpdate;
        public delegate void GenerationUpdateDelegate(int generation);

        public event GenerationProgressDelegate GenerationProgress;
        public delegate void GenerationProgressDelegate(int current, int total);

        public event StatusUpdateDelegate StatusUpdateEvent;
        public delegate void StatusUpdateDelegate(string status);

        private void UpdateGeneration(int generation)
        {
            if (GenerationUpdate != null)
                GenerationUpdate(generation);
        }

        private void UpdateGenerationProgress(int current, int total)
        {
            if (GenerationProgress != null)
                GenerationProgress(current, total);
        }

        private void UpdateStatus(string status)
        {
            if (StatusUpdateEvent != null)
                StatusUpdateEvent(status);
        }

        private Image CurrentFittestImage;
        private void UpdateFittestGeneratedImage(Image img)
        {
            if (CurrentFittestImage == null)
                CurrentFittestImage = img;
            lock (CurrentFittestImage)
            {
                CurrentFittestImage = (Image)new Bitmap(img);
            }
            if (FittestGeneratedImageUpdate != null)
                FittestGeneratedImageUpdate(CurrentFittestImage);
        }

        #endregion

        #region Image Functions

        private double GetEuclideanDistance(Image img1, Image img2)
        {
            double dist = 0.0;

            try
            {
                Bitmap a = new Bitmap(img1);
                Bitmap b = new Bitmap(img2);

                if (a.Width != b.Width || a.Height != b.Height)
                {
                    System.Windows.Forms.MessageBox.Show("Target comparable image does not have the same dimensions. Cannot get Euclidean Distance.");
                    return 0;
                }

                int nRows = a.Height;
                int nCols = a.Width;
                for (int row = 0; row < nRows; row++)
                {
                    for (int col = 0; col < nCols; col++)
                    {
                        double d = a.GetPixel(col, row).ToArgb() - b.GetPixel(col, row).ToArgb();
                        dist += (d * d);
                    }
                }
            }
            catch (Exception ex)
            {
                IEvo.ReportException(ex, "GetEuclideanDistance");
            }

            return Math.Sqrt(dist);
        }

        private Image ResizeSourceImage(Image Source, int width, int height)
        {
            if (Source == null)
                return null;

            var newImg = new Bitmap(width, height);
            Graphics.FromImage(newImg).DrawImage(Source, 0, 0, width, height);
            return newImg;
        }

        public void LoadSourceImage(string src)
        {
            PathToSourceImage = src;
            try
            {
                SourceImage = Image.FromFile(src);
                ImageWidth = SourceImage.Width;
                ImageHeight = SourceImage.Height;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Could not open source image @ '" + src + "'\n" + ex.Message);
            }
        }

        public void LoadSourceImage(string src, int imgWidth, int imgHeight)
        {
            PathToSourceImage = src;
            try
            {
                ImageWidth = imgWidth;
                ImageHeight = imgHeight;
                SourceImage = Image.FromFile(src);
                SourceImage = ResizeSourceImage(SourceImage, imgWidth, imgHeight);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Could not open source image @ '" + src + "'\n" + ex.Message);
            }
        }

        #endregion

        #region Sequential Evolution Functions

        /*private void computeNextGeneration()
        {
            GImages.Clear();
            UpdateStatus("Mutating the most fit image");
            Random rnd = new Random();
            GImages.Add(bestImage);
            for (int i = 1; i < NumChildren; i++)
            {
                if (GenerationProgress != null)
                    GenerationProgress(i, NumChildren - 1);

                GeneratedImage cpy = new GeneratedImage(bestImage);
                cpy.Mutate(MutationRate);
                GImages.Add(cpy);
            }

            UpdateStatus("Determining most fit image");
            // See which is fittest.
            DateTime start = DateTime.Now;
            bestImage = GImages[0];
            bestImageScore = GetEuclideanDistance(GImages[0].Source, SourceImage);
            double dist;
            for (int i = 1; i < GImages.Count; i++)
            {
                if (GenerationProgress != null)
                    GenerationProgress(i, GImages.Count - 1);

                dist = GetEuclideanDistance(GImages[i].Source, SourceImage);
                if (dist < bestImageScore)
                {
                    bestImage = GImages[i];
                    bestImageScore = dist;
                }
            }
            DateTime end = DateTime.Now;
            UpdateStatus("Operation took " + end.Subtract(start).TotalMilliseconds.ToString() + " ms.");

            if (SaveFrequency != 0 && SaveImageLocation != "")
            {
                if (currentGeneration % SaveFrequency == 0)
                {
                    bestImage.Source.Save(SaveImageLocation + "\\" + currentGeneration.ToString() + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                }
            }

            currentGeneration++;
            if (GenerationUpdate != null)
                GenerationUpdate(currentGeneration);

            UpdateFittestGeneratedImage(bestImage.Source);
        }*/


        /*        private void createInitialGeneration()
        {
            // Create randomized starting images.
            UpdateStatus("Generating random starting images");
            GImages.Clear();
            for (int i = 0; i < NumChildren; i++)
            {
                GeneratedImage gi = new GeneratedImage();
                gi.RandomizeSourceImage(ImageWidth, ImageHeight, 1000);
                GImages.Add(gi);

                UpdateFittestGeneratedImage(gi.Source);

                if (GenerationProgress != null)
                    GenerationProgress(i, NumChildren - 1);
            }

            UpdateStatus("Determining most fit image");
            // See which is fittest.
            DateTime start = DateTime.Now;
            bestImage = GImages[0];
            bestImageScore = GetEuclideanDistance(GImages[0].Source, SourceImage);
            double dist;
            for (int i = 1; i < GImages.Count; i++)
            {
                if (GenerationProgress != null)
                    GenerationProgress(i, GImages.Count - 1);

                dist = GetEuclideanDistance(GImages[i].Source, SourceImage);
                if (dist < bestImageScore)
                {
                    bestImage = GImages[i];
                    bestImageScore = dist;
                }
            }
            DateTime end = DateTime.Now;
            UpdateStatus("Operation took " + end.Subtract(start).TotalMilliseconds.ToString() + " ms.");

            if (SaveFrequency != 0 && SaveImageLocation != "")
            {
                bestImage.Source.Save(SaveImageLocation + "\\" + currentGeneration.ToString() + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
            }

            currentGeneration++;
            if (GenerationUpdate != null)
                GenerationUpdate(currentGeneration);

            UpdateFittestGeneratedImage(bestImage.Source);
        }*/

        #endregion
    }
}
