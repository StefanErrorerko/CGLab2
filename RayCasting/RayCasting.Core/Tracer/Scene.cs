using RayCasting.Core.Objects;
using RayCasting.Core.Structures;

namespace RayCasting.Core;

public class Scene
{
    //MARK: - Properties
    
    private Vector3 _lightVector;
    public List<IObjectProtocol> Objects = new();
    public Vector3 Light
    {
        get
        {
            return this._lightVector;
        }
        
        set
        {
            this._lightVector = value.Normalized();
        }
    }
    
    //MARK: - Initialization
    
    public Scene(List<IObjectProtocol> objects, Vector3 light)
    {
        this.Objects = objects;
        this._lightVector = light.Normalized();
    }
}
