using RayCasting.Core.BoundingVolumeHierarchies;
using RayCasting.Core.Objects;
using RayCasting.Core.Scene;
using RayCasting.Core.Structures;
using System.Drawing;

namespace RayCasting.Core.Tracer;

public class BvhRayTracer : IRayTracer
{
    //MARK: - Properties
    private BvhScene Scene { get; }
    private BvhAccel _bvhAccel;

    public int BvhMaxLeafSize { get; set; } = 10;

    //MARK: - Initialization
    public BvhRayTracer( BvhScene scene)
    {
        Scene = scene;
        _bvhAccel = new BvhAccel(Scene.Objects, BvhMaxLeafSize);
    }

    //MARK: - Public methods
    public Color[,] Trace()
    {
        var projectionPlane = Scene.Camera.GetProjectionPlane();
        var pixels = new Color[projectionPlane.GetLength(0), projectionPlane.GetLength(1)];

        Parallel.For(0, projectionPlane.GetLength(0), i =>
        {
            for (var j = 0; j < projectionPlane.GetLength(1); j++)
            {
                var currentRay = new Ray(Scene.Camera.Origin, projectionPlane[i, j]);
                var (figure, point) = _bvhAccel.GetNearestIntersection(_bvhAccel.Root, currentRay);

                pixels[i, j] = Color.Red;
                
                if (point is null) continue;
                
                var normal = figure!.Normal(new Vector3((Point3)point));
                foreach (var light in Scene.Lights)
                    pixels[i, j] = light.GetPixel((Point3)point, figure, Scene.Objects);
            }
        });
        return pixels;
    }


    private bool IsInShadow(Point3 intersectionPoint)
    {
        //TODO: update for new scene lights
        var oppositeLightVector = new Ray(intersectionPoint, new Vector3().Normalized());

        foreach (var sceneObject in _bvhAccel.Root.Primitives)
        {
            var overlapPoint = sceneObject.GetIntersectionWith(oppositeLightVector).point;
            if (overlapPoint is not null) return true;
        }

        return false;
    }

    private BvhAccel BuildAccel(int maxLeafSize)
    {
        var primitives = new List<IObject>(Scene.Objects);

        return new BvhAccel(primitives, maxLeafSize);
    }
}