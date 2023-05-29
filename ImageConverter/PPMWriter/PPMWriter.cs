using System.Drawing;
using System.Text;
using ImageConverter.Protocols.Interfaces;

namespace PPMWriter;

public class PpmWriter : IImageWriter
{
    public string FileExtention => "ppm";

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

    public byte[] Write(Color[,] pixels)
    {
        var sb = new StringBuilder();
        sb.AppendLine(Constants.Header);
        //var header = Encoding.ASCII.GetBytes(Constants.Header + "\n");
        sb.AppendLine($"{pixels.GetLength(0)} {pixels.GetLength(1)}");
        //var dimensions = Encoding.ASCII.GetBytes($"{pixels.GetLength(0)} {pixels.GetLength(1)}\n");
        sb.AppendLine(Constants.MaxValue);
        //var maxValue = Encoding.ASCII.GetBytes($"{Constants.MaxValue}\n");
        //var data = new byte[pixels.GetLength(0) * pixels.GetLength(1) * 6];
        //int dataIndex = 0;
        for (var j = pixels.GetLength(1) - 1; j >= 0; j--)
        {
            for (var i = 0; i < pixels.GetLength(0); i++)
            {
                sb.Append($"{pixels[i, j].B} ");
                sb.Append($"{pixels[i, j].G} ");
                sb.Append($"{pixels[i, j].R} ");
                //data[dataIndex++] = pixels[i, j].B;
                //data[dataIndex++] = (byte)' ';
                //data[dataIndex++] = pixels[i, j].G;
                //data[dataIndex++] = (byte)' ';
                //data[dataIndex++] = pixels[i, j].R;
                //data[dataIndex++] = (byte)' ';
            }
            sb.AppendLine();
        }

        //var bytes = new byte[header.Length + dimensions.Length + maxValue.Length + data.Length];
        //Buffer.BlockCopy(header, 0, bytes, 0, header.Length);
        //Buffer.BlockCopy(dimensions, 0, bytes, header.Length, dimensions.Length);
        //Buffer.BlockCopy(maxValue, 0, bytes, header.Length + dimensions.Length, maxValue.Length);
        //Buffer.BlockCopy(data, 0, bytes, header.Length + dimensions.Length + maxValue.Length, data.Length);
        var str = sb.ToString().Replace("\r", "");
        return Encoding.ASCII.GetBytes(str);
    }

    private struct Constants
    {
        public const string Header = "P3";
        public const string MaxValue = "255";
    }
}