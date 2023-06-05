using RayCasting.Core.Objects;
using RayCasting.Core.Structures;
using RayCasting.Core.Tracer;

namespace RayCasting.Core.BoundingVolumeHierarchies;

public class BvhAccel
{
    private BvhNode _root;

    public BvhAccel(List<IObject> primitives, int maxLeafSize)
    {
        _root = ConstructBvh(primitives, maxLeafSize);
    }

    private BvhNode ConstructBvh(List<IObject> primitives, int maxLeafSize)
    {
        BvhNode node = new BvhNode();
        node.BoundingBox = ComputeBoundingBox(primitives);

        if (primitives.Count <= maxLeafSize)
        {
            // Leaf node: Set the start and end iterators
            node.Start = 0;
            node.End = primitives.Count - 1;
        }
        else
        {
            // Find the axis that provides the most benefit for splitting
            int splitAxis = GetSplitAxis(node.BoundingBox);

            // Compute the split point along the chosen axis
            float splitPoint = ComputeSplitPoint(primitives, splitAxis);

            // Divide the primitives into left and right collections
            List<IObject> leftPrimitives = new List<IObject>();
            List<IObject> rightPrimitives = new List<IObject>();

            foreach (IObject primitive in primitives)
            {
                BoundingBox bbox = primitive.GetBoundingBox();
                float centroid = bbox.GetCentroid()[splitAxis];

                if (centroid <= splitPoint)
                    leftPrimitives.Add(primitive);
                else
                    rightPrimitives.Add(primitive);
            }

            // Recursively construct BVH for the left and right collections
            node.LeftChild = ConstructBvh(leftPrimitives, maxLeafSize);
            node.RightChild = ConstructBvh(rightPrimitives, maxLeafSize);
        }

        return node;
    }

    private BoundingBox ComputeBoundingBox(List<IObject> primitives)
    {
        BoundingBox bbox = primitives[0].GetBoundingBox();

        for (int i = 1; i < primitives.Count; i++)
            bbox.Expand(primitives[i].GetBoundingBox());

        return bbox;
    }

    private int GetSplitAxis(BoundingBox bbox)
    {
        Vector3 dimensions = bbox.GetDimensions();
        int splitAxis = 0;
        double maxDimension = dimensions.X;

        if (dimensions.Y > maxDimension)
        {
            splitAxis = 1;
            maxDimension = dimensions.Y;
        }

        if (dimensions.Z > maxDimension)
            splitAxis = 2;

        return splitAxis;
    }

    private float ComputeSplitPoint(List<IObject> primitives, int splitAxis)
    {
        float centroidSum = 0.0f;
        foreach (IObject primitive in primitives)
        {
            BoundingBox bbox = primitive.GetBoundingBox();
            centroidSum += bbox.GetCentroid()[splitAxis];
        }

        return centroidSum / primitives.Count;
    }

    public bool Intersect(Ray ray, out IObject hitObject, out float t)
    {
        hitObject = null;
        t = float.MaxValue;

        return IntersectBVHNode(_root, ray, ref hitObject, ref t);
    }

    private bool IntersectBVHNode(BvhNode node, Ray ray, ref IObject hitObject, ref float t)
    {
        if (node.BoundingBox.Intersect(ray, out t))
        {
            if (node.Start != -1 && node.End != -1)
            {
                // Leaf node: Iterate over the primitives and check for intersection
                for (int i = node.Start; i <= node.End; i++)
                {
                    if (node.Primitives[i].Intersects(ray, out double thit) && thit < t)
                    {
                        hitObject = node.Primitives[i];
                        t = (float)thit;
                    }
                }
            }
            else
            {
                // Interior node: Recursively check intersection with child nodes
                bool hitLeft = IntersectBVHNode(node.LeftChild, ray, ref hitObject, ref t);
                bool hitRight = IntersectBVHNode(node.RightChild, ray, ref hitObject, ref t);

                return hitLeft || hitRight;
            }
        }

        return false;
    }
}