using RayCasting.Core.BoundingVolumeHierarchies;
using RayCasting.Core.Objects;
using RayCasting.Core.Structures;

namespace RayCasting.Core.Scene;

public class BvhScene : Tracer.Scene
{
    public BvhNode Bvh { get; set; }
    public BvhScene(List<IObject> objects, Point3 light) : base(objects, light)
    {
        ConstructBVH(objects);
    }

    public BvhScene(Point3 light) : base(light)
    {
    }
    
    private void ConstructBVH(List<IObject> objects)
    {
        var prims = new List<IObject>(objects);

        var tree = new BvhAccel(prims, 4);
        Bvh = tree.Root;
    }
}