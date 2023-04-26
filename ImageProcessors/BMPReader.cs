﻿using System.Drawing;
using CGLab2.Errors;
using CGLab2.Images;
using Image = CGLab2.Images.Image;

namespace CGLab2.ImageProcessors;

public class BMPReader : IImageReader
{
    private readonly string _filePath;

    public BMPReader(string path)
    {
        _filePath = path;
    }

    public Image Read()
    {
        using (var stream = new FileStream(_filePath, FileMode.Open))
        {
            // Read file header (14 bytes)
            var header = new byte[14];
            stream.Read(header, 0, header.Length);
            //if header BM
            if (header[0] != 'B' || header[1] != 'M') throw new ImageFormatException("Invalid file format (not BMP)");
            var pixelDataOffset = BitConverter.ToInt32(header, 10);

            // Read DIB header
            var dibHeader = new byte[40];
            stream.Read(dibHeader, 0, dibHeader.Length);
            var width = BitConverter.ToInt32(dibHeader, 4);
            var height = BitConverter.ToInt32(dibHeader, 8);
            var bitsPerPixel = BitConverter.ToInt16(dibHeader, 14);
            var compressionMethod = BitConverter.ToInt32(dibHeader, 16);

            // Read pixel data
            var bytesPerPixel = bitsPerPixel / 8;
            //var rowSize = ((width * bitsPerPixel + 31) / 32) * 4;
            var padding = (4 - width * bytesPerPixel % 4) % 4;
            var imageSize = width * height * bytesPerPixel;
            var pixelData = new byte[imageSize + height * padding];
            stream.Seek(pixelDataOffset, SeekOrigin.Begin);
            stream.Read(pixelData, 0, imageSize);

                // Store pixel data (in this example, just output it)
                var pixels = new Color[width, height];
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        var offset = (width * y + x) * bytesPerPixel + y * padding;
                        var r = pixelData[offset];
                        var g = pixelData[offset + 1];
                        var b = pixelData[offset + 2];
                        var pixel = Color.FromArgb(r, g, b);
                        pixels[x, y] = pixel;
                    }
                }
                var bmpImage = new ImageBMP(width, height, pixels);
                return bmpImage;
            }
        }
    }
}
