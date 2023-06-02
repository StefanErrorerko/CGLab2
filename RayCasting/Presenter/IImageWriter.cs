using System.Drawing;

namespace ImageWriter;

public interface IImageWriter
{
    public string FileExtention { get; }
    public byte[] Write(Color[,] pixels);
}