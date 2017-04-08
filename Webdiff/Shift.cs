using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webdiff
{
    class Shift
    {
        public int ShiftX;
        public int ShiftY;
        
        public Shift(int minX, int minY, int maxX, int maxY)
        {
            Random rnd = new Random();
            ShiftX = rnd.Next(minX, maxX);
            ShiftY = rnd.Next(minY, maxY);
        }
    }
}
