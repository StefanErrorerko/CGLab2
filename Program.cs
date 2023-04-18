using CGLab2.Menus;
using CGLab2.Images;
using CGLab2.Images.Extensions;
using System;

namespace CGLab2
{
    public class Program
    {
        private static void Main(string[] args)
        {
            var menu = new MainMenu(args);
            menu.Display();

            var bmp = new ImageBMP();
            var gif = new ImageGIF();

            //IImageReader child
            using (var reader = new StreamReader(args[0]))
            {
                // read bmp
                //read gif
            }

            var gifBMPed = gif.ToBMP();

            //IImageWriter child
            using(var  writer = new StreamWriter(args[1]))
            {
                //write bmp
            }
        }
    }
}