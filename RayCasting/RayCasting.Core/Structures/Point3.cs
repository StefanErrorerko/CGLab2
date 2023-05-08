namespace RayCasting.Core.Structures;

public struct Point3
{
    // Properties
    public float X { get; }
    public float Y { get; }
    public float Z { get; }

    // Constructor
    public Point3(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    // Operators
    public static Point3 operator +(Point3 lhs, Vector3 rhs)
    {
        return new Point3(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z);
    }

    public static Vector3 operator -(Point3 lhs, Point3 rhs)
    {
        return new Vector3(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z);
    }
}