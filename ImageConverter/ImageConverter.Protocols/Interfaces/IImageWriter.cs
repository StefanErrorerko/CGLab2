using System.Drawing;

namespace ImageConverter.Protocols.Interfaces;

public interface IImageWriter
{
    public string FileExtention { get; }
    public byte[] Write(Color[,] pixels);
}