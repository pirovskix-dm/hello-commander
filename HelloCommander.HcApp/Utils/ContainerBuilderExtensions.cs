using Autofac;
using HelloCommander.Core.Dependencies;

namespace HelloCommander.HcApp.Utils;

public static class ContainerBuilderExtensions
{
    public static void RegisterSingleton<TImplementer, TTypedService>(this ContainerBuilder containerBuilder)
        where TImplementer : TTypedService
        where TTypedService : notnull
    {
        containerBuilder.RegisterType<TImplementer>().As<TTypedService>().SingleInstance();
    }

    public static void RegisterSingleton<TImplementer>(this ContainerBuilder containerBuilder)
        where TImplementer : notnull
    {
        containerBuilder.RegisterType<TImplementer>().AsSelf().SingleInstance();
    }

    public static void RegisterSingleton<TImplementer>(this ContainerBuilder containerBuilder, Func<IComponentContext, TImplementer> @delegate)
        where TImplementer : notnull
    {
        containerBuilder.Register<TImplementer>(@delegate).SingleInstance();
    }

    public static void Register<TImplementer, TTypedService>(this ContainerBuilder containerBuilder)
        where TImplementer : TTypedService
        where TTypedService : notnull
    {
        containerBuilder.RegisterType<TImplementer>().As<TTypedService>().ExternallyOwned();
    }

    public static void RegisterTransient<TImplementer, TTypedService>(this ContainerBuilder containerBuilder)
        where TImplementer : TTypedService
        where TTypedService : notnull
    {
        containerBuilder.RegisterType<TImplementer>().As<TTypedService>().InstancePerDependency();
    }

    public static void RegisterTransient<TImplementer>(this ContainerBuilder containerBuilder)
        where TImplementer : notnull
    {
        containerBuilder.RegisterType<TImplementer>().AsSelf().InstancePerDependency();
    }

    public static void Register<TImplementer>(this ContainerBuilder containerBuilder)
        where TImplementer : notnull
    {
        containerBuilder.RegisterType<TImplementer>().AsSelf().ExternallyOwned();
    }

    public static void RegisterFuncFactory<TImplementer, TTypedService>(this ContainerBuilder containerBuilder)
        where TImplementer : TTypedService
        where TTypedService : notnull
    {
        containerBuilder.RegisterType<TImplementer>().As<TTypedService>().InstancePerDependency();
        containerBuilder.Register<Func<TTypedService>>(delegate(IComponentContext context)
        {
            var cc = context.Resolve<IComponentContext>();
            return cc.Resolve<TTypedService>;
        });
    }

    public static void RegisterFuncFactory<TImplementer>(this ContainerBuilder containerBuilder)
        where TImplementer : notnull
    {
        containerBuilder.RegisterType<TImplementer>().AsSelf().InstancePerDependency();
        containerBuilder.Register<Func<TImplementer>>(delegate(IComponentContext context)
        {
            var cc = context.Resolve<IComponentContext>();
            return cc.Resolve<TImplementer>;
        });
    }

    public static void RegisterInitializableFuncFactory<TImplementer, TParameter>(this ContainerBuilder containerBuilder)
        where TImplementer : IInitializableComponent<TParameter>
    {
        containerBuilder.RegisterType<TImplementer>().AsSelf().InstancePerDependency();
        containerBuilder.Register<Func<TParameter, TImplementer>>(delegate(IComponentContext context)
        {
            var cc = context.Resolve<IComponentContext>();
            return parameter =>
            {
                var resolved = cc.Resolve<TImplementer>();
                resolved.Initialize(parameter);
                return resolved;
            };
        });
    }

    public static void RegisterAsyncInitializableFuncFactory<TImplementer, TParameter>(this ContainerBuilder containerBuilder)
        where TImplementer : IAsyncInitializableComponent<TParameter>
    {
        containerBuilder.RegisterType<TImplementer>().AsSelf().InstancePerDependency();
        containerBuilder.Register<Func<TParameter, Task<TImplementer>>>(delegate(IComponentContext context)
        {
            var cc = context.Resolve<IComponentContext>();
            return async parameter =>
            {
                var resolved = cc.Resolve<TImplementer>();
                await resolved.InitializeAsync(parameter);
                return resolved;
            };
        });
    }

    public static void RegisterAsyncInitializableFuncFactory<TImplementer, TParameter1, TParameter2>(this ContainerBuilder containerBuilder)
        where TImplementer : IAsyncInitializableComponent<TParameter1, TParameter2>
    {
        containerBuilder.RegisterType<TImplementer>().AsSelf().InstancePerDependency();
        containerBuilder.Register<Func<TParameter1, TParameter2, Task<TImplementer>>>(delegate(IComponentContext context)
        {
            var cc = context.Resolve<IComponentContext>();
            return async (p1, p2) =>
            {
                var resolved = cc.Resolve<TImplementer>();
                await resolved.InitializeAsync(p1, p2);
                return resolved;
            };
        });
    }
}
