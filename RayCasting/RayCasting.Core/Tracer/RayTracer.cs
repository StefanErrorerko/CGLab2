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
                for (int j = 0; j < projectionPlane.GetLength(1); j++)
                {
                    var currentRay = new Ray(Camera.Origin, projectionPlane[i, j]);
                    var nearestIntersection = GetNearestIntersection(Scene.Objects, currentRay);
                    pixels[i, j] = Color.Black;
                    if (nearestIntersection.point is not null)
                    {
                        var normal = nearestIntersection.figure!.Normal(new Vector3((Point3)nearestIntersection.point));
                        var val = normal.Dot(new Vector3((Point3)nearestIntersection.point, Scene.Light).Normalized());
                        //float diffuse = Math.Clamp(val, 0, 1); //Vector.Dot(normal, Scene.LightSource.ToVector());
                        //float shadow = IsInShadow((Point3) nearestIntersection.point + normal * 0.1f) ? 0.5f : 1.0f;
                        foreach (var light in Lights)
                            pixels[i, j] = light.GetPixel(nearestIntersection.point, nearestIntersection.figure, Scene.Objects); //diffuse * shadow;
                    }                    
                }
            });
        return pixels;
    }

    //MARK: - Private methods

    // private (IObject? Object, float? T) GetClosestObject(IList<IObject> objects, Ray ray)
    // {
    //     IObject closestObject = null;
    //     float? closestT = null;
    //
    //     foreach (var obj in objects)
    //     {
    //         var t = obj.Intersects(ray);
    //         if (t.HasValue && (closestT == null || t < closestT))
    //         {
    //             closestObject = obj;
    //             closestT = t;
    //         }
    //     }
    //
    //     return (Object: closestObject, T: closestT);
    // }

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
        var oppositeLightVector =
            new Ray(intersectionPoint, new Vector3(Scene.Light).Normalized());

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