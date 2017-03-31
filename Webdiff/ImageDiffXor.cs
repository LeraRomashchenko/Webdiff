using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webdiff
{
    static class ImageDiffXor
    {
        public static int CompareImages(Bitmap bmp1, Bitmap bmp2, out Bitmap diff)
        {
            if (bmp1.Width != bmp2.Width)
                throw new Exception("Image width");
            int unmatched = 0;
            var stopwatch = Stopwatch.StartNew();
            diff = new Bitmap(bmp1.Width * bmp1.Height > bmp2.Width * bmp2.Height ? bmp1 : bmp2, Math.Max(bmp1.Width, bmp2.Width), Math.Max(bmp1.Height, bmp2.Height));
            var matrix = new bool[Math.Max(bmp1.Width, bmp2.Width), Math.Max(bmp1.Height, bmp2.Height)];
            int minX = int.MaxValue, minY = int.MaxValue, maxX = int.MinValue, maxY = int.MinValue;
            for (int y = 0; y < Math.Min(bmp1.Height, bmp2.Height); y++)
            {
                for (int x = 0; x < bmp1.Width; x++)
                {
                    var color1 = bmp1.GetPixel(x, y);
                    var color2 = bmp2.GetPixel(x, y);
                    var c1 = color1.ToArgb();
                    var c2 = color2.ToArgb();
                    var xor = c1 ^ c2;
                    if (xor != 0)
                    {
                        diff.SetPixel(x, y, Color.FromArgb(xor | unchecked((int)0xff00007f)));
                        minX = Math.Min(minX, x);
                        minY = Math.Min(minY, y);
                        maxX = Math.Max(maxX, x);
                        maxY = Math.Max(minY, y);
                        matrix[x, y] = true;
                        unmatched++;
                    }
                }
            }
            Console.WriteLine(stopwatch.Elapsed);
            for (int y = Math.Min(bmp1.Height, bmp2.Height); y < Math.Max(bmp1.Height, bmp2.Height); y++)
            {
                for (int x = 0; x < bmp1.Width; x++)
                {
                    diff.SetPixel(x, y, Color.Black);
                    unmatched++;
                }
            }
            if (unmatched > 0)
            {
                var g = Graphics.FromImage(diff);
                var pen = new Pen(Color.Red, 2.0f);
                foreach (var rect in new Rectangle(minX, minY, maxX - minX, maxY - minY).Split(matrix))
                {
                    g.DrawRectangle(pen, new Rectangle(Math.Max(0, rect.X - 4), Math.Max(0, rect.Y - 4), Math.Min(diff.Width - rect.X, rect.Width + 8), Math.Min(diff.Height - rect.Y, rect.Height + 8)));
                }
            }
            return unmatched;
        }

        private static IEnumerable<Rectangle> Split(this Rectangle rect, bool[,] diff)
        {
            var split = rect.SplitByX(diff).SelectMany(r => r.SplitByY(diff)).ToArray();
            return split.Length == 1 ? split : split.AsParallel().SelectMany(r => r.Split(diff)).ToArray();
        }

        private static IEnumerable<Rectangle> SplitByX(this Rectangle rect, bool[,] diff)
        {
            var gap = 0;
            for (int x = rect.Left; x <= rect.Right; x++)
            {
                if (rect.ScanCol(x, diff))
                    gap = 0;
                else if (++gap >= 5)
                {
                    yield return new Rectangle(rect.X, rect.Y, x - rect.X, rect.Height).Contract(diff);
                    yield return new Rectangle(x, rect.Y, rect.Width - (x - rect.X), rect.Height).Contract(diff);
                    yield break;
                }
            }
            yield return rect;
        }

        private static IEnumerable<Rectangle> SplitByY(this Rectangle rect, bool[,] diff)
        {
            var gap = 0;
            for (int y = rect.Top; y <= rect.Bottom; y++)
            {
                if (rect.ScanRow(y, diff))
                    gap = 0;
                else if (++gap >= 5)
                {
                    yield return new Rectangle(rect.X, rect.Y, rect.Width, y - rect.Y).Contract(diff);
                    yield return new Rectangle(rect.X, y, rect.Width, rect.Height - (y - rect.Y)).Contract(diff);
                    yield break;
                }
            }
            yield return rect;
        }

        private static Rectangle Contract(this Rectangle rect, bool[,] diff)
        {
            for (int y = rect.Top; y <= rect.Bottom; y++)
            {
                if (rect.ScanRow(y, diff))
                {
                    if (y != rect.Top)
                        rect = new Rectangle(rect.X, y, rect.Width, rect.Height - (y - rect.Y));
                    break;
                }
            }
            for (int y = rect.Bottom; y >= rect.Top; y--)
            {
                if (rect.ScanRow(y, diff))
                {
                    if (y != rect.Bottom)
                        rect = new Rectangle(rect.X, rect.Y, rect.Width, y - rect.Y);
                    break;
                }
            }
            for (int x = rect.Left; x <= rect.Right; x++)
            {
                if (rect.ScanCol(x, diff))
                {
                    if (x != rect.X)
                        rect = new Rectangle(x, rect.Y, rect.Width - (x - rect.X), rect.Height);
                    break;
                }
            }
            for (int x = rect.Right; x >= rect.Left; x--)
            {
                if (rect.ScanCol(x, diff))
                {
                    if (x != rect.Right)
                        rect = new Rectangle(rect.X, rect.Y, x - rect.X, rect.Height);
                    break;
                }
            }
            return rect;
        }

        private static bool ScanRow(this Rectangle rect, int y, bool[,] diff)
        {
            for (int x = rect.Left; x <= rect.Right; x++)
            {
                if (diff[x, y])
                    return true;
            }
            return false;
        }

        private static bool ScanCol(this Rectangle rect, int x, bool[,] diff)
        {
            for (int y = rect.Top; y <= rect.Bottom; y++)
            {
                if (diff[x, y])
                    return true;
            }
            return false;
        }

    }

}
