using CGLab2.Images;

namespace CGLab2.ImageProcessors;

public class PPMWriter : IImageWriter
{
    private readonly string _filePath;
    private readonly ImagePPM _ppm;

    public PPMWriter(string path, ImagePPM ppm)
    {
        _filePath = path;
        _ppm = ppm;
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
                    writer.Write($"{ppm.Pixels[i, j].B} ");
                    writer.Write($"{ppm.Pixels[i, j].G} ");
                    writer.Write($"{ppm.Pixels[i, j].R} ");
                    
                }
                writer.Write('\n');
            }
            writer.Close();
        }
    }
}
