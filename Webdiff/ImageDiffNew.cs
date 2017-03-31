using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webdiff
{
    class ImageDiffNew
    {
        public static Bitmap CompareImage(Bitmap img1, Bitmap img2)
        {
            List<List<VectorRgb>> matrix1 = new List<List<VectorRgb>>();
            List<List<VectorRgb>> matrix2 = new List<List<VectorRgb>>();
            for (int i = 0; i < img1.Height; i++)
            {
                matrix1.Add(new List<VectorRgb>());
                for (int j = 0; j < img1.Width; j++)
                {
                    matrix1[i].Add(new VectorRgb(img1.GetPixel(j,i)));
                }
            }
            for (int i = 0; i < img2.Height; i++)
            {
                matrix2.Add(new List<VectorRgb>());
                for (int j = 0; j < img2.Width; j++)
                {
                    matrix2[i].Add(new VectorRgb(img2.GetPixel(j, i)));
                }
            }
            return img1;
        }
    }
}
