using System.Drawing;
using ImageConverter.Protocols.Interfaces;

namespace BMPReader;

public class BmpReader : IImageReader
{
    public string FileExtention => "bmp";

    public Color[,] Read(byte[] bytes)
    {
        var header = new byte[14];
        Array.Copy(bytes, 0, header, 0, header.Length);

        var pixelDataOffset = BitConverter.ToInt32(header, 10);

        var dibHeader = new byte[40];
        Array.Copy(bytes, 14, dibHeader, 0, dibHeader.Length);
        var width = BitConverter.ToInt32(dibHeader, 4);
        var height = BitConverter.ToInt32(dibHeader, 8);
        var bitsPerPixel = BitConverter.ToInt16(dibHeader, 14);
        var compressionMethod = BitConverter.ToInt32(dibHeader, 16);

        var bytesPerPixel = bitsPerPixel / 8;
        var padding = (4 - width * bytesPerPixel % 4) % 4;
        var imageSize = width * height * bytesPerPixel;
        var pixelData = new byte[imageSize + height * padding];
        Array.Copy(bytes, pixelDataOffset, pixelData, 0, imageSize);

        var pixels = new Color[width, height];
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var offset = (width * y + x) * bytesPerPixel + y * padding;
                var r = pixelData[offset];
                var g = pixelData[offset + 1];
                var b = pixelData[offset + 2];
                var pixel = Color.FromArgb(r, g, b);
                pixels[x, y] = pixel;
            }
        }

        return pixels;
    }

    public bool ValidateHeader(byte[] fileContent)
    {
        if (fileContent == null || fileContent.Length < 14) return false;

        if (fileContent[0] != 'B' || fileContent[1] != 'M') return false;

        var pixelDataOffset = BitConverter.ToInt32(fileContent, 10);

        if (pixelDataOffset < 14) return false;

        return true;
    }
}