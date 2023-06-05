namespace RayCasting.Core.Structures;

public struct Point3
{
    // Properties
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }


    // Constructor
    public Point3(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public float GetDistanceTo(Point3 rhs)
    {
        var a = Math.Pow(rhs.X - X, 2);
        var b = Math.Pow(rhs.Y - Y, 2);
        var c = Math.Pow(rhs.Z - Z, 2);
        return (float)Math.Sqrt(a + b + c);
    }
    
    // Operators
    public static Point3 operator +(Point3 lhs, Vector3 rhs)
    {
        return new Point3(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z);
    }

    public static Point3 operator -(Point3 lhs, Vector3 rhs)
    {
        return new Point3(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z);
    }

    public float this[int index]
    {
        get
        {
            return index switch
            {
                0 => X,
                1 => Y,
                2 => Z,
                _ => throw new IndexOutOfRangeException("Vector3Indexer index out of range.")
            };
        }
    }
}