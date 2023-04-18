using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGLab2.Images.Extensions
{
    public static class ImageExtension
    {
        public static Image ToBMP(this ImageGIF image)
        {
            return new ImageBMP();
        }
    }
}
