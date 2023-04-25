using System.Drawing;

namespace ImageConverter.Protocols.Interfaces;

public interface IImageWriter
{
    public void Write(string filePath, Color[,] pixels);
}