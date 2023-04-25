using System.Drawing;

namespace CGLab2.Images;

public abstract class Image
{
    public Color[,] Pixels { get; set; } = new Color[0, 0];
    public int Width { get; set; }
    public int Height { get; set; }
}