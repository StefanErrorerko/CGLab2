using RayCasting.Core.Objects;

namespace RayCasting.Core.BoundingVolumeHierarchies;

public class BvhNode
{
    public BoundingBox BoundingBox;
    public BvhNode LeftChild;
    public List<IObject> Primitives;
    public BvhNode RightChild;
    public List<int> TriangleIndices;

    public BvhNode(BoundingBox boundingBox, BvhNode leftChild, BvhNode rightChild, List<int> triangleIndices)
    {
        BoundingBox = boundingBox;
        LeftChild = leftChild;
        RightChild = rightChild;
        TriangleIndices = triangleIndices;
    }

    public BvhNode()
    {
    }

    public int Start { get; set; } = -1;
    public int End { get; set; } = -1;

    public bool IsLeafNode() => LeftChild == null && RightChild == null;
}