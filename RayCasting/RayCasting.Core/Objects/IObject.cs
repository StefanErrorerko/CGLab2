using RayCasting.Core.Structures;
using RayCasting.Core.Tracer;

namespace RayCasting.Core.Objects;

public interface IObject
{
    Vector3 Normal(Vector3 point);
    float? Intersects(Ray ray);
    float Reflects(Vector3 lightRay, Vector3 surfacePoint)
    {
        return Normal(surfacePoint).Dot(lightRay);
    }
}