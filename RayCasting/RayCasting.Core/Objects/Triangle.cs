using RayCasting.Core.Structures;
using RayCasting.Core.Tracer;

namespace RayCasting.Core.Objects;

public struct Triangle : IObject
{
    public Point3 V1 { get; }
    public Point3 V2 { get; }
    public Point3 V3 { get; }

    private Vector3 _normal;

    public Triangle(Point3 p1, Point3 p2, Point3 p3)
    {
        V1 = p1;
        V2 = p2;
        V3 = p3;
        _normal = new Vector3(p1, p2).Cross(new Vector3(p1, p3)).Normalized();
    }

    public Vector3 Normal(Vector3 point)
    {
        return _normal;
    }

    public float? Intersects(Ray ray)
    {
        const float epsilon = 0.000001f;

        // Calculate edge vectors
        var e1 = V2 - V1;
        var e2 = V3 - V1;

        // Calculate normal vector
        var pvec = ray.Direction.Cross(e2);
        var det = e1.Dot(pvec);

        // Check if ray and triangle are parallel
        if (MathF.Abs(det) < epsilon) return null;

        var invDet = 1f / det;

        // Calculate distance from V1 to ray origin
        var tvec = new Vector3(ray.Origin.X - V1.X, ray.Origin.Y - V1.Y, ray.Origin.Z - V1.Z);

        // Calculate u parameter
        var u = tvec.Dot(pvec) * invDet;

        // Check if u is outside the triangle
        if (u < 0f || u > 1f) return null;

        // Calculate v parameter
        var qvec = tvec.Cross(e1);
        var v = ray.Direction.Dot(qvec) * invDet;

        // Check if v is outside the triangle
        if (v < 0f || u + v > 1f) return null;

        // Calculate distance from ray origin to intersection point
        var t = e2.Dot(qvec) * invDet;

        // Check if intersection point is behind ray origin
        if (t < epsilon) return null;

        return t;
    }

    public Point3? GetIntersectionPointWith(Ray ray)
    {
        throw new NotImplementedException();
    }


    public override string ToString()
    {
        return $"V1: ({V1.X}, {V1.Y}, {V1.Z})\n" +
               $"V2: ({V2.X}, {V2.Y}, {V2.Z})\n" +
               $"V3: ({V3.X}, {V3.Y}, {V3.Z})";
    }
}