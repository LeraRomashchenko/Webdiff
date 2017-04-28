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
        public static Bitmap CompareImage(Bitmap img1, Bitmap img2) //todo Пока реализация для одинаковой размерности изображений
        {
            var height = img1.Height;
            var width = img1.Width;
            List<List<VectorRgb>> matrix1 = new List<List<VectorRgb>>();
            List<List<VectorRgb>> matrix2 = new List<List<VectorRgb>>();
            List<List<Shift>> shiftMatrix = new List<List<Shift>>();
            Random rnd = new Random();
            for (int i = 0; i < height; i++)
            {
                matrix1.Add(new List<VectorRgb>());
                shiftMatrix.Add(new List<Shift>());
                for (int j = 0; j < width; j++)
                {
                    matrix1[i].Add(new VectorRgb(img1.GetPixel(j,i)));
                    shiftMatrix[i].Add(new Shift(-j + 1, -i + 1, width - j - 1, height - i - 1, rnd));
                }
            }
            for (int i = 0; i < height; i++)
            {
                matrix2.Add(new List<VectorRgb>());
                for (int j = 0; j < width; j++)
                {
                    matrix2[i].Add(new VectorRgb(img2.GetPixel(j, i)));
                }
            }
            Debug.PrintMatrix(matrix1, @"C:\Users\romashchenko\Dropbox\univer_diplom\debug\first");
            Debug.PrintMatrix(matrix2, @"C:\Users\romashchenko\Dropbox\univer_diplom\debug\second");
            Debug.PrintShiftX(shiftMatrix, @"C:\Users\romashchenko\Dropbox\univer_diplom\debug\shift");
            Bitmap diff;
            Debug.DrawShiftedImage(img1, matrix2, shiftMatrix, out diff);
            var testProb = ProbabilityFunction.ProbabilityFinaly(height, width, matrix1, matrix2, shiftMatrix);
            var isOdd = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if ((i + j)%2 == isOdd)
                    {
                        List<double> probability = new List<double>();
                        foreach (var shift in GetNearShifts(shiftMatrix[i][j]))
                        {
                            // TODO искать соседей и вызывать ProbabilityOfShift, выбирать максимум и так пока не сойдется
                        }
                    }
               }
            }
             return img1;
        }

        public static List<Shift> GetNearShifts(Shift center)
        {
            var radius = 5;
            List<Shift> area = new List<Shift>();
            for (int i = center.ShiftX; i <= center.ShiftX + radius; i++)
            {
                for (int j = center.ShiftY; j <= center.ShiftY + radius; j++)
                {
                    area.Add(new Shift(i,j));
                }
            }
            return area;
        }
    }
}
