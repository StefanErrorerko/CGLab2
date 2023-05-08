using RayCasting.Core.Structures;

namespace RayCasting.Core.Tracer;

public class Camera : ICameraProtocol
{
    private readonly Vector3 _horizontal;
    private readonly Vector3 _lowerLeftCorner;
    private readonly Vector3 _vertical;

    // Initialization
    public Camera(Vector3 origin, Vector3 direction, Vector3 top, float fieldOfView,
        (float width, float height) aspectRatio)
    {
        Origin = origin;
        var direction1 = direction;

        var tetha = fieldOfView * (float)Math.PI / 180;
        var h = Math.Abs(MathF.Tan(tetha));
        var viewPortHeight = 2.0f * h;
        var viewPortWidth = aspectRatio.width / aspectRatio.height * viewPortHeight;

        var w = (Origin - direction1).Normalized();
        var u = top.Cross(w).Normalized();
        var v = w.Cross(u);

        var focusDistance = (Origin - direction1).Length;

        _horizontal = u * (focusDistance * viewPortWidth);
        _vertical = v * (focusDistance * viewPortHeight);
        _lowerLeftCorner = Origin - _horizontal / 2 - _vertical / 2 - w * focusDistance;
    }

    // Properties
    public Vector3 Origin { get; }

    // Public methods
    public Ray GetRay(float u, float v)
    {
        return new Ray(Origin, _lowerLeftCorner + _horizontal * u + _vertical * v - Origin);
    }
}