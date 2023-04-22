using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace CGLab2.Images
{
    public abstract class Image
    {
        public Color[,] Pixels { get; set; }
        public int Width { get;set; }
        public int Height { get; set; }
    }
}
