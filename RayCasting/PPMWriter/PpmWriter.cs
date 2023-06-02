using System.Drawing;
using System.Text;
using ImageWriter;

namespace PPMWriter;

public class PpmWriter : IImageWriter
{
    public string FileExtention => "ppm";

    public byte[] Write(Color[,] pixels)
    {
        var sb = new StringBuilder();
        sb.AppendLine(Constants.Header);
        sb.AppendLine($"{pixels.GetLength(0)} {pixels.GetLength(1)}");
        sb.AppendLine(Constants.MaxValue);

        for (var j = pixels.GetLength(1) - 1; j >= 0; j--)
        {
            for (var i = 0; i < pixels.GetLength(0); i++)
            {
                sb.Append($"{pixels[i, j].B} ");
                sb.Append($"{pixels[i, j].G} ");
                sb.Append($"{pixels[i, j].R} ");
            }

            sb.AppendLine();
        }

        var str = sb.ToString().Replace("\r", "");
        return Encoding.ASCII.GetBytes(str);
    }

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