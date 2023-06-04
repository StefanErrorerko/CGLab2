using RayCasting.Core.Structures;
using System.Drawing;

namespace RayCasting.Core.Tracer;

public struct Ray
{
    //MARK: - Properties

    public Point3 Origin;
    public Vector3 Direction;

    //MARK: - Initialization

    public Ray(Point3 origin, Point3 direction)
    {
        Origin = origin;
        Direction = new Vector3(origin, direction).Normalized();
    }

    public Ray(Point3 origin, Vector3 direction)
    {
        Origin = origin;
        Direction = direction;
    }

    //MARK: - Public methods

    public Point3 PointAt(float t = 1)
    {
        return new Point3(
               Origin.X + Direction.X * t,
               Origin.Y + Direction.Y * t,
               Origin.Z + Direction.Z * t);
    }
}