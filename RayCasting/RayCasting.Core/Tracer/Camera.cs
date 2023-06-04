using RayCasting.Core.Misc;
using RayCasting.Core.Structures;

namespace RayCasting.Core.Tracer;

public class Camera : ICameraProtocol
{
    public Camera(Point3 origin, Vector3 direction, float distance, int fieldOfView, Transverter? transverter)
    {
        Origin = origin;
        Direction = direction;
        FieldOfView = fieldOfView;
        Distance = distance;
        Transverter = transverter ?? new Transverter();
    }

    public Vector3 Direction { get; set; }
    public float Distance { get; set; }
    public int FieldOfView { get; set; }
    public int HorizontalResolution { get; set; }
    public int VerticalResolution { get; set; }
    public Transverter Transverter { get; set; }
    public Point3 Origin { get; set; }

    public Point3[,] GetProjectionPlane()
    {
        var rightAnchor = new Vector3(0, 0, 1).Cross(Direction).Normalized();
        var topAnchor = Direction.Cross(rightAnchor).Normalized();

        var projectionPlane = new Point3[HorizontalResolution, VerticalResolution];

        float alpha = FieldOfView / 2;
        var leftOffset = (float)Math.Tan(Math.PI / 180 * alpha) * Distance;
        var bottomOffset = leftOffset * (HorizontalResolution / (float)VerticalResolution);

        var horizontalDistanceBetweenPixels = leftOffset / HorizontalResolution * 2;
        var verticalDistanceBetweenPixels = bottomOffset / VerticalResolution * 2;

        var localOrigin = Origin;

        for (var x = 0; x < HorizontalResolution; x++)
        {
            for (var y = 0; y < VerticalResolution; y++)
            {
                projectionPlane[x, y] = localOrigin + Direction * Distance +
                                        rightAnchor * (x * horizontalDistanceBetweenPixels) +
                                        topAnchor * (y * verticalDistanceBetweenPixels);
                projectionPlane[x, y] = Transverter.ApplyTransformation(projectionPlane[x, y]);
            }
        }

        Origin = Transverter.ApplyTransformation(Origin);

        return projectionPlane;
    }
}
