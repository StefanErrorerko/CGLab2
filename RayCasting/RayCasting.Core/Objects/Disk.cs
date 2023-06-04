using RayCasting.Core.Structures;
using RayCasting.Core.Tracer;

namespace RayCasting.Core.Objects;

public struct Disk : IObject
{
    // Properties

    private Vector3 _normal;
    public Vector3 Center;
    public float Radius;

    // Initialization

    public Disk(Vector3 center, Vector3 normal, float radius)
    {
        Center = center;
        _normal = normal.Normalized();
        Radius = radius;
    }

    public Vector3 Normal(Vector3 point)
    {
        return _normal;
    }

    public (Point3? point, float? t) GetIntersectionWith(Ray ray)
    {
        //var denominator = _normal.Dot(ray.Direction);
        //if (Math.Abs(denominator) < 1e-6) return (null, null);
        //var numerator = _normal.Dot(Center - ray.Origin);
        //var t = numerator / denominator;
        //if (t < 0) return (null, null);
        //var point = ray.PointAt(t);
        //if ((point - Center).Length > Radius) return (null, null);
        //var intersectionPoint = ray.Origin + ray.Direction * t;
        //return new ValueTuple<Point3?, float?>(
        //    new Point3(intersectionPoint.X, intersectionPoint.Y, intersectionPoint.Z),
        //    t);
        throw new NotImplementedException("");
    }
}