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

            var reversedLightRay = new Ray(point, point - lightNormalized);
            if (HasIntersectionWithAnyObject(reversedLightRay, objects, figure))
            {
                return Color.Black;
            }

            var intersectionVectorNormalized = figure.Normal(new Vector3(point));
            var lightCoefficient = (-1 * lightNormalized).DotProduct(intersectionVectorNormalized);
            lightCoefficient = Math.Max(lightCoefficient, 0);
            lightCoefficient *= _intensity;

            return Color.FromShadowedColor(lightCoefficient, _color);
        }

        internal static bool HasIntersectionWithAnyObject(Ray ray, IEnumerable<ISceneObject> sceneObjects, ISceneObject bypassObject)
        {
            foreach (var sceneObject in sceneObjects)
            {
                var intersectionPoint = sceneObject.GetIntersection(ray);

                if (intersectionPoint is not null && intersectionPoint.Value.Object != bypassObject)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
