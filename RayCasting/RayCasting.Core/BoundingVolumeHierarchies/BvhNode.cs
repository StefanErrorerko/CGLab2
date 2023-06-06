using RayCasting.Core.Objects;

namespace RayCasting.Core.BoundingVolumeHierarchies;

public class BvhNode
{
    public BoundingBox BoundingBox;
    public BvhNode LeftChild;
    public List<IObject> Primitives;
    public BvhNode RightChild;
    // public List<int> TriangleIndices;

    public BvhNode(BoundingBox boundingBox, BvhNode leftChild=null, BvhNode rightChild=null, List<IObject> primitives=null)
    {
        BoundingBox = boundingBox;
        LeftChild = leftChild;
        RightChild = rightChild;
        Primitives = primitives;
    }

    public BvhNode()
    {
    }

    public int Start { get; set; } = -1;
    public int End { get; set; } = -1;

    public bool IsLeafNode() => LeftChild == null && RightChild == null;
}