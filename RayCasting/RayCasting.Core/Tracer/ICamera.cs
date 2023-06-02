using RayCasting.Core.Misc;
using RayCasting.Core.Structures;

namespace RayCasting.Core.Tracer;

public interface ICameraProtocol
{
    public Vector3 Direction { get; set; }
    public float Distance { get; set; }
    public int FieldOfView { get; set; }
    public int HorizontalResolution { get; set; }
    public int VerticalResolution { get; set; }
    public Transverter Transverter { get; set; }
    public Point3 Origin { get; set; }
    public Point3[,] GetProjectionPlane();
}