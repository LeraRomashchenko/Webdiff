using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webdiff
{
    class TestNewDiff
    {
        private Bitmap CompareByUrls(string url1, string url2)
        {
            return ImageDiffNew.CompareImage(new Bitmap(Image.FromFile(url1)), new Bitmap(Image.FromFile(url2)));
        }

        public void TestTemp()
        {
            var url1 = @"C:\Users\romashchenko\Desktop\diplom\1_1.png";
            var url2 = @"C:\Users\romashchenko\Desktop\diplom\2_1.png";

            CompareByUrls(url1, url2).Save(@"C:\Users\romashchenko\Desktop\diplom\out.png");
        }
        public void TestTemp(string outUrl)
        {
            var url1 = @"";
            var url2 = @"";

            CompareByUrls(url1, url2).Save(outUrl);
        }
    }
}
