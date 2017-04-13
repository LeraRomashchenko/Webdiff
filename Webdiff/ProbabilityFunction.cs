using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webdiff
{
    class ProbabilityFunction
    {
        public static double ProbabilityByNeighbors(Shift first, Shift second)
        {
            const double eps = 0.1;
            if (first.ShiftX == second.ShiftX && first.ShiftY == second.ShiftY)
                return (eps);
            return (1 - eps);
        }

        public static List<List<VectorRgb>> ProbabilityByShift(int h, int w, List<List<VectorRgb>> first, List<List<VectorRgb>> second, List<List<Shift>> shift)
        {
            List<List<VectorRgb>> probMatrix = new List<List<VectorRgb>>();
            for (int i = 0; i < h; i++)
            {
                probMatrix.Add(new List<VectorRgb>());
                for (int j = 0; j < w; j++)
                {
                    var newX = i + shift[i][j].ShiftY;
                    var newY = j + shift[i][j].ShiftX;
                    var f = first[i][j];
                    var s = second[newX][newY];
                    
                    double diffR = Math.Pow(f.R - s.R, 2)/2;
                    double diffG = Math.Pow(f.G - s.G, 2)/2;
                    double diffB = Math.Pow(f.B - s.B, 2)/2;
                    if (Math.Abs(diffR) == 0)
                    {
                        diffR = 0.01;
                    }
                    if (Math.Abs(diffG) == 0)
                    {
                        diffG = 0.01;
                    }
                    if (Math.Abs(diffB) == 0)
                    {
                        diffB = 0.01;
                    }
                    probMatrix[i].Add(new VectorRgb(diffR, diffG, diffB));
                }
            }
            return probMatrix;
        }

        public static VectorRgb ProbabilityFinaly(int h, int w, List<List<VectorRgb>> first, List<List<VectorRgb>> second, List<List<Shift>> shift)
        {
            List<List<VectorRgb>> probMatrix = ProbabilityByShift(h, w, first, second, shift);
            Debug.PrintMatrix(probMatrix, @"C:\Users\romashchenko\Dropbox\univer_diplom\debug\probMatrix");
            VectorRgb mult = new VectorRgb(0, 0, 0);
            for (int i = 1; i < h - 1; i++)
            {
                for (int j = 1; j < w - 1; j++)
                {
                   var temp = probMatrix[i][j];
                    foreach (var neighbor in new[] { shift[i - 1][j], shift[i + 1][j], shift[i][j - 1], shift[i][j + 1] })
                    {
                        temp *= new VectorRgb(ProbabilityByNeighbors(shift[i][j], neighbor), ProbabilityByNeighbors(shift[i][j], neighbor), ProbabilityByNeighbors(shift[i][j], neighbor));
                    }
                    mult += temp;
                }
            }
            return mult;
        }
    }
}
