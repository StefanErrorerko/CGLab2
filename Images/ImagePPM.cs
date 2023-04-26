using System.Drawing;

namespace CGLab2.Images;

public class ImagePPM : Image
{
    public ImagePPM(string header, int width, int height, int maxValue, Color[,] pixels)
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

    public string Header { get; set; } = "P3";
    public int MaxValue { get; set; } = 255;
}