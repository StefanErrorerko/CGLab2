using DIContainer;

namespace RayCasting.Console;

public class CompositionRoot : IInitializable
{
    private readonly IContainer _container;

    public CompositionRoot(IContainer container)
    {
        _container = container;
    }

    public void Initialize(IContainer container)
    {
        RegisterDependencies();
    }

    private void RegisterDependencies()
    {
        // Register dependencies here
    }
}