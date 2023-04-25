using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGLab2.Errors
{
    public class ImageFormatException : FormatException
    {
        public ImageFormatException() 
            : base() 
        { }
        public ImageFormatException(string message) 
            : base(message)
        {

        }
    }
}
