using CGLab2.Images;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CGLab2.ImageProcessors
{
    public class BMPWriter : IImageWriter
    {
        String filePath;
        ImageBMP bmp;
        public BMPWriter(String path, ImageBMP bmp)
        {
            filePath = path;
            this.bmp = bmp;
        }

        public void Write()
        {
            // Write the pixel data to a BMP file
            using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
            {
                // BMP header
                writer.Write((byte)bmp.Signature[0]);
                writer.Write((byte)bmp.Signature[1]);
                writer.Write(bmp.FileSize);
                writer.Write(bmp.Reserved);
                writer.Write(bmp.FileOffset);

                // DIB header
                writer.Write(bmp.DIBHeaderSize);
                writer.Write(bmp.Width);
                writer.Write(bmp.Height);
                writer.Write(bmp.Planes);
                writer.Write(bmp.ColorDepth);
                writer.Write(bmp.Compression);
                writer.Write(bmp.ImageSize);
                writer.Write(bmp.HorizontalResolution);
                writer.Write(bmp.VerticalResolution);
                writer.Write(bmp.ColorPaletteAmount);
                writer.Write((int)0);

                var bytesPerPixel = bmp.ColorDepth / 8;
                var padding = (4 - (bmp.Width * bytesPerPixel) % 4) % 4;
                var imageSize = bmp.Width * bmp.Height * bytesPerPixel;
                var pixelData = new byte[imageSize + bmp.Height * padding];

                for(int j=0; j< bmp.Height; j++)
                {
                    for (int i=0; i< bmp.Width; i++)
                    {
                        int offset = (bmp.Width * j + i) * bytesPerPixel + j * padding;
                        pixelData[offset] = bmp.Pixels[i, j].R;
                        pixelData[offset + 1] = bmp.Pixels[i, j].G;
                        pixelData[offset + 2] = bmp.Pixels[i, j].B;
                    }
                    for(int k = 0; k < padding; k++)
                    {
                        pixelData[(bmp.Width * (j + 1)) * bytesPerPixel + j * padding + k] = 0;
                    }
                }
                writer.Write(pixelData);
            }
        }
    }
}
