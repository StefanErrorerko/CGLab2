using Microsoft.VisualBasic;
using RayCasting.Core.Objects;
using RayCasting.Core.Structures;
using RayCasting.Core.Tracer;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayCasting.Core.Lights
{
    public class PointLight : Light
    {
        readonly Point3 _position;

        public PointLight(Point3 position, Color color, float intensity)
            : base(color, intensity)
        {
            _position = position;
        }
        public PointLight(Point3 position) : this(position, Color.White, intensity: 1) { }

        public override Color GetPixel(Point3 point, IObject figure, List<IObject> objects)
        {
            var lightDirection = new Vector3(point - _position).Normalized();
            var lightRay = new Ray(point, _position);

            if (HasIntersectionWithAnyObject(lightRay, objects, figure))
            {
                return Color.Black;
            }

            var intersectionVector = figure.Normal(new Vector3(point));
            var coeff = (lightDirection * (-1.0f)).Dot(intersectionVector);
            coeff = Math.Max(coeff, 0) * _intensity;

            return Color.FromArgb(
                _color.A,
                (byte)Math.Round(_color.R * coeff, MidpointRounding.AwayFromZero),
                (byte)Math.Round(_color.G * coeff, MidpointRounding.AwayFromZero),
                (byte)Math.Round(_color.B * coeff, MidpointRounding.AwayFromZero));
        }

        internal static bool HasIntersectionWithAnyObject(Ray ray, List<IObject> objects, IObject figure)
        {
            foreach (var obj in objects)
            {
                var intersection = obj.GetIntersectionWith(ray);

                if (intersection.point is not null && obj != figure)
                    return true;
            }
            return false;
        }
    }
}
