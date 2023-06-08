using RayCasting.Core.BoundingVolumeHierarchies;
using RayCasting.Core.Objects;
using RayCasting.Core.Scene;
using RayCasting.Core.Structures;
using System.Drawing;

namespace RayCasting.Core.Tracer;

public class BvhRayTracer : IRayTracer
{
    //MARK: - Properties
    private ICameraProtocol Camera { get; }
    private BvhScene Scene { get; }
    private BvhAccel _bvhAccel;

    public int BvhMaxLeafSize { get; set; } = 100;

    //MARK: - Initialization
    public BvhRayTracer(ICameraProtocol camera, BvhScene scene)
    {
        Camera = camera;
        Scene = scene;
        _bvhAccel = new BvhAccel(Scene.Objects, BvhMaxLeafSize);
    }

    //MARK: - Public methods
    public Color[,] Trace()
    {
        var projectionPlane = Camera.GetProjectionPlane();
        var pixels = new Color[projectionPlane.GetLength(0), projectionPlane.GetLength(1)];

        Parallel.For(0, projectionPlane.GetLength(0), i =>
        {
            for (var j = 0; j < projectionPlane.GetLength(1); j++)
            {
                var currentRay = new Ray(Camera.Origin, projectionPlane[i, j]);
                var (figure, point) = GetNearestIntersection(_bvhAccel.Root, currentRay);

                pixels[i, j] = Color.Red;
                if (point is not null)
                {
                    var normal = figure!.Normal(new Vector3((Point3)point));
                    foreach (var light in Scene.Lights)
                        pixels[i, j] = light.GetPixel((Point3)point, figure, Scene.Objects);
                }
            }
        });
        return pixels;
    }

    // public Color[,] Trace()
    // {
    //     var projectionPlane = Camera.GetProjectionPlane();
    //     var pixels = new Color[projectionPlane.GetLength(0), projectionPlane.GetLength(1)];
    //
    //     Parallel.For((long)0, projectionPlane.GetLength(0), i =>
    //     {
    //         for (int j = 0; j < projectionPlane.GetLength(1); j++)
    //         {
    //             var currentRay = new Ray(Camera.Origin, projectionPlane[i, j]);
    //             var nearestIntersection = GetNearestIntersection(_bvhAccel.Root, currentRay);
    //             if (nearestIntersection.point is not null)
    //             {
    //                 var normal = nearestIntersection.figure!.Normal(new Vector3((Point3)nearestIntersection.point));
    //                 var val = normal.Dot(new Vector3((Point3)nearestIntersection.point, Scene.Light).Normalized());
    //                 float diffuse = Math.Clamp(val, 0, 1);
    //                 float shadow = IsInShadow((Point3)nearestIntersection.point + normal * 0.1f) ? 0.5f : 1.0f;
    //
    //                 pixels[i, j] = diffuse * shadow;
    //             }
    //         }
    //     });
    //
    //     return pixels;
    // }


    //MARK: - Private methods
    private (IObject? figure, Point3? point) GetNearestIntersection(BvhNode node, Ray ray)
    {
        if (!node.BoundingBox.Intersect(ray, out _, out _)) return (null, null);

        if (node.IsLeafNode) return GetClosestIntersection(node.Primitives, ray);

        var leftIntersection = GetNearestIntersection(node.LeftChild, ray);
        var rightIntersection = GetNearestIntersection(node.RightChild, ray);

        if (leftIntersection.point is null) return rightIntersection;
        if (rightIntersection.point is null) return leftIntersection;

        var leftDistance = ray.CalculateT(leftIntersection.point.Value);
        var rightDistance = ray.CalculateT(rightIntersection.point.Value);

        return leftDistance < rightDistance ? leftIntersection : rightIntersection;
    }

    private (IObject? figure, Point3? point) GetClosestIntersection(List<IObject> objects, Ray ray)
    {
        IObject? closestObject = null;
        Point3? closestPoint = null;
        var closestDistance = double.MaxValue;

        foreach (var obj in objects)
        {
            var intersection = obj.GetIntersectionWith(ray);

            if (intersection.point is null || intersection.t is null) continue;

            var intersectionPoint = intersection.point.Value;
            var distanceToIntersection = intersection.t.Value;

            if (!(distanceToIntersection < closestDistance)) continue;
            closestDistance = distanceToIntersection;
            closestPoint = intersectionPoint;
            closestObject = obj;
        }

        return (closestObject, closestPoint);
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