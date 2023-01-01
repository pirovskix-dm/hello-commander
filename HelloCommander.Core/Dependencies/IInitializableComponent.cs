namespace HelloCommander.Core.Dependencies;

public interface IInitializableComponent<in T>
{
    void Initialize(T parameter);
}

public interface IAsyncInitializableComponent<in T>
{
    Task InitializeAsync(T parameter);
}

public interface IAsyncInitializableComponent<in T1, in T2>
{
    Task InitializeAsync(T1 parameter1, T2 parameter2);
}
