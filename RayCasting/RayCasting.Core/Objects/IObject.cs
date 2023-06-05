using RayCasting.Core.BoundingVolumeHierarchies;
using RayCasting.Core.Structures;
using RayCasting.Core.Tracer;

namespace RayCasting.Core.Objects;

public interface IObject
{
    Vector3 Normal(Vector3 vector3);
    (Point3? point, float? t) GetIntersectionWith(Ray ray);
    BoundingBox GetBoundingBox();
    float Reflects(Vector3 lightRay, Vector3 surfacePoint)
    {
        return Normal(surfacePoint).Dot(lightRay);
    }

    bool Intersects(Ray ray, out double d);
}