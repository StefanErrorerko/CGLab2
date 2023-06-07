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

            var intersectionPoint = intersection.Point;
            var lightDirection = (intersectionPoint - _position).Normalized;
            var lightRay = new Ray(intersectionPoint, _position);

            if (HasIntersectionWithAnyObject(lightRay, sceneObjects, intersection.Object))
            {
                return Color.Black;
            }

            var intersectionVectorNormalized = intersection.Object.GetNormalAt(intersectionPoint);
            var lightCoefficient = (-lightDirection).DotProduct(intersectionVectorNormalized);
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
