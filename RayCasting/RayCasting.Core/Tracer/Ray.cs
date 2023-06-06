using RayCasting.Core.Structures;

namespace RayCasting.Core.Tracer;

public struct Ray
{
    //MARK: - Properties

    public Point3 Origin;
    public Vector3 Direction;
    public int Depth;
    public double MinT; 
    public double MaxT; 
    public Vector3 InvD;
    public int[] Sign; 

    //MARK: - Initialization

    public Ray(Point3 origin, Point3 direction)
    {
        Origin = origin;
        Direction = new Vector3(origin, direction).Normalized();
        Depth = 0;
        MinT = 0;
        MaxT = 0;
        Sign = new int[] { };
        InvD = default;
    }

    public Ray(Point3 origin, Vector3 direction)
    {
        Origin = origin;
        Direction = direction;
        Depth = 0;
        MinT = 0;
        MaxT = 0;
        Sign = new int[] { };
        InvD = default;
    }

    public Ray(Vector3 origin, Vector3 direction, int depth = 0)
    {
        Origin = new Point3(origin.X, origin.Y, origin.Z);
        Direction = direction;
        MinT = 0.0;
        MaxT = double.PositiveInfinity;
        Depth = depth;
        InvD = new Vector3(1 / direction.X, 1 / direction.Y, 1 / direction.Z);
        Sign = new int[3] { (InvD.X < 0) ? 1 : 0, (InvD.Y < 0) ? 1 : 0, (InvD.Z < 0) ? 1 : 0 };
    }

    public Ray(Vector3 origin, Vector3 direction, double maxT, int depth = 0)
    {
        Origin = new Point3(origin.X, origin.Y, origin.Z);
        Direction = direction;
        MinT = 0.0;
        MaxT = maxT;
        Depth = depth;
        InvD = new Vector3(1 / direction.X, 1 / direction.Y, 1 / direction.Z);
        Sign = new[] { (InvD.X < 0) ? 1 : 0, (InvD.Y < 0) ? 1 : 0, (InvD.Z < 0) ? 1 : 0 };
    }

    //MARK: - Public methods

    public Point3 PointAt(float t = 1)
    {
        return new Point3(
            Origin.X + Direction.X * t,
            Origin.Y + Direction.Y * t,
            Origin.Z + Direction.Z * t);
    }

    public float CalculateT(Point3 intersectionPoint)
    {
        Vector3 vectorOnIntersection = new Vector3(intersectionPoint);
        Vector3 origin = new Vector3(Origin);

        float t = (vectorOnIntersection - origin).Dot(Direction);

        return t;
    }
}