using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CGLab2.Images;

namespace CGLab2.ImageProcessors
{
    public class PPMWriter : IImageWriter
    {
        String filePath;
        ImagePPM ppm;
        public PPMWriter(String path, ImagePPM ppm) 
        {
            filePath = path;
            this.ppm = ppm;
        }

        public void Write()
        {
            var writer = new StreamWriter(filePath);
            writer.WriteLine(ppm.Header);
            writer.WriteLine($"{ppm.Width} {ppm.Height}");
            writer.WriteLine(ppm.MaxValue);
            for (int j = ppm.Height - 1; j >= 0; j--)
            {
                for (int i = 0; i < ppm.Width; i++)
                {
                    writer.Write((int)(ppm.Pixels[i, j].R));
                    writer.Write((int)ppm.Pixels[i, j].G);
                    writer.Write((int)ppm.Pixels[i, j].B);
                }
                writer.WriteLine();
            }
            writer.Close();
        }
    }
}
