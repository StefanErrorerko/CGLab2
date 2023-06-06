using RayCasting.Core.BoundingVolumeHierarchies;
using RayCasting.Core.Objects;
using RayCasting.Core.Scene;
using RayCasting.Core.Structures;

namespace RayCasting.Core.Tracer;

public class BvhRayTracer : IRayTracer
{
    //MARK: - Initialization
    public BvhRayTracer(ICameraProtocol camera, BvhScene scene)
    {
        Camera = camera;
        Scene = scene;
        BuildAccel();
    }

    //MARK: - Properties
    private ICameraProtocol Camera { get; }
    private BvhScene Scene { get; }
    private BvhAccel _bvhAccel;

    //MARK: - Public methods
    public float[,] Trace()
    {
        var projectionPlane = Camera.GetProjectionPlane();
        var pixels = new float[projectionPlane.GetLength(0), projectionPlane.GetLength(1)];

        Parallel.For((long)0, projectionPlane.GetLength(0), i =>
        {
            for (int j = 0; j < projectionPlane.GetLength(1); j++)
            {
                var currentRay = new Ray(Camera.Origin, projectionPlane[i, j]);
                var nearestIntersection = GetNearestIntersection(_bvhAccel.Root, currentRay);
                if (nearestIntersection.point is not null)
                {
                    var normal = nearestIntersection.figure!.Normal(new Vector3((Point3)nearestIntersection.point));
                    var val = normal.Dot(new Vector3((Point3)nearestIntersection.point, Scene.Light).Normalized());
                    float diffuse = Math.Clamp(val, 0, 1);
                    float shadow = IsInShadow((Point3)nearestIntersection.point + normal * 0.1f) ? 0.5f : 1.0f;

                    pixels[i, j] = diffuse * shadow;
                }
            }
        });

        return pixels;
    }

    //MARK: - Private methods
    public (IObject? figure, Point3? point) GetNearestIntersection(BvhNode node, Ray ray)
    {
        if (!node.BoundingBox.Intersect(ray, out double t0, out double t1))
        {
            return (null, null);
        }


        if (node.IsLeafNode())
        {
            return GetClosestIntersection(node.Primitives, ray);
        }
        else
        {
            var leftIntersection = GetNearestIntersection(node.LeftChild, ray);
            var rightIntersection = GetNearestIntersection(node.RightChild, ray);

            if (leftIntersection.point is null)
                return rightIntersection;
            if (rightIntersection.point is null)
                return leftIntersection;

            var leftDistance = ray.CalculateT((Point3)leftIntersection.point);
            var rightDistance = ray.CalculateT((Point3)rightIntersection.point);

            return leftDistance < rightDistance ? leftIntersection : rightIntersection;
        }
    }

    private (IObject? figure, Point3? point) GetClosestIntersection(List<IObject> objects, Ray ray)
    {
        IObject? closestObject = null;
        Point3? closestPoint = null;
        var closestDistance = double.MaxValue;

        foreach (var obj in objects)
        {
            var intersection = obj.GetIntersectionWith(ray);
            var intersectionPoint = intersection.point;

            if (intersectionPoint is null) continue;

            if (intersection.t is not null)
            {
                var distanceToIntersection = intersection.t.Value;

                if (distanceToIntersection < closestDistance)
                {
                    closestDistance = distanceToIntersection;
                    closestPoint = intersectionPoint;
                    closestObject = obj;
                }
            }
        }

        return (closestObject, closestPoint);
    }

    private bool IsInShadow(Point3 intersectionPoint)
    {
        var oppositeLightVector = new Ray(intersectionPoint, new Vector3(Scene.Light).Normalized());

        foreach (var sceneObject in _bvhAccel.Root.Primitives)
        {
            var overlapPoint = sceneObject.GetIntersectionWith(oppositeLightVector).point;
            if (overlapPoint is not null)
            {
                return true;
            }
        }

        return false;
    }

    private void BuildAccel()
    {
        var primitives = new List<IObject>(Scene.Objects);
        _bvhAccel = new BvhAccel(primitives, 100);
    }
}