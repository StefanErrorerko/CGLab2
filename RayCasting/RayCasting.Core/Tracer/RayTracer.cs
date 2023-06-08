using RayCasting.Core.Lights;
using RayCasting.Core.Objects;
using RayCasting.Core.Structures;
using System.Drawing;

namespace RayCasting.Core.Tracer;

public class RayTracer : IRayTracer
{
    //MARK: - Initialization
    public RayTracer(ICameraProtocol camera, Scene scene)
    {
        Camera = camera;
        Scene = scene;
    }
    //MARK: - Properties

    private ICameraProtocol Camera { get; }
    private Scene Scene { get; }
    private List<Light> Lights { get; }

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
                var (figure, point) = GetNearestIntersection(Scene.Objects, currentRay);
                
                pixels[i, j] = Color.Black;
                if (point is not null)
                {
                    var normal = figure!.Normal(new Vector3((Point3)point));
                    foreach (var light in Lights)
                        pixels[i, j] = light.GetPixel((Point3)point, figure, Scene.Objects);
                }
            }
        });
        return pixels;
    }

    public (IObject? figure, Point3? point) GetNearestIntersection(List<IObject> objects, Ray ray)
    {
        IObject? closestObject = null;
        Point3? closestPoint = null;
        var closestDistance = float.MaxValue;
        foreach (var intersectableObject in objects)
        {
            var intersection = intersectableObject.GetIntersectionWith(ray);
            var intersectionPoint = intersection.point;

            if (intersectionPoint is null) continue;

            if (intersection.t != null)
            {
                var disatanceToIntersection = intersection.t.Value;

                if (disatanceToIntersection < closestDistance)
                {
                    closestDistance = disatanceToIntersection;
                    closestPoint = intersectionPoint;
                    closestObject = intersectableObject;
                }
            }
        }

        return (closestObject, closestPoint);
    }

    private bool IsInShadow(Point3 intersectionPoint)
    {
        //TODO: update for new scene lights
        var oppositeLightVector =
            new Ray(intersectionPoint, new Vector3().Normalized());

        var isOnShadow = false;
        foreach (var sceneObject in Scene.Objects)
        {
            var overlapPoint = sceneObject.GetIntersectionWith(oppositeLightVector).point;

            if (overlapPoint is not null)
            {
                isOnShadow = true;
                break;
            }
        }

        return isOnShadow;
    }
}