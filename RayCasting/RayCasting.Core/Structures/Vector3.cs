namespace RayCasting.Core.Structures;

public struct Vector3
{
    // Properties
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }

    public float Length => (float)Math.Sqrt(X * X + Y * Y + Z * Z);

    public float LengthSquared => X * X + Y * Y + Z * Z;

    // Constructor
    public Vector3(float x, float y, float z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public Vector3(Point3 point) : this(new Point3(0, 0, 0), point)
    {
    }
    public Vector3(Point3 start, Point3 end)
    {
        X = end.X - start.X;
        Y = end.Y - start.Y;
        Z = end.Z - start.Z;
    }

    // Public methods
    public Vector3 Normalized()
    {
        var len = Length;
        return new Vector3(X / len, Y / len, Z / len);
    }

    public float Dot(Vector3 other)
    {
        return X * other.X + Y * other.Y + Z * other.Z;
    }

    public Point3 EndPoint()
    {
        return new Point3(X, Y, Z);
    }

    public Vector3 Cross(Vector3 other)
    {
        var x = Y * other.Z - Z * other.Y;
        var y = Z * other.X - X * other.Z;
        var z = X * other.Y - Y * other.X;
        return new Vector3(x, y, z);
    }

    public static Vector3 Min(Vector3 a, Vector3 b) => new Vector3(Math.Min(a.X, b.X), Math.Min(a.Y, b.Y), Math.Min(a.Z, b.Z));
    public static Vector3 Max(Vector3 a, Vector3 b) => new Vector3(Math.Max(a.X, b.X), Math.Max(a.Y, b.Y), Math.Max(a.Z, b.Z));

    // Operators
    public static Vector3 operator +(Vector3 lhs, Vector3 rhs)
    {
        return new Vector3(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z);
    }

    public static Vector3 operator -(Vector3 lhs, Vector3 rhs)
    {
        return new Vector3(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z);
    }

    public static Vector3 operator *(Vector3 lhs, float rhs)
    {
        return new Vector3(lhs.X * rhs, lhs.Y * rhs, lhs.Z * rhs);
    }

    public static Vector3 operator /(Vector3 lhs, float rhs)
    {
        return new Vector3(lhs.X / rhs, lhs.Y / rhs, lhs.Z / rhs);
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
    
    public float MagnitudeSquared()
    {
        return X * X + Y * Y + Z * Z;
    }

}