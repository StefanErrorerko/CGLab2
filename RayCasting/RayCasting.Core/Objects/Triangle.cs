using RayCasting.Core.BoundingVolumeHierarchies;
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
        //const float epsilon = 0.000001f;

        //var vertex0 = new Vector3(V1);
        //var vertex1 = new Vector3(V2);
        //var vertex2 = new Vector3(V3);

        //var edge1 = vertex1 - vertex0;
        //var edge2 = vertex2 - vertex0;

        //var h = ray.Direction.Cross(edge2);
        //var a = edge1.Dot(h);

        //if (Math.Abs(a) < epsilon) return (null, null);

        //var f = 1.0f / a;
        //var s = ray.Origin - vertex0;
        //var u = f * s.Dot(h);

        //if (u < 0.0f || u > 1.0f) return (null, null);

        //var q = s.Cross(edge1);
        //var v = f * ray.Direction.Dot(q);

        //if (v < 0.0f || u + v > 1.0f) return (null, null);

        //var t = f * edge2.Dot(q);

        //if (t > epsilon)
        //{
        //    var intersectionPoint = ray.Origin + ray.Direction * t;
        //    return (new Point3(intersectionPoint.X, intersectionPoint.Y, intersectionPoint.Z),
        //        t);
        //}
        //return (null, null);

        var v1v2 = new Vector3(V1, V2);
        var v1v3 = new Vector3(V1, V3);
        var N = v1v2.Cross(v1v3);

        float NRayDirection = N.Dot(ray.Direction);
        if (Math.Abs(NRayDirection) < Single.Epsilon)
            return (null, null);

        // compute d parameter using equation 2
        var d = -N.Dot(new Vector3(V1));

        var t = -(N.Dot(new Vector3(ray.Origin)) + d) / NRayDirection;

        // check if the triangle is behind the ray
        if (t < 0)
            return (null, null); // the triangle is behind

        // compute the intersection point using equation 1
        var P = new Vector3(ray.PointAt(t));

        // Step 2: inside-outside test
        Vector3 C; // vector perpendicular to triangle's plane

        // edge 0
        var edge0 = new Vector3(V1, V2);
        var vp0 = P - new Vector3(V1);
        C = edge0.Cross(vp0);
        if (N.Dot(C) < 0)
            return (null, null); // P is on the right side

        // edge 1
        var edge1 = new Vector3(V2, V3);
        var vp1 = P - new Vector3(V2);
        C = edge1.Cross(vp1);
        if (N.Dot(C) < 0)
            return (null, null); // P is on the right side

        // edge 2
        var edge2 = new Vector3(V3, V1);
        var vp2 = P - new Vector3(V3);
        C = edge2.Cross(vp2);
        if (N.Dot(C) < 0)
            return (null, null); // P is on the right side;
        return (new Point3(P.X, P.Y, P.Z), t);
    }

    public BoundingBox GetBoundingBox()
    {
        throw new NotImplementedException();
    }

    public bool Intersects(Ray ray, out double d)
    {
        throw new NotImplementedException();
    }

    private BoundingBox CalculateBoundingBox( int start, int end)
    {
        float minX = Math.Min(V1.X, Math.Min(V2.X, V3.X));
        float minY = Math.Min(V1.Y, Math.Min(V2.Y, V3.Y));
        float minZ = Math.Min(V1.Z, Math.Min(V2.Z, V3.Z));
        
        float maxX = Math.Max(V1.X, Math.Max(V2.X, V3.X));
        float maxY = Math.Max(V1.Y, Math.Max(V2.Y, V3.Y));
        float maxZ = Math.Max(V1.Z, Math.Max(V2.Z, V3.Z));
        
        return new BoundingBox(new Point3(minX, minY, minZ), new Point3(maxX, maxY, maxZ));
    }
    
    public override string ToString()
    {
        return $"V1: ({V1.X}, {V1.Y}, {V1.Z})\n" +
               $"V2: ({V2.X}, {V2.Y}, {V2.Z})\n" +
               $"V3: ({V3.X}, {V3.Y}, {V3.Z})";
    }
}