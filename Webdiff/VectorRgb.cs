using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webdiff
{
    class VectorRgb
    {
        public double R;
        public double G;
        public double B;

        public VectorRgb(double r, double g, double b)
        {
            this.B = b;
            this.G = g;
            this.R = r;
        }

        public VectorRgb(Color pixel)
        {
            R = pixel.R;
            G = pixel.G;
            B = pixel.B;
        }
        
        public VectorRgb()
        {
            Random rnd = new Random();
            R = rnd.Next();
            G = rnd.Next();
            B = rnd.Next();
        }

        public static VectorRgb operator -(VectorRgb first, VectorRgb second) => new VectorRgb(first.R - second.R, first.G - second.G, first.B - second.B);
        public static VectorRgb operator +(VectorRgb first, VectorRgb second) => new VectorRgb(first.R + second.R, first.G + second.G, first.B + second.B);
        public static VectorRgb operator *(VectorRgb first, VectorRgb second) => new VectorRgb(first.R*second.R, first.G*second.G, first.B*second.B);
    }
}
