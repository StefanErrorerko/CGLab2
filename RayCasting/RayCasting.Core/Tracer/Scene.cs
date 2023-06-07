using RayCasting.Core.Lights;
using RayCasting.Core.Objects;
using RayCasting.Core.Structures;

namespace RayCasting.Core.Tracer;

public class Scene
{
    //MARK: - Properties

    public List<IObject> Objects;
    public List<Light> Lights { get; set; }

    //MARK: - Initialization

    public Scene(List<IObject> objects, List<Light> lights)
    {
        Objects = new List<IObject>(objects);
        Lights = lights;
    }

    public Scene(List<Light> lights)
    {
        Objects = new List<IObject>();
        Lights = lights;
    }
}