using System.Drawing;
using ImageConverter.Protocols.Interfaces;

namespace BMPWriter;

public class BmpWriter : IImageWriter
{
    public string FileExtention => "bmp";

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

    public byte[] Write(Color[,] pixels)
    {
        var bytesPerPixel = sizeof(byte) * 3; // Assuming each color channel is represented by a byte (8 bits)
        var padding = (4 - pixels.GetLength(0) * bytesPerPixel % 4) % 4;
        var imageSize = pixels.GetLength(0) * pixels.GetLength(1) * bytesPerPixel;
        var pixelDataSize = imageSize + pixels.GetLength(1) * padding;
        var fileOffset = 54; // Offset where pixel data starts in BMP file
        var fileSize = fileOffset + pixelDataSize;

        var bytes = new byte[fileSize];

// BMP signature
        bytes[0] = 0x42; // 'B'
        bytes[1] = 0x4D; // 'M'

// File size
        bytes[2] = (byte)(fileSize);
        bytes[3] = (byte)(fileSize >> 8);
        bytes[4] = (byte)(fileSize >> 16);
        bytes[5] = (byte)(fileSize >> 24);

// Reserved
        bytes[6] = 0;
        bytes[7] = 0;
        bytes[8] = 0;
        bytes[9] = 0;

// File offset
        bytes[10] = (byte)(fileOffset);
        bytes[11] = (byte)(fileOffset >> 8);
        bytes[12] = (byte)(fileOffset >> 16);
        bytes[13] = (byte)(fileOffset >> 24);

// DIB header size (40 bytes for BITMAPINFOHEADER)
        bytes[14] = 0x28;
        bytes[15] = 0;
        bytes[16] = 0;
        bytes[17] = 0;

// Width
        bytes[18] = (byte)(pixels.GetLength(0));
        bytes[19] = (byte)(pixels.GetLength(0) >> 8);
        bytes[20] = (byte)(pixels.GetLength(0) >> 16);
        bytes[21] = (byte)(pixels.GetLength(0) >> 24);

// Height
        bytes[22] = (byte)(pixels.GetLength(1));
        bytes[23] = (byte)(pixels.GetLength(1) >> 8);
        bytes[24] = (byte)(pixels.GetLength(1) >> 16);
        bytes[25] = (byte)(pixels.GetLength(1) >> 24);

// Planes (1 plane)
        bytes[26] = 0x01;
        bytes[27] = 0;

// Color depth (24 bits)
        bytes[28] = 0x18;
        bytes[29] = 0;

        for (int i = 30; i <= 55; i++)
        {
            bytes[i] = 0;
        }

        int dataIndex = fileOffset;
        for (var j = 0; j < pixels.GetLength(1); j++)
        {
            for (var i = 0; i < pixels.GetLength(0); i++)
            {
                bytes[dataIndex++] = pixels[i, j].R;
                bytes[dataIndex++] = pixels[i, j].G;
                bytes[dataIndex++] = pixels[i, j].B;
            }

            for (var k = 0; k < padding; k++)
            {
                bytes[dataIndex++] = 0;
            }
        }

        return bytes;
    }
}