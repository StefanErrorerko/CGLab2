using System.Drawing;
using System.IO.Compression;
using ImageConverter.Protocols.Interfaces;

namespace PNGWriter;

public class PngWriter : IImageWriter
{
    public string FileExtention => "png";
    public void Write(string filePath, Color[,] pixels)
    {
        throw new NotImplementedException();
    }

    public byte[] Write(Color[,] pixels)
    {
        throw new NotImplementedException();
    }
}