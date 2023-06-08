using RayCasting.Core.Lights;
using RayCasting.Core.Objects;

namespace RayCasting.Core.Tracer;

public class Scene
{
    //MARK: - Properties

    public List<IObject> Objects;
    public List<Light> Lights { get; set; }

    public ICameraProtocol Camera { get; }
    //MARK: - Initialization

    public Scene(List<IObject> objects, List<Light> lights, ICameraProtocol camera)
    {
        Objects = new List<IObject>(objects);
        Lights = lights;
        Camera = camera;
    }

    public Scene(List<Light> lights, ICameraProtocol camera)
    {
        Objects = new List<IObject>();
        Lights = lights;
        Camera = camera;
    }
}