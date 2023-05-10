using System.Drawing;
using System.IO.Compression;
using System.Text;
using ImageConverter.Protocols.Interfaces;

namespace PNGReader;

public class PngReader : IImageReader
{
    public string FileExtention => "png";

    public Color[,] Read(byte[] bytes)
    {
        int ihdrIndex = FindChunk(bytes, 0, "IHDR");
        if (ihdrIndex == -1)
        {
            throw new InvalidOperationException("IHDR chunk not found.");
        }

        int width = ReadInt32(bytes, ihdrIndex + 8);
        int height = ReadInt32(bytes, ihdrIndex + 12);

        int idatIndex = FindChunk(bytes, 0, "IDAT");
        if (idatIndex == -1)
        {
            throw new InvalidOperationException("IDAT chunk not found.");
        }

        byte[] uncompressedData = DecompressZlib(bytes, idatIndex + 8);

        Color[,] pixels = new Color[width, height];

        int pixelIndex = 0;
        for (int y = 0; y < height; y++)
        {
            int filterType = uncompressedData[pixelIndex++];

            for (int x = 0; x < width; x++)
            {
                byte red = uncompressedData[pixelIndex++];
                byte green = uncompressedData[pixelIndex++];
                byte blue = uncompressedData[pixelIndex++];
                byte alpha = 255;

                pixels[x, y] = Color.FromArgb(alpha, red, green, blue);
            }
        }

        return pixels;
    }

    public bool ValidateHeader(byte[] bytes)
    {
        if (bytes.Length < 8)
        {
            return false;
        }

        return bytes[0] == 0x89 && bytes[1] == 0x50 && bytes[2] == 0x4E && bytes[3] == 0x47 &&
               bytes[4] == 0x0D && bytes[5] == 0x0A && bytes[6] == 0x1A && bytes[7] == 0x0A;
    }

    private int FindChunk(byte[] bytes, int startIndex, string chunkType)
    {
        int index = startIndex;
        while (index < bytes.Length - 12)
        {
            int length = ReadInt32(bytes, index);
            string type = Encoding.ASCII.GetString(bytes, index + 4, 4);

            if (type == chunkType)
            {
                return index;
            }

            index += 12 + length;
        }

        return -1;
    }

    private int ReadInt32(byte[] bytes, int index)
    {
        return (bytes[index] << 24) | (bytes[index + 1] << 16) | (bytes[index + 2] << 8) | bytes[index + 3];
    }

    private byte[] DecompressZlib(byte[] bytes, int startIndex)
    {
        startIndex += 2;

        byte[] output = new byte[bytes.Length * 8];

        using (MemoryStream memoryStream = new MemoryStream(bytes, startIndex, bytes.Length - startIndex))
        {
            using (DeflateStream deflateStream = new DeflateStream(memoryStream, CompressionMode.Decompress))
            {
                MemoryStream outputStream;
                using (outputStream = new MemoryStream())
                {
                    int bytesRead;
                    byte[] buffer = new byte[1024];
                    while ((bytesRead = deflateStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        outputStream.Write(buffer, 0, bytesRead);
                    }
                }

                return outputStream.ToArray();
            }
        }
    }
}