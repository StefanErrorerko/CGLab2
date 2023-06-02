using RayCasting.Core.Objects;
using RayCasting.Core.Structures;

namespace RayCasting.Core.Tracer;

public class Scene
{
    //MARK: - Properties

    public List<IObject> Objects;
    public Vector3 Light { get; set; }

    //MARK: - Initialization

    public Scene(List<IObject> objects, Vector3 light)
    {
        Objects = new List<IObject>(objects);
        Light = light.Normalized();
    }
}