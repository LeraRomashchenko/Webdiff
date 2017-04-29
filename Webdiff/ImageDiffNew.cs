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
//            Debug.PrintMatrix(matrix1, @"C:\Users\romashchenko\Dropbox\univer_diplom\debug\first");
//            Debug.PrintMatrix(matrix2, @"C:\Users\romashchenko\Dropbox\univer_diplom\debug\second");
            Debug.PrintShiftX(shiftMatrix, @"C:\Users\romashchenko\Dropbox\univer_diplom\debug\shift0");
            Bitmap diff;
            Debug.DrawShiftedImage(img1, matrix2, shiftMatrix, out diff);
            var isOdd = 0;
            var iterationCount = 300;
            //var funcVal = ProbabilityFunction.ProbabilityFinaly(height, width, matrix1, matrix2, shiftMatrix);
            //var funcValNext = 0.0;
            while (iterationCount >= 0)
            {
                iterationCount--;
                isOdd = (isOdd + 1)%2;
                for (int i = 1; i < height - 1; i++)
                {
                    for (int j = 1; j < width - 1; j++)
                    {
                        if ((i + j)%2 == isOdd)
                        {
                            var min = Double.MaxValue;
                            var bestShift = shiftMatrix[i][j];
                            // List<double> probability = new List<double>();
                            foreach (var shift in GetAreaShifts(shiftMatrix[i][j]))
                            {
                                if (shift.ShiftX + i < height && shift.ShiftX + i >= 0
                                    && shift.ShiftY + j < width && shift.ShiftY + j >= 0)
                                {
                                    var p = ProbabilityFunction.ProbabilityOfShift(shiftMatrix[i - 1][j],
                                        shiftMatrix[i][j + 1],
                                        shiftMatrix[i + 1][j], shiftMatrix[i][j - 1], shift, matrix1, matrix2, i, j);
                                    // probability.Add(p);
                                    if (p < min)
                                    {
                                        bestShift = shift;
                                        min = p;
                                    }
                                }
                            }
                            shiftMatrix[i][j] = bestShift;
                        }
                    }
                }
               // ProbabilityFunction.ProbabilityFinaly(height, width, matrix1, matrix2, shiftMatrix);
            }
            Debug.PrintShiftX(shiftMatrix, @"C:\Users\romashchenko\Dropbox\univer_diplom\debug\shift");
            return img1;
        }

        public static List<Shift> GetAreaShifts(Shift center)
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
