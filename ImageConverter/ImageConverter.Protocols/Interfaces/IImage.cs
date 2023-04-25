using System.Drawing;

namespace ImageConverter.Protocols.Interfaces;

public interface IImage
{
    public Color[,] Pixels { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}