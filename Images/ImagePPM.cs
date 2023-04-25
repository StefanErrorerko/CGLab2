using System.Drawing;

namespace CGLab2.Images
{
    public class ImagePPM : Image
    {
        public String Header { get; set; } = "P3";
        public int MaxValue { get; set; } = 255;
        public ImagePPM(String header, int width, int height, int maxValue, Color[,] pixels) 
        {
            Header = header;
            Pixels = pixels;
            Width = width;
            Height = height;
            MaxValue = maxValue;
        }
        public ImagePPM(int width, int height, Color[,] pixels)
        {
            Pixels = pixels;
            Width = width;
            Height = height;
        }
    }
}
