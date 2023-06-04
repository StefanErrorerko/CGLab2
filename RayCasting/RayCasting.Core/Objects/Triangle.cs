using RayCasting.Core.Structures;
using RayCasting.Core.Tracer;

namespace RayCasting.Core.Objects;

public struct Triangle : IObject
{
    public Point3 V1 { get; }
    public Point3 V2 { get; }
    public Point3 V3 { get; }

    private readonly Vector3 _normal;

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
    
    public (Point3? point, float? t) GetIntersectionWith(Ray ray)
    {
        const float epsilon = 0.000001f;

        var vertex0 = new Vector3(V1);
        var vertex1 = new Vector3(V2);
        var vertex2 = new Vector3(V3);

        var edge1 = vertex1 - vertex0;
        var edge2 = vertex2 - vertex0;

        var h = ray.Direction.Cross(edge2);
        var a = edge1.Dot(h);

        if (a > -epsilon && a < epsilon) return (null, null);

        var f = 1.0f / a;
        var s = ray.Origin - vertex0;
        var u = f * s.Dot(h);

        if (u < 0.0f || u > 1.0f) return (null, null);

        var q = s.Cross(edge1);
        var v = f * ray.Direction.Dot(q);

        if (v < 0.0f || u + v > 1.0f) return (null, null);

        var t = f * edge2.Dot(q);

        if (t > epsilon)
        {
            var intersectionPoint = ray.Origin + ray.Direction * t;
            return (new Point3(intersectionPoint.X, intersectionPoint.Y, intersectionPoint.Z),
                t);
        }

        return (null, null);
    }
    
    public override string ToString()
    {
        return $"V1: ({V1.X}, {V1.Y}, {V1.Z})\n" +
               $"V2: ({V2.X}, {V2.Y}, {V2.Z})\n" +
               $"V3: ({V3.X}, {V3.Y}, {V3.Z})";
    }
}