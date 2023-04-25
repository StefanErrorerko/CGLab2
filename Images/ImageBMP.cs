using System.Drawing;

namespace CGLab2.Images
{
    public class ImageBMP : Image
    {
        #region Bitmap File Header
        public String Signature { get; } = "BM";
        public int FileSize { get; set; }
        public int Reserved { get;set; } = 0;
        public int FileOffset { get; set; }
        #endregion

        #region DIB Header
        public int DIBHeaderSize = 40;
        public short Planes { get; } = 1;
        public short ColorDepth { get; set; } = 24;
        public int Compression { get; set; } = 0;
        public int ImageSize { get; set;}
        public int HorizontalResolution { get; set; } = 2835;
        public int VerticalResolution { get; set; } = 2835;
        public int ColorPaletteAmount { get; set; } = 0;
        #endregion
        public ImageBMP(int width, int height, Color[,] pixels)
        {
            Pixels = pixels;
            Signature = "BM";
            DIBHeaderSize = 40;
            FileSize = 14 + DIBHeaderSize + Pixels.Length;
            FileOffset = 14 + DIBHeaderSize;
            ImageSize = Pixels.Length;
            Width = width;
            Height = height;
        }
    }
}
