using System.Drawing;

namespace ImageWriter;

public class TraceResultConverter
{
    public static Color[,] ConvertToGrayscalePixels(float[,] traceResult)
    {
        var width = traceResult.GetLength(1);
        var height = traceResult.GetLength(0);
        var colors = new Color[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                byte greyscale = (byte)(traceResult[y, x] * 255.0f);
                colors[x, y] = Color.FromArgb(greyscale, greyscale, greyscale);
            }
        }
        return colors;
    }
}