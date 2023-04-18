using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGLab2.Menus
{
    public class MainMenu
    {
        String[] args;

        public MainMenu(String[] args)
        {
            this.args = args;
        }

        public void Display()
        {
            Console.WriteLine("Hello, stranger!");
            if(args.Length != 2)
            {
                MenuIfNoArguments();
            }
            return;
        }

        public void MenuIfNoArguments()
        {
            Console.WriteLine("Please type an image source path and output image format using space:");
            while (true)
            {
                try
                {
                    var args = Console.ReadLine()?.Split(' ');
                    _ = args ?? throw new NullReferenceException();
                    break;
                }
                catch (NullReferenceException)
                {
                    //smth to output
                }
            }
            return;
        }
    }
}
