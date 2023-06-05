using RayCasting.Core.Objects;
using RayCasting.Core.Structures;

namespace RayCasting.Core.BoundingVolumeHierarchies;

public class BvhTree
{
    // Recursive function to build the BVH tree
    public BvhNode Build(List<Triangle> triangles, int start, int end)
    {
        // Base case: If there are no triangles, return null
        if (start > end)
            return null;

        // Calculate the bounding box for the current set of triangles
        var bbox = CalculateBoundingBox(triangles, start, end);

        // Create a new node with the bounding box and empty child nodes
        var node = new BvhNode(bbox, null, null, null);

        // If there are only a few triangles, create a leaf node
        if (end - start <= 3)
        {
            // Store the triangle indices in the leaf node
            node.TriangleIndices = new List<int>();
            for (var i = start; i <= end; i++) node.TriangleIndices.Add(i);
        }
        else
        {
            // Choose the longest axis of the bounding box as the splitting axis
            var longestAxis = bbox.Max.X - bbox.Min.X > bbox.Max.Y - bbox.Min.Y
                ? bbox.Max.X - bbox.Min.X > bbox.Max.Z - bbox.Min.Z ? 0 : 2
                : bbox.Max.Y - bbox.Min.Y > bbox.Max.Z - bbox.Min.Z
                    ? 1
                    : 2;

            // Sort the triangles based on the midpoint along the longest axis
            var sortedTriangles = new List<Triangle>(triangles.GetRange(start, end - start + 1));
            sortedTriangles.Sort((t1, t2) =>
            {
                var midpoint1 = GetTriangleMidpoint(t1, longestAxis);
                var midpoint2 = GetTriangleMidpoint(t2, longestAxis);
                return midpoint1.CompareTo(midpoint2);
            });

            // Calculate the midpoint to split the triangles
            var mid = start + (end - start) / 2;

            // Recursively build the left and right child BVH trees
            node.LeftChild = Build(sortedTriangles, start, mid);
            node.RightChild = Build(sortedTriangles, mid + 1, end);
        }

        return node;
    }

    // Calculate the bounding box for a set of triangles
    private BoundingBox CalculateBoundingBox(List<Triangle> triangles, int start, int end)
    {
        var min = new Point3(float.MaxValue, float.MaxValue, float.MaxValue);
        var max = new Point3(float.MinValue, float.MinValue, float.MinValue);

        for (var i = start; i <= end; i++)
        {
            var triangle = triangles[i];
            min.X = Math.Min(min.X, Math.Min(triangle.V1.X, Math.Min(triangle.V2.X, triangle.V3.X)));
            min.Y = Math.Min(min.Y, Math.Min(triangle.V1.Y, Math.Min(triangle.V2.Y, triangle.V3.Y)));
            min.Z = Math.Min(min.Z, Math.Min(triangle.V1.Z, Math.Min(triangle.V2.Z, triangle.V3.Z)));
            max.X = Math.Max(max.X, Math.Max(triangle.V1.X, Math.Max(triangle.V2.X, triangle.V3.X)));
            max.Y = Math.Max(max.Y, Math.Max(triangle.V1.Y, Math.Max(triangle.V2.Y, triangle.V3.Y)));
            max.Z = Math.Max(max.Z, Math.Max(triangle.V1.Z, Math.Max(triangle.V2.Z, triangle.V3.Z)));
        }

        return new BoundingBox(min, max);
    }

    // Calculate the midpoint of a triangle along a given axis
    private float GetTriangleMidpoint(Triangle triangle, int axis)
    {
        var midpoint = 0.0f;

        switch (axis)
        {
            case 0:
                midpoint = (triangle.V1.X + triangle.V2.X + triangle.V3.X) / 3.0f;
                break;
            case 1:
                midpoint = (triangle.V1.Y + triangle.V2.Y + triangle.V3.Y) / 3.0f;
                break;
            case 2:
                midpoint = (triangle.V1.Z + triangle.V2.Z + triangle.V3.Z) / 3.0f;
                break;
        }

        return midpoint;
    }
    
    // bool IntersectBVHNode(BvhNode node, Ray ray, out float tMin, out float tMax)
    // {
    //     tMin = 0.0f;
    //     tMax = float.MaxValue;
    //
    //     // Check if the ray intersects the bounding box of the BVH node
    //     if (!node.BoundingBox.Intersect(ray, out tMin, out tMax))
    //         return false;
    //
    //     // Leaf node: Check for intersection with the triangles in the BVH node
    //     if (node.LeftChild == null && node.RightChild == null)
    //     {
    //         bool hit = false;
    //         foreach (int triangleIndex in node.TriangleIndices)
    //         {
    //             Triangle triangle = node.[triangleIndex];
    //             if (IntersectTriangle(triangle, ray, out float t))
    //             {
    //                 hit = true;
    //                 tMax = t;
    //             }
    //         }
    //         return hit;
    //     }
    //
    //     // Interior node: Recursively check for intersection with the child nodes
    //     bool hitLeft = IntersectBVHNode(node.LeftChild, ray, out float tMinLeft, out float tMaxLeft);
    //     bool hitRight = IntersectBVHNode(node.RightChild, ray, out float tMinRight, out float tMaxRight);
    //
    //     // Update the tMin and tMax values based on the intersection results
    //     tMin = Math.Min(tMinLeft, tMinRight);
    //     tMax = Math.Min(tMaxLeft, tMaxRight);
    //
    //     return hitLeft || hitRight;
    // }
}