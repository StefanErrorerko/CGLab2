using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CGLab2.Images.Extensions
{
    public static class ImageExtension
    {
        public static ImageBMP ToBMP(this Images.Image image)
        {
            var bmp = new ImageBMP(image.Width, image.Height, image.Pixels);
            return bmp;
        }
        public static ImagePPM ToPPM(this Images.Image image) 
        {
            var ppm = new ImagePPM(image.Width, image.Height, image.Pixels);
            return ppm;
        }
    }
}
