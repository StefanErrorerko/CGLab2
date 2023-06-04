#define Light
#define MT
#define Shadow

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

    //MARK: - Public methods

    // public float[][] Trace()
    // {
    //     var image = new float[ImageHeight][];
    //     for (var i = 0; i < ImageHeight; i++)
    //     {
    //         image[i] = new float[ImageWidth];
    //         for (var j = ImageWidth - 1; j >= 0; j--)
    //         {
    //             var u = (float)j / (ImageWidth - 1);
    //             var v = (float)i / (ImageHeight - 1);
    //             var castRay = Camera.GetRay(u, v);
    //
    //             var closest = GetClosestObject(Scene.Objects, castRay);
    //
    //             if (closest.T >= 0)
    //             {
    //                 var point = castRay.PointAt(closest.T);
    //
    //                 image[i][j] = closest.Object.Reflects(Scene.Light, point);
    //             }
    //         }
    //     }
    //
    //     return image;
    // }

    public float[,] Trace()
    {
        var projectionPlane = Camera.GetProjectionPlane();
        var pixels = new float[projectionPlane.GetLength(0), projectionPlane.GetLength(1)];

#if MT
            Parallel.For(0, projectionPlane.GetLength(0), i =>
            {
                for (int j = 0; j < projectionPlane.GetLength(1); j++)
                {
                    var currentRay = new Ray(Camera.Origin, projectionPlane[i, j]);

                    var nearestIntersection = GetNearestIntersection(Scene.Objects, currentRay);
#if Light
                    if (nearestIntersection.point is not null)
                    {
#if Shadow
                        var normal = nearestIntersection.figure!.Normal(new Vector3((Point3)nearestIntersection.point));
                        var smth = normal.Dot(new Vector3((Point3)nearestIntersection.point, Scene.Light).Normalized());
                        float diffuse = Math.Clamp(smth, 0, 1); //Vector.Dot(normal, Scene.LightSource.ToVector());
                        float shadow = IsInShadow((Point3)nearestIntersection.point + normal * 0.1f) ? 0.5f : 1.0f;

                        pixels[i, j] = diffuse * shadow;
#else
                        var normal = nearestIntersection.figure!.GetNormalAtPoint((Point)nearestIntersection.point);

                        pixels[i, j] = Math.Clamp(Vector.Dot(normal, Scene.LightSource.ToVector()),0 , 1); //Vector.Dot(normal, Scene.LightSource.ToVector());

#endif

                    }
#else

                        pixels[i, j] = nearestIntersection.point is not null ? 1.0f : 0.0f;
                    
#endif
                }
            }); // Parallel.For
#else

        for (int i = 0; i < projectionPlane.GetLength(0); i++)
        {
            for (int j = 0; j < projectionPlane.GetLength(1); j++)
            {
                var currentRay = new Ray(Camera.Origin, projectionPlane[i, j]);

                var nearestIntersection = GetNearestIntersection(Scene.Objects, currentRay);
#if Light
                if (nearestIntersection.point is not null)
                {
                    var normal = nearestIntersection.figure!.Normal(new Vector3((Point3)nearestIntersection.point));

                    pixels[i, j] = normal.Dot(new Vector3(Scene.Light));
                }
#else

                        pixels[i, j] = nearestIntersection.point is not null ? 1.0f : 0.0f;
                    
#endif

            }
        }
#endif
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