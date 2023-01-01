using System.Reflection;
using Autofac;
using ReactiveUI;
using Splat;
using Splat.Autofac;

namespace HelloCommander.HcApp;

class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        // https://github.com/reactiveui/splat/blob/main/src/Splat.Autofac/README.md
        var builder = new ContainerBuilder();
        builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AsImplementedInterfaces();

        new Startup().ConfigureServices(builder);

        var autofacResolver = builder.UseAutofacDependencyResolver();
        builder.RegisterInstance(autofacResolver);

        Locator.SetLocator(autofacResolver);

        autofacResolver.InitializeSplat();
        autofacResolver.InitializeReactiveUI();

        var container = builder.Build();
        autofacResolver.SetLifetimeScope(container);

        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }

    private static AppBuilder BuildAvaloniaApp()
    {
        return AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .LogToTrace();
    }
}
