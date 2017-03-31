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
        private int r;
        private int g;
        private int b;

        public VectorRgb(int r, int g, int b)
        {
            this.b = b;
            this.g = g;
            this.r = r;
        }

        public VectorRgb(Color pixel)
        {
            r = pixel.R;
            g = pixel.G;
            b = pixel.B;
        }

        public VectorRgb()
        {
            r = 0;
            g = 0;
            b = 0;
        }
    }
}
