using RayCasting.Core.Structures;
using RayCasting.Core.Tracer;

namespace RayCasting.Core.BoundingVolumeHierarchies;

public struct BoundingBox
{
    public Vector3 Min, Max, Extent;

    public BoundingBox() :
        this(new Vector3(float.MinValue, float.MinValue, float.MinValue),
            new Vector3(float.MaxValue, float.MaxValue, float.MaxValue))
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

    public bool Intersect(Ray ray, out float tMin)
    {
        tMin = 0.0f;
        float tMax = float.MaxValue;

        // Calculate the inverse direction components to avoid divisions inside the loop
        float invDirX = 1.0f / ray.Direction.X;
        float invDirY = 1.0f / ray.Direction.Y;
        float invDirZ = 1.0f / ray.Direction.Z;

        // Calculate t-values for each slab of the bounding box
        float tMinX = (Min.X - ray.Origin.X) * invDirX;
        float tMaxX = (Max.X - ray.Origin.X) * invDirX;
        float tMinY = (Min.Y - ray.Origin.Y) * invDirY;
        float tMaxY = (Max.Y - ray.Origin.Y) * invDirY;
        float tMinZ = (Min.Z - ray.Origin.Z) * invDirZ;
        float tMaxZ = (Max.Z - ray.Origin.Z) * invDirZ;

        // Find the largest tMin value
        tMin = Math.Max(Math.Max(Math.Min(tMinX, tMaxX), Math.Min(tMinY, tMaxY)), Math.Min(tMinZ, tMaxZ));

        // Check for valid intersection
        return tMax >= tMin;
    }

    public Vector3 GetCentroid()
    {
        // double centerX = (Min.X + Max.X) * 0.5;
        // double centerY = (Min.Y + Max.Y) * 0.5;
        // double centerZ = (Min.Z + Max.Z) * 0.5;
        //
        // return new Vector3((float)centerX, (float)centerY, (float)centerZ);
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

    public void Expand(Vector3 vector)
    {
        Min.X = Math.Min(Min.X, vector.X);
        Min.Y = Math.Min(Min.Y, vector.Y);
        Min.Z = Math.Min(Min.Z, vector.Z);

        Max.X = Math.Max(Max.X, vector.X);
        Max.Y = Math.Max(Max.Y, vector.Y);
        Max.Z = Math.Max(Max.Z, vector.Z);
    }

    public bool IsEmpty => Min.X > Max.X || Min.Y > Max.Y || Min.Z > Max.Z;

    public double SurfaceArea()
    {
        if (IsEmpty)
            return 0;
        return 2 * (Extent.X * Extent.Z + Extent.X * Extent.Y + Extent.Y * Extent.Z);
    }

    public Vector3 GetDimensions()
    {
        float width = Max.X - Min.X;
        float height = Max.Y - Min.Y;
        float depth = Max.Z - Min.Z;

        return new Vector3(width, height, depth);
    }

    // public bool Intersect(Ray ray, out float t0, out float t1)
    // {
    //     float tmin = float.MinValue;
    //     float tmax = float.MaxValue;
    //
    //     for (int i = 0; i < 3; i++)
    //     {
    //         if (Math.Abs(ray.Direction[i]) < 1e-6)
    //         {
    //             if (ray.Origin[i] < Min[i] || ray.Origin[i] > Max[i])
    //             {
    //                 t0 = tmin;
    //                 t1 = tmax;
    //                 return false;
    //             }
    //         }
    //         else
    //         {
    //             float invDir = 1.0f / ray.Direction[i];
    //             float tNear = (Min[i] - ray.Origin[i]) * invDir;
    //             float tFar = (Max[i] - ray.Origin[i]) * invDir;
    //
    //             if (tNear > tFar)
    //                 Swap(ref tNear, ref tFar);
    //
    //             tmin = Math.Max(tmin, tNear);
    //             tmax = Math.Min(tmax, tFar);
    //
    //             if (tmin > tmax)
    //             {
    //                 t0 = tmin;
    //                 t1 = tmax;
    //                 return false;
    //             }
    //         }
    //     }
    //
    //     t0 = tmin;
    //     t1 = tmax;
    //     return true;
    // }

    public bool Intersect(Ray r, out double t0, out double t1)
    {
        double tmin = (Min.X - r.Origin.X) / r.Direction.X;
        double tmax = (Max.X - r.Origin.X) / r.Direction.X;
        if (tmin > tmax)
            (tmin, tmax) = (tmax, tmin);

        double tymin = (Min.Y - r.Origin.Y) / r.Direction.Y;
        double tymax = (Max.Y - r.Origin.Y) / r.Direction.Y;
        if (tymin > tymax)
            (tymin, tymax) = (tymax, tymin);

        double tzmin = (Min.Z - r.Origin.Z) / r.Direction.Z;
        double tzmax = (Max.Z - r.Origin.Z) / r.Direction.Z;
        if (tzmin > tzmax)
            (tzmin, tzmax) = (tzmax, tzmin);

        tmin = Math.Max(Math.Max(tmin, tymin), tzmin);
        tmax = Math.Min(Math.Min(tmax, tymax), tzmax);

        t0 = tmin;
        t1 = tmax;

        return t0 <= t1;
    }

    // private void Swap(ref float a, ref float b)
    // {
    //     (a, b) = (b, a);
    // }
}