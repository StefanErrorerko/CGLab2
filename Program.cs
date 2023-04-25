using CGLab2.ImageProcessors;
using CGLab2.Images;
using CGLab2.Images.Extensions;
using CGLab2.Menus;

namespace CGLab2;

public class Program
{
    private static void Main(string[] args)
    {
        var menu = new MainMenu(args);
        var formats = new[] { "bmp", "ppm" };
        var startPath = menu.Display();
        var goalPath = startPath.Substring(0, startPath.LastIndexOf('\\'));

        var results = menu.MenuFormatCheck(formats);
        Image image;
        if (results[0] == formats[0])
        {
            var bmpReader = new BMPReader(startPath);
            image = bmpReader.Read();
            if (results[1] == formats[0])
            {
                var bmped = image.ToBMP();
                goalPath += "output.bmp";
                var bmpWriter = new BmpWriter(goalPath, bmped);
                bmpWriter.Write();
            }
            else if (results[1] == formats[1])
            {
                var ppmed = image.ToPPM();
                goalPath += "output.ppm";
                var ppmWriter = new PPMWriter(goalPath, ppmed);
                ppmWriter.Write();
            }
        }
        else if (results[0] == formats[1])
        {
            var ppmReader = new PPMReader(startPath);
            image = ppmReader.Read();
            if (results[1] == formats[0])
            {
                var bmped = image.ToBMP();
                var bmpWriter = new BmpWriter(goalPath, bmped);
                bmpWriter.Write();
            }
            else if (results[1] == formats[1])
            {
                var ppmed = image.ToPPM();
                var ppmWriter = new PPMWriter(goalPath, ppmed);
                ppmWriter.Write();
            }
        }

        menu.LastMenu();
        //var goalBMPPath = @"C:\Users\chere\source\repos\CGLab2\TestPics\output.bmp";
        //var goalPPMPath = @"C:\Users\chere\source\repos\CGLab2\TestPics\output.ppm";
    }
}