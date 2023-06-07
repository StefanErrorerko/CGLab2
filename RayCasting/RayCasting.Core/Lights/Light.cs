using RayCasting.Core.Objects;
using RayCasting.Core.Structures;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayCasting.Core.Lights
{
    public abstract class Light
    {
        protected readonly Color _color;
        protected readonly float _intensity;

        public Light(Color color, float intensity)
        {
            _color = color;
            if (intensity is < 0 or > 1)
                throw new ArgumentOutOfRangeException("Intensity value is out of range");
            _intensity = intensity;
        }

        public Light(float intensity) : this(Color.White, intensity) { }
        public virtual Color GetPixel(Point3 point, IObject figure, List<IObject> objects)
        {
            var coeff = 1.0f;
            coeff *= _intensity;
            return Color.FromArgb(
                _color.A,
                (byte)Math.Round(_color.R * coeff, MidpointRounding.AwayFromZero),
                (byte)Math.Round(_color.G * coeff, MidpointRounding.AwayFromZero),
                (byte)Math.Round(_color.B * coeff, MidpointRounding.AwayFromZero));
        }
    }
}
