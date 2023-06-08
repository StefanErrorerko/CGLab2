using RayCasting.Core.Objects;
using RayCasting.Core.Structures;
using RayCasting.Core.Tracer;
using System.Drawing;

namespace RayCasting.Core.Lights
{
    public class DirectionalLight : Light
    {
        private Vector3 _direction;

        public DirectionalLight(Vector3 direction, float intensity, Color color)
            : base(color, intensity)
        {
            _direction = direction;
        }

        public DirectionalLight(Vector3 direction) : this(direction, intensity: 1, Color.White)
        {
        }

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
            coeff = Math.Max(coeff, 0) * Intensity;

            return Color.FromArgb(
                Color.A,
                (byte)Math.Round(Color.R * coeff, MidpointRounding.AwayFromZero),
                (byte)Math.Round(Color.G * coeff, MidpointRounding.AwayFromZero),
                (byte)Math.Round(Color.B * coeff, MidpointRounding.AwayFromZero));
        }

        private static bool HasIntersectionWithAnyObject(Ray ray, List<IObject> objects, IObject figure)
        {
            foreach (var obj in objects)
            {
                var (point, _) = obj.GetIntersectionWith(ray);
                if (point is not null && obj != figure)
                    return true;
            }

            return false;
        }
    }
}