using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayCasting.Core.Lights
{
    public class AreaLight : Light
    {
        public AreaLight(Color color, float intensity) : base(color, intensity) { }
    }
}
