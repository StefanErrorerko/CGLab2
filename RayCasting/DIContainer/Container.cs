namespace DIContainer;

public class Container : IContainer
{
    private readonly Dictionary<Type, Func<object>> _registrations;
    private readonly Dictionary<Type, object> _singletons;

    public Container()
    {
        _registrations = new Dictionary<Type, Func<object>>();
        _singletons = new Dictionary<Type, object>();
    }

    public void Register<TInterface, TImplementation>(Lifetime lifetime = Lifetime.Transient)
        where TImplementation : TInterface
    {
        _registrations[typeof(TInterface)] = () => Resolve(typeof(TImplementation), lifetime);
    }

    public void Register<TInterface>(Func<TInterface> factory, Lifetime lifetime = Lifetime.Transient)
    {
        _registrations[typeof(TInterface)] = () => Resolve(factory, lifetime);
    }

    public TInterface Resolve<TInterface>()
    {
        return (TInterface)Resolve(typeof(TInterface), Lifetime.Transient);
    }

    private object Resolve(Type type, Lifetime lifetime)
    {
        if (lifetime == Lifetime.Singleton && _singletons.TryGetValue(type, out var resolve))
        {
            return resolve;
        }

        if (!_registrations.TryGetValue(type, out var factory))
        {
            throw new InvalidOperationException($"No registration for type {type}.");
        }

        var instance = factory();

        if (lifetime == Lifetime.Singleton)
        {
            _singletons[type] = instance;
        }

        if (instance is IInitializable initializable)
        {
            initializable.Initialize(this);
        }

        return instance;
    }

    private object Resolve<TInterface>(Func<TInterface> factory, Lifetime lifetime)
    {
        if (lifetime == Lifetime.Singleton && _singletons.ContainsKey(typeof(TInterface)))
        {
            return _singletons[typeof(TInterface)];
        }

        var instance = factory();

        if (lifetime == Lifetime.Singleton)
        {
            _singletons[typeof(TInterface)] = instance;
        }

        if (instance is IInitializable initializable)
        {
            initializable.Initialize(this);
        }

        return instance;
    }
}