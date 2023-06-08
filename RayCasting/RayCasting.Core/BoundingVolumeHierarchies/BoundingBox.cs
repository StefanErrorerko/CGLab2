using RayCasting.Core.Structures;
using RayCasting.Core.Tracer;

namespace RayCasting.Core.BoundingVolumeHierarchies;

public struct BoundingBox
{
    public Vector3 Min;
    public Vector3 Max;
    public Vector3 Extent;

    public BoundingBox() :
        this(
            new Vector3(float.MaxValue, float.MaxValue, float.MaxValue),
            new Vector3(float.MinValue, float.MinValue, float.MinValue))
    {
    }

    public BoundingBox(Vector3 min, Vector3 max)
    {
        Min = min;
        Max = max;
        Extent = Max - Min;
    }

    public BoundingBox(Vector3 p) : this(p, p)
    {
    }

    // checked
    public Vector3 GetCentroid()
    {
        return (Min + Max) * 0.5f;
    }

    public void Expand(Point3 point)
    {
        Min.X = Math.Min(Min.X, point.X);
        Min.Y = Math.Min(Min.Y, point.Y);
        Min.Z = Math.Min(Min.Z, point.Z);

        Max.X = Math.Max(Max.X, point.X);
        Max.Y = Math.Max(Max.Y, point.Y);
        Max.Z = Math.Max(Max.Z, point.Z);
    }

    //checked
    public void Expand(BoundingBox otherBox)
    {
        Min.X = Math.Min(Min.X, otherBox.Min.X);
        Min.Y = Math.Min(Min.Y, otherBox.Min.Y);
        Min.Z = Math.Min(Min.Z, otherBox.Min.Z);

        Max.X = Math.Max(Max.X, otherBox.Max.X);
        Max.Y = Math.Max(Max.Y, otherBox.Max.Y);
        Max.Z = Math.Max(Max.Z, otherBox.Max.Z);

        Extent = Max - Min;
    }

    //checked
    public void Expand(Vector3 vector)
    {
        Min.X = Math.Min(Min.X, vector.X);
        Min.Y = Math.Min(Min.Y, vector.Y);
        Min.Z = Math.Min(Min.Z, vector.Z);

        Max.X = Math.Max(Max.X, vector.X);
        Max.Y = Math.Max(Max.Y, vector.Y);
        Max.Z = Math.Max(Max.Z, vector.Z);
    }

    //checked
    private bool IsEmpty => Min.X > Max.X || Min.Y > Max.Y || Min.Z > Max.Z;

    public double SurfaceArea()
    {
        if (IsEmpty)
            return 0;
        return 2 * (Extent.X * Extent.Z + Extent.X * Extent.Y + Extent.Y * Extent.Z);
    }

    public Vector3 GetDimensions()
    {
        var width = Max.X - Min.X;
        var height = Max.Y - Min.Y;
        var depth = Max.Z - Min.Z;

        return new Vector3(width, height, depth);
    }

    // public bool Intersect(Ray r, out double t0, out double t1)
    // {
    //     double tmin = (Min.X - r.Origin.X) / r.Direction.X;
    //     double tmax = (Max.X - r.Origin.X) / r.Direction.X;
    //     if (tmin > tmax)
    //         (tmin, tmax) = (tmax, tmin);
    //
    //     double tymin = (Min.Y - r.Origin.Y) / r.Direction.Y;
    //     double tymax = (Max.Y - r.Origin.Y) / r.Direction.Y;
    //     if (tymin > tymax)
    //         (tymin, tymax) = (tymax, tymin);
    //
    //     double tzmin = (Min.Z - r.Origin.Z) / r.Direction.Z;
    //     double tzmax = (Max.Z - r.Origin.Z) / r.Direction.Z;
    //     if (tzmin > tzmax)
    //         (tzmin, tzmax) = (tzmax, tzmin);
    //
    //     tmin = Math.Max(Math.Max(tmin, tymin), tzmin);
    //     tmax = Math.Min(Math.Min(tmax, tymax), tzmax);
    //
    //     t0 = tmin;
    //     t1 = tmax;
    //
    //     return t0 <= t1;
    // }
    
    public bool IntersectsWithRay(in Ray ray, out float t)
    {
        var tMin = (Min.X - ray.Origin.X) / ray.Direction.X;
        var tMax = (Max.X - ray.Origin.X) / ray.Direction.X;
        t = 0;
        
        if (tMin > tMax)
        {
            (tMin, tMax) = (tMax, tMin);
        }

        var tymin = (Min.Y - ray.Origin.Y) / ray.Direction.Y;
        var tymax = (Max.Y - ray.Origin.Y) / ray.Direction.Y;

        if (tymin > tymax)
        {
            (tymin, tymax) = (tymax, tymin);
        }

        if ((tMin > tymax) || (tymin > tMax))
        {
            t = -1;
            return false;
        }

        if (tymin > tMin)
            tMin = tymin;

        if (tymax < tMax)
            tMax = tymax;

        var tzmin = (Min.Z - ray.Origin.Z) / ray.Direction.Z;
        var tzmax = (Max.Z - ray.Origin.Z) / ray.Direction.Z;

        if (tzmin > tzmax)
        {
            (tzmin, tzmax) = (tzmax, tzmin);
        }

        if ((tMin > tzmax) || (tzmin > tMax))
        {
            t = -1;
            return false;
        }

        if (tzmin > tMin)
            tMin = tzmin;

        if (tzmax < tMax)
            tMax = tzmax;

        t = tMin;

        if (tMax < tMin || tMax < 0)
        {
            t = -1;
            return false;
        }
        return true;
    }
}