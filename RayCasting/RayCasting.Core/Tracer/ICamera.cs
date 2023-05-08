using RayCasting.Core.Structures;

namespace RayCasting.Core.Tracer;

public interface ICameraProtocol
{
    Vector3 Origin { get; }
    Ray GetRay(float u, float v);
}