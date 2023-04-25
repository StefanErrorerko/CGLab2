using System.Drawing;
using ImageConverter.Protocols.Interfaces;

namespace PPMWriter;

public class PpmWriter : IImageWriter
{
    public void Write(string filePath, Color[,] pixels)
    {
        var writer = new StreamWriter(filePath);
        writer.WriteLine(Constants.Header);
        writer.WriteLine($"{pixels.GetLength(1)} {pixels.GetLength(0)} {Constants.MaxValue}\n");
        for (var j = pixels.GetLength(0) - 1; j >= 0; j--)
        {
            for (var i = 0; i < pixels.GetLength(1); i++)
            {
                writer.Write(pixels[i, j].R);
                writer.Write(pixels[i, j].G);
                writer.Write(pixels[i, j].B);
            }

            writer.WriteLine();
        }

        writer.Close();
    }

    private struct Constants
    {
        public const string Header = "P3";
        public const string MaxValue = "255";
    }
}