using System.Drawing;

namespace ImageWriter;

public class TraceResultConverter
{
    public static Color[,] ConvertToGrayscalePixels(float[,] traceResult)
    {
        var width = traceResult.GetLength(0);
        var height = traceResult.GetLength(1);
        var pixels = new Color[width, height];

        var maxIntensity = 1.0f;

        for (var i = 0; i < width; i++)
        for (var j = 0; j < height; j++)
        {
            var intensity = traceResult[i, j] / maxIntensity;
            var grayscaleValue = (byte)(intensity * 255);
            var color = Color.FromArgb(grayscaleValue, grayscaleValue, grayscaleValue);
            pixels[i, j] = color;
        }

        return pixels;
    }
}