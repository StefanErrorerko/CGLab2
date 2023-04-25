using System.Drawing;
using ImageConverter.Protocols.Interfaces;

namespace PPMReader;

public class PpmReader : IImageReader
{
    private readonly string _filePath;

    public PpmReader(string path)
    {
        _filePath = path;
    }

    //public ImagePPM ReadP6()
    //{
    //    using (FileStream fs = new FileStream(filePath, FileMode.Open))
    //    {
    //        using (BinaryReader reader = new BinaryReader(fs))
    //        {
    //            // Read the PPM header
    //            string header = new string(reader.ReadChars(2));
    //            if (header != "P6")
    //            {
    //                Console.WriteLine("Error: Invalid PPM file format.");
    //                return null;
    //            }

    //            // Read the image dimensions
    //            string dimensions = "";
    //            char c = ' ';
    //            c = (char)reader.ReadByte();
    //            do
    //            {
    //                c = (char)reader.ReadByte();
    //                dimensions += c;
    //            } while (c != '\n');
    //            dimensions = dimensions.Trim();
    //            string[] dimensionValues = dimensions.Split(' ');
    //            int width = int.Parse(dimensionValues[0]);
    //            int height = int.Parse(dimensionValues[1]);

    //            // Read the maximum color value
    //            string maxColorValue = "";
    //            c = ' ';
    //            while (c != '\n')
    //            {
    //                c = (char)reader.ReadByte();
    //                maxColorValue += c;
    //            }
    //            maxColorValue = maxColorValue.Trim();
    //            int maxVal = int.Parse(maxColorValue);

    //            // Read the pixel data
    //            byte[] pixelData = reader.ReadBytes(width * height * 3);
    //            Color[,] pixels = new Color[width*3, height];

    //            // Print the pixel data
    //            for (int i = 0; i < height; i++)
    //            {
    //                for (int j = 0; j < width * 3; j += 3)
    //                {
    //                    int r = pixelData[i * width * 3 + j];
    //                    int g = pixelData[i * width * 3 + j + 1];
    //                    int b = pixelData[i * width * 3 + j + 2];
    //                    pixels[i, j] = Color.FromArgb(r, g, b);
    //                }

    //            }
    //            return new ImagePPM(header, width, height, maxVal, pixels);
    //        }
    //    }
    //}

    public Color[,] Read()
    {
        int width = 0,
            height = 0,
            maxColor = 0;
        var pixels = new Color[width, height];

        // read the PPM file header
        using (var reader = new StreamReader(_filePath))
        {
            if (reader.ReadLine() != "P3") throw new Exception("Invalid file format (not PPM)");

            // read image dimensions
            var dimensions = reader.ReadLine()?.Split(' ');
            if (dimensions is null) throw new Exception("Error occured while reading dimnesions in PPM");
            width = int.Parse(dimensions[0]);
            height = int.Parse(dimensions[1]);
            pixels = new Color[width, height];

            // read max color value
            maxColor = int.Parse(reader.ReadLine() ?? "0");
            if (maxColor == 0) throw new Exception("Error occured while reading max color value in PPM");

            // read the PPM file pixel data
            // read pixel data
            for (var j = 0; j < height; j++)
            {
                var line = reader.ReadLine() ?? string.Empty;
                if (line.StartsWith("#"))
                {
                    line = reader.ReadLine();
                    continue;
                }

                var pixelValues = line.Split(' ');
                for (var i = 0; i < width; i += 3)
                {
                    var r = int.Parse(pixelValues[i]);
                    var g = int.Parse(pixelValues[i + 1]);
                    var b = int.Parse(pixelValues[i + 2]);
                    pixels[i, j] = Color.FromArgb(r, g, b);
                }
            }
        }

        // var ppm = new ImagePPM(width, height, pixels);
        return pixels;
    }
}