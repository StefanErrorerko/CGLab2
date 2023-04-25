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
        var writer = new StreamWriter(_filePath);
        writer.WriteLine(_ppm.Header);
        writer.WriteLine($"{_ppm.Width} {_ppm.Height}");
        writer.WriteLine(_ppm.MaxValue);
        for (var j = _ppm.Height - 1; j >= 0; j--)
        {
            for (var i = 0; i < _ppm.Width; i++)
            {
                writer.Write(_ppm.Pixels[i, j].R);
                writer.Write(_ppm.Pixels[i, j].G);
                writer.Write(_ppm.Pixels[i, j].B);
            }

            writer.WriteLine();
        }

        writer.Close();
    }
}