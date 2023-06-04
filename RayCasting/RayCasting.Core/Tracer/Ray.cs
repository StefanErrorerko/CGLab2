using RayCasting.Core.Structures;

namespace RayCasting.Core.Tracer;

public struct Ray
{
    //MARK: - Properties

    public Vector3 Origin;
    public Vector3 Direction;

    //MARK: - Initialization

    public Ray(Vector3 origin, Vector3 direction)
    {
        Origin = origin;
        Direction = new Vector3(origin.EndPoint(), direction.EndPoint()).Normalized();
    }

    //MARK: - Public methods

    public Vector3 PointAt(float t = 1)
    {
        return Origin + Direction * t;
    }
}