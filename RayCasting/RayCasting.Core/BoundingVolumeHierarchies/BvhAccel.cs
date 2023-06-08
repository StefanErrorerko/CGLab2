using RayCasting.Core.Objects;
using RayCasting.Core.Structures;
using RayCasting.Core.Tracer;

namespace RayCasting.Core.BoundingVolumeHierarchies;

public class BvhAccel
{
    public BvhNode Root { get; }
    public BoundingBox BoundingBox => Root.BoundingBox;

    public BvhAccel(List<IObject> primitives, int maxLeafSize)
    {
        Root = ConstructBvh(primitives, maxLeafSize);
    }

    // private BvhNode ConstructBvh(List<IObject> primitives, int maxLeafSize)
    // {
    //     BvhNode node = new BvhNode();
    //     node.BoundingBox = ComputeBoundingBox(primitives);
    //
    //     if (primitives.Count <= maxLeafSize)
    //     {
    //         // Leaf node: Set the start and end iterators
    //         node.Start = 0;
    //         node.End = primitives.Count - 1;
    //     }
    //     else
    //     {
    //         // Find the axis that provides the most benefit for splitting
    //         int splitAxis = GetSplitAxis(node.BoundingBox);
    //
    //         // Compute the split point along the chosen axis
    //         float splitPoint = ComputeSplitPoint(primitives, splitAxis);
    //
    //         // Divide the primitives into left and right collections
    //         List<IObject> leftPrimitives = new List<IObject>();
    //         List<IObject> rightPrimitives = new List<IObject>();
    //
    //         foreach (IObject primitive in primitives)
    //         {
    //             BoundingBox bbox = primitive.GetBoundingBox();
    //             float centroid = bbox.GetCentroid()[splitAxis];
    //
    //             if (centroid <= splitPoint)
    //                 leftPrimitives.Add(primitive);
    //             else
    //                 rightPrimitives.Add(primitive);
    //         }
    //
    //         // Recursively construct BVH for the left and right collections
    //         node.LeftChild = ConstructBvh(leftPrimitives, maxLeafSize);
    //         node.RightChild = ConstructBvh(rightPrimitives, maxLeafSize);
    //     }
    //
    //     return node;
    // }

    private BvhNode ConstructBvh(List<IObject> primitives, int maxLeafSize)
    {
        BoundingBox centroidBox = new BoundingBox();
        BoundingBox bbox = new BoundingBox();
        var pbb = primitives[0].GetBoundingBox();
        foreach (var bb in primitives.Select(p => p.GetBoundingBox()))
        {
            bbox.Expand(bb);
            var centroid = bb.GetCentroid();
            centroidBox.Expand(centroid);
        }

        var node = new BvhNode(bbox)
        {
            Primitives = new List<IObject>(primitives)
        };

        if (primitives.Count <= maxLeafSize)
        {
            return node;
        }

        var recAxis = CalulateRecAxis(centroidBox);

        Vector3 splitPoint = centroidBox.GetCentroid();
        List<IObject> leftChild = new List<IObject>();
        List<IObject> rightChild = new List<IObject>();

        foreach (IObject p in primitives)
        {
            Vector3 primitiveBbCentroid = p.GetBoundingBox().GetCentroid();

            if (primitiveBbCentroid[recAxis] <= splitPoint[recAxis])
            {
                leftChild.Add(p);
            }
            else
            {
                rightChild.Add(p);
            }
        }

        node.LeftChild = ConstructBvh(leftChild, maxLeafSize);
        node.RightChild = ConstructBvh(rightChild, maxLeafSize);

        return node;
    }

    private static int CalulateRecAxis(BoundingBox centroidBox)
    {
        int recAxis;
        Vector3 boxExtent = centroidBox.Extent;
        double max = Math.Max(boxExtent.X, Math.Max(boxExtent.Y, boxExtent.Z));

        if (Math.Abs(max - boxExtent.X) < 0.00001)
        {
            recAxis = 0;
        }
        else if (Math.Abs(max - boxExtent.Y) < 0.00001)
        {
            recAxis = 1;
        }
        else
        {
            recAxis = 2;
        }

        return recAxis;
    }

    public bool IsIntersects(Ray ray, BvhNode node)
    {
        return Root.Primitives.Any(primitive => primitive.Intersects(ray));
    }
    
    private bool Instersect(Ray ray, BvhNode node)
    {
        double t1;
        if (!node.BoundingBox.Intersect(ray, out _, out t1)) return false;
        if (!(t1 > ray.MinT) || !(t1 < ray.MaxT)) return false;
        if (node.IsLeafNode)
        {
            return node.Primitives.Any(primitive => primitive.Intersects(ray));
        }

        var isLeftChildIntersected = Instersect(ray, node.LeftChild);
        var isRightChildIntersected = Instersect(ray, node.RightChild);

        return isLeftChildIntersected || isRightChildIntersected;
    }
    

    private BoundingBox CalculateBoundingBox(List<IObject> primitives)
    {
        BoundingBox bbox = primitives[0].GetBoundingBox();

        for (int i = 1; i < primitives.Count; i++)
            bbox.Expand(primitives[i].GetBoundingBox());

        return bbox;
    }

    private float CalculateSplitPoint(List<IObject> primitives, int splitAxis)
    {
        float centroidSum = 0.0f;
        foreach (IObject primitive in primitives)
        {
            BoundingBox bbox = primitive.GetBoundingBox();
            centroidSum += bbox.GetCentroid()[splitAxis];
        }

        return centroidSum / primitives.Count;
    }

    // public bool Intersect(Ray ray, out IObject hitObject, out float t)
    // {
    //     hitObject = null;
    //     t = float.MaxValue;
    //
    //     return IntersectBVHNode(_root, ray, ref hitObject, ref t);
    // }
    //
    // private bool IntersectBVHNode(BvhNode? node, Ray ray, ref IObject hitObject, ref float t)
    // {
    //     if (node.BoundingBox.Intersect(ray, out t))
    //     {
    //         if (node.Start != -1 && node.End != -1)
    //         {
    //             // Leaf node: Iterate over the primitives and check for intersection
    //             for (int i = node.Start; i <= node.End; i++)
    //             {
    //                 if (node.Primitives[i].Intersects(ray) && thit < t)
    //                 {
    //                     hitObject = node.Primitives[i];
    //                     t = (float)thit;
    //                 }
    //             }
    //         }
    //         else
    //         {
    //             // Interior node: Recursively check intersection with child nodes
    //             bool hitLeft = IntersectBVHNode(node.LeftChild, ray, ref hitObject, ref t);
    //             bool hitRight = IntersectBVHNode(node.RightChild, ray, ref hitObject, ref t);
    //
    //             return hitLeft || hitRight;
    //         }
    //     }
    //
    //     return false;
    // }

    // public bool HasIntersection(Ray ray)
    // {
    //     return HasIntersection(ray, _root);
    // }
    //
    // private bool HasIntersection(Ray ray, BvhNode? node)
    // {
    //     if (!node.BoundingBox.Intersect(ray, ray.CalculateT()))
    //         return false;
    //
    //     if (node.IsLeafNode())
    //     {
    //         foreach (IObject p in node.Primitives)
    //         {
    //             var intersection = p.GetIntersectionWith(ray);
    //             if (intersection.point != null)
    //                 return true;
    //         }
    //     }
    //     else
    //     {
    //         bool hit1 = HasIntersection(ray, node.LeftChild);
    //         bool hit2 = HasIntersection(ray, node.RightChild);
    //
    //         return hit1 || hit2;
    //     }
    //
    //     return false;
    // }
}