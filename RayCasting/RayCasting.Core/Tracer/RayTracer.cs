﻿using RayCasting.Core.Objects;
using RayCasting.Core.Structures;

namespace RayCasting.Core.Tracer;

public class RayTracer : IRayTracer
{
    //MARK: - Properties
    private ICameraProtocol Camera { get; }
    private Scene Scene { get; }
    
    //MARK: - Initialization
    public RayTracer(ICameraProtocol camera, Scene scene)
    {
        Camera = camera;
        Scene = scene;
    }

    //MARK: - Public methods

    public float[,] Trace()
    {
        var projectionPlane = Camera.GetProjectionPlane();
        var pixels = new float[projectionPlane.GetLength(0), projectionPlane.GetLength(1)];

            Parallel.For(0, projectionPlane.GetLength(0), i =>
            {
                for (int j = 0; j < projectionPlane.GetLength(1); j++)
                {
                    var currentRay = new Ray(Camera.Origin, projectionPlane[i, j]);
                    var nearestIntersection = GetNearestIntersection(Scene.Objects, currentRay);
                    if (nearestIntersection.point is not null)
                    {
                        var normal = nearestIntersection.figure!.Normal(new Vector3((Point3)nearestIntersection.point));
                        var val = normal.Dot(new Vector3((Point3)nearestIntersection.point, Scene.Light).Normalized());
                        float diffuse = Math.Clamp(val, 0, 1);
                        float shadow = IsInShadow((Point3) nearestIntersection.point + normal * 0.1f) ? 0.5f : 1.0f;

                        pixels[i, j] = diffuse * shadow;
                    }                    
                }
            });
        return pixels;
    }

    //MARK: - Private methods

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