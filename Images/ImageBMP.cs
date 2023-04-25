using System.Drawing;

namespace CGLab2.Images;

public class ImageBmp : Image
{
    public ImageBmp(int width, int height, Color[,] pixels)
    {
        Pixels = pixels;
        Signature = "BM";
        DibHeaderSize = 40;
        FileSize = 14 + DibHeaderSize + Pixels.Length;
        FileOffset = 14 + DibHeaderSize;
        ImageSize = Pixels.Length;
        Width = width;
        Height = height;
    }

    #region Bitmap File Header

    public string Signature { get; } = "BM";
    public int FileSize { get; set; }
    public int Reserved { get; set; } = 0;
    public int FileOffset { get; set; }

    #endregion

    #region DIB Header

    public int DibHeaderSize = 40;
    public short Planes { get; } = 1;
    public short ColorDepth { get; set; } = 24;
    public int Compression { get; set; } = 0;
    public int ImageSize { get; set; }
    public int HorizontalResolution { get; set; } = 2835;
    public int VerticalResolution { get; set; } = 2835;
    public int ColorPaletteAmount { get; set; } = 0;

    #endregion
}