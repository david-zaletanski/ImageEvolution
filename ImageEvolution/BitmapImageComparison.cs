using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace WindowsFormsApplication1
{
    class BitmapImageComparison
    {
        public static double GetEuclideanDistance(Image a, Image b)
        {
            if (a.Width != b.Width || a.Height != b.Height)
            {
                System.Windows.Forms.MessageBox.Show("Images a and b are not the same dimensions. Cannot calculate Euclidean distance.");
                return 0;
            }

            double dist = 0;

            Bitmap bitA = new Bitmap(a);
            BitmapData bdA = bitA.LockBits(new Rectangle(0, 0, bitA.Width, bitA.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            Bitmap bitB = new Bitmap(b);
            BitmapData bdB = bitB.LockBits(new Rectangle(0, 0, bitB.Width, bitB.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

            int bitsPerPixel = 24; // As per the PixelFormat bpp
            int size = bdA.Stride * bdA.Height; // Size of the image in bytes

            byte[] aArr = new byte[size];
            System.Runtime.InteropServices.Marshal.Copy(bdA.Scan0, aArr, 0, size); // Copies data from BitmapData into byte array.
            byte[] bArr = new byte[size];
            System.Runtime.InteropServices.Marshal.Copy(bdB.Scan0, bArr, 0, size);

            for (int i = 0; i < size; i += bitsPerPixel / 8)
            {
                // This is every pixel where Arr[i] is the first 3 bytes of color
                int aR = aArr[i];
                int aG = aArr[i + 1];
                int aB = aArr[i + 2];
                int bR = bArr[i];
                int bG = bArr[i + 1];
                int bB = bArr[i + 2];
                //double d = (0.3*aR + 0.59*aG + 0.11*aB) - (0.3*bR + 0.59*bG + 0.11*bB);
                // Converts to greyscale and then gets difference.
                double d = ((0.3 * aR + 0.59 * aG + 0.11 * aB) - (0.3 * bR + 0.59 * bG + 0.11 * bB)) * 100.0 / 256.0;
                dist += (d * d);
            }

            bitA.UnlockBits(bdA);
            bitB.UnlockBits(bdB);

            aArr = null;
            bArr = null;

            return Math.Sqrt(dist);
        }

    }
}
