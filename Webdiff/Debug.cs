using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webdiff
{
    class Debug
    {
        public static void PrintMatrix(List<List<VectorRgb>> matrix, string filename)
        {
            File.WriteAllText(filename, "");
            for (int i = 0; i < matrix.Count; i++)
            {
                string s = "";
                for (int j = 0; j < matrix[i].Count; j++)
                {
                    s += matrix[i][j].R + " ";
                }
                File.AppendAllLines(filename, new []{s + "\n"});
            }
        }
        public static void PrintShiftX(List<List<Shift>> matrix, string filename)
        {
            File.WriteAllText(filename, "");
            for (int i = 0; i < matrix.Count; i++)
            {
                string s = "";
                for (int j = 0; j < matrix[i].Count; j++)
                {
                    s += matrix[i][j].ShiftX + " ";
                }
                File.AppendAllLines(filename, new[] { s + "\n" });
            }
        }

        public static void DrawShiftedImage(Bitmap bmp, List<List<VectorRgb>> second, List<List<Shift>> shift, out Bitmap diff)
        {
            diff = new Bitmap(bmp, second[0].Count, second.Count);
            for (int i = 0; i < second.Count; i++)
            {
                for (int j = 0; j < second[i].Count; j++)
                {

                    diff.SetPixel(j,i,Color.FromArgb((int)second[i + shift[i][j].ShiftY][j + shift[i][j].ShiftX].R,
                        (int) second[i + shift[i][j].ShiftY][j + shift[i][j].ShiftX].G,
                        (int) second[i + shift[i][j].ShiftY][j + shift[i][j].ShiftX].B));
                }
            }
            diff.Save(@"C:\Users\romashchenko\Desktop\diplom\out_3.png");
        }
    }
}
