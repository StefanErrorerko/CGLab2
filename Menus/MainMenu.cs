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

        public String Display()
        {
            Console.WriteLine("Hello, stranger!");
            if(args.Length != 2)
            {
                MenuIfNoArguments();
            }
            return args[0];
        }

        public String MenuIfNoArguments()
        {
            Console.WriteLine("Please type an image source path and output image format using space:");
            while (true)
            {
                try
                {
                    args = Console.ReadLine()?.Split(' ');
                    _ = args ?? throw new NullReferenceException();
                    break;
                }
                catch (NullReferenceException)
                {
                    //smth to output
                }
            }
            return args[0];
        }

        public String[] MenuFormatCheck(String[] formats)
        {
            String inputFormat;
            var index = args[0].IndexOf('.');
            if (index >= 0)
            {
                inputFormat = args[0].Substring(index + 1);
                if (IfSupportedFormat(formats, inputFormat) && IfSupportedFormat(formats, args[1]))
                {
                    return new String[] { inputFormat, args[1] };
                }
            }
            MenuIfNoArguments();
            var result = MenuFormatCheck(formats);
            return result;
        }

        public void LastMenu()
        {
            Console.WriteLine("Successfully converted.");
        }

        public bool IfSupportedFormat(String[] formats, String format)
        {
            if (formats.Contains(format))
            {
                return true;
            }
            return false;
        } 
        
    }
}
