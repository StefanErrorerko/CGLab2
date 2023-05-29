using RayCasting.Core.Structures;
using RayCasting.Core.Tracer;

namespace RayCasting.Core.Objects;

public struct Sphere : IObject
{
    public Vector3 Center { get; }
    public float Radius { get; }

    public Sphere(Vector3 center, float radius)
    {
        Center = center;
        Radius = radius;
    }

    public Vector3 Normal(Vector3 point)
    {
        return (Center - point).Normalized();
    }

    public float? Intersects(Ray ray)
    {
        Vector3 dir = ray.Direction;
        Vector3 s = ray.Origin - Center;
        float a = dir.Dot(dir);
        float b = 2 * s.Dot(ray.Direction);
        float c = s.Dot(s) - (float)Math.Pow(Radius, 2);
        float discriminant = b * b - 4 * a * c;

        if (discriminant >= 0)
        {
            float t1 = (-b - (float)Math.Sqrt(discriminant)) / (2 * a);
            float t2 = (-b + (float)Math.Sqrt(discriminant)) / (2 * a);

            float t = Math.Min(t1, t2);

            if (t < 0)
            {
                return null;
            }
            return t;
        }
        return null;
    }
}