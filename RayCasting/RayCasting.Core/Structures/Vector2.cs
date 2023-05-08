namespace RayCasting.Core.Structures;

public struct Vector2
{
    // Properties
    public float X { get; }
    public float Y { get; }

    public float Length => (float)Math.Sqrt(X * X + Y * Y);

    public float LengthSquared => X * X + Y * Y;

    // Constructor
    public Vector2(float x, float y)
    {
        X = x;
        Y = y;
    }

    // Public methods
    public Vector2 Normalized()
    {
        var len = Length;
        return new Vector2(X / len, Y / len);
    }

    public float Dot(Vector2 other)
    {
        return X * other.X + Y * other.Y;
    }

    // Operators
    public static Vector2 operator +(Vector2 lhs, Vector2 rhs)
    {
        return new Vector2(lhs.X + rhs.X, lhs.Y + rhs.Y);
    }

    public static Vector2 operator -(Vector2 lhs, Vector2 rhs)
    {
        return new Vector2(lhs.X - rhs.X, lhs.Y - rhs.Y);
    }

    public static Vector2 operator *(Vector2 lhs, float rhs)
    {
        return new Vector2(lhs.X * rhs, lhs.Y * rhs);
    }

    public static Vector2 operator /(Vector2 lhs, float rhs)
    {
        return new Vector2(lhs.X / rhs, lhs.Y / rhs);
    }
}