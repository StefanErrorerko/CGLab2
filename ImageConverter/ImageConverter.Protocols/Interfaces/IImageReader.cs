using System.Drawing;

namespace ImageConverter.Protocols.Interfaces;

public interface IImageReader
{
    public string FileExtention { get; }
    public Color[,] Read(byte[] bytes);
    public bool ValidateHeader(byte[] bytes);
}