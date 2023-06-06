namespace DIContainer;

public interface IContainer
{
    void Register<TInterface, TImplementation>(Lifetime lifetime = Lifetime.Transient) where TImplementation : TInterface;
    void Register<TInterface>(Func<TInterface> factory, Lifetime lifetime = Lifetime.Transient);
    TInterface Resolve<TInterface>();
}