using RayCasting.Core.Objects;
using RayCasting.Core.Structures;

namespace RayCasting.Core.Tracer;

public class Scene
{
    //MARK: - Properties

    public List<IObject> Objects;
    public Point3 Light { get; set; }

    //MARK: - Initialization

    public Scene(List<IObject> objects, Point3 light)
    {
        Objects = new List<IObject>(objects);
        Light = light;
    }

    public Scene(Point3 light)
    {
        Objects = new List<IObject>();
        Light = light;
    }
}