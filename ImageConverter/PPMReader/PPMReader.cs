using System.Drawing;
using ImageConverter.Protocols.Interfaces;

namespace PPMReader;

public class PpmReader : IImageReader
{
    private readonly string _filePath = "";
    public string FileExtention => "ppm";
    public PpmReader()
    {
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

    public Color[,] Read(byte[] bytes)
    {
        int width,
            height,
            maxColor;
        Color[,] pixels;

        //using var reader = new StreamReader(_filePath);
        // if (dimensions is null) throw new Exception("Error occured while reading dimnesions in PPM");
        var lines = System.Text.Encoding.Default.GetString(bytes).Split('\n');
        var dimensions = lines[1].Split(' ');
        width = int.Parse(dimensions[0]);
        height = int.Parse(dimensions[1]);
        pixels = new Color[width, height];

        maxColor = int.Parse(lines[2]);
        // if (maxColor == 0) throw new Exception("Error occured while reading max color value in PPM");

        for (var j = height - 1; j >= 0 ; j--)
        {
            var line = lines[3 + (height - 1 - j)];
            if (line.StartsWith('#'))
            {
                continue;
            }

            var values = line.Split(' ');
            for (int i = 0; i < width; i++)
            {
                var r = Int32.Parse(values[3 * i]);
                var g = Int32.Parse(values[3 * i + 1]);
                var b = Int32.Parse(values[3 * i + 2]);
                pixels[i, j] = Color.FromArgb(b, g, r);
            }
        }

        // var ppm = new ImagePPM(width, height, pixels);
        return pixels;
    }

    public bool ValidateHeader(byte[] bytes)
    {
        if (bytes == null || bytes.Length < 3)
        {
            return false;
        }

        if (bytes[0] != 'P' || bytes[1] != '3')
        {
            return false;
        }

        if (!char.IsWhiteSpace((char)bytes[2]))
        {
            return false;
        }

        return true;
    }
}