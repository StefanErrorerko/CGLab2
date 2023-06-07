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
    public class DirectionalLight : Light
    {
        readonly Vector3 _direction;

        public DirectionalLight(Vector3 direction, float intensity, Color color)
            : base(color, intensity)
        {
            _direction = direction;
        }
        public DirectionalLight(Vector3 direction) : this(direction, intensity: 1, Color.White) { }

        public override Color GetPixel(Point3 point, IObject figure, List<IObject> objects)
        {
            var lightNormalized = _direction.Normalized();

            var reversedRay = new Ray(point, point - lightNormalized);
            if (HasIntersectionWithAnyObject(reversedRay, objects, figure))
            {
                return Color.Black;
            }

            var intersectionVector = figure.Normal(new Vector3(point));
            var coeff = (lightNormalized * (-1.0f)).Dot(intersectionVector);
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
                var (point, t) = obj.GetIntersectionWith(ray);
                if (point is not null && obj != figure)
                    return true;
            }
            return false;
        }
    }
}
