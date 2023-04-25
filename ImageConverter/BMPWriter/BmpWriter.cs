﻿using System.Drawing;
using ImageConverter.Protocols.Interfaces;

namespace BMPWriter;

public class BmpWriter : IImageWriter
{
    public void Write(string filePath, Color[,] pixels)
    {
    //     // Write the pixel data to a BMP file
    //     using var writer = new BinaryWriter(File.Open(filePath, FileMode.Create));
    //     // BMP header
    //     writer.Write((byte)_bmp.Signature[0]);
    //     writer.Write((byte)_bmp.Signature[1]);
    //     writer.Write(_bmp.FileSize);
    //     writer.Write(_bmp.Reserved);
    //     writer.Write(_bmp.FileOffset);
    //
    //     // DIB header
    //     writer.Write(_bmp.DibHeaderSize);
    //     writer.Write(_bmp.Width);
    //     writer.Write(_bmp.Height);
    //     writer.Write(_bmp.Planes);
    //     writer.Write(_bmp.ColorDepth);
    //     writer.Write(_bmp.Compression);
    //     writer.Write(_bmp.ImageSize);
    //     writer.Write(_bmp.HorizontalResolution);
    //     writer.Write(_bmp.VerticalResolution);
    //     writer.Write(_bmp.ColorPaletteAmount);
    //     writer.Write(0);
    //
    //     var bytesPerPixel = _bmp.ColorDepth / 8;
    //     var padding = (4 - _bmp.Width * bytesPerPixel % 4) % 4;
    //     var imageSize = _bmp.Width * _bmp.Height * bytesPerPixel;
    //     var pixelData = new byte[imageSize + _bmp.Height * padding];
    //
    //     for (var j = 0; j < _bmp.Height; j++)
    //     {
    //         for (var i = 0; i < _bmp.Width; i++)
    //         {
    //             var offset = (_bmp.Width * j + i) * bytesPerPixel + j * padding;
    //             pixelData[offset] = _bmp.Pixels[i, j].R;
    //             pixelData[offset + 1] = _bmp.Pixels[i, j].G;
    //             pixelData[offset + 2] = _bmp.Pixels[i, j].B;
    //         }
    //
    //         for (var k = 0; k < padding; k++) pixelData[_bmp.Width * (j + 1) * bytesPerPixel + j * padding + k] = 0;
    //     }
    //
    //     writer.Write(pixelData);
    }
}