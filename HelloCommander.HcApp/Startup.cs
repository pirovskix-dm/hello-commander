using System.Collections.Generic;
using Autofac;
using HelloCommander.BL.Services;
using HelloCommander.Core.Dependencies;
using HelloCommander.Core.ViewModels.Controls;
using HelloCommander.Core.ViewModels.Controls.HcTabItems;
using HelloCommander.Core.ViewModels.Windows;
using HelloCommander.HcApp.Dependencies;
using HelloCommander.HcApp.Utils;

namespace HelloCommander.HcApp;

public class Startup
{
    public void ConfigureServices(ContainerBuilder builder)
    {
        builder.RegisterTransient<NavigationHistory, INavigationHistory>();
        builder.RegisterTransient<TerminalProcessService, ITerminalProcessService>();
        builder.RegisterTransient<HcTabTerminalViewModel>();
        builder.RegisterTransient<HcTabItemsViewModel>();

        builder.RegisterAsyncInitializableFuncFactory<HcTabViewModel, Guid, string>();
        builder.RegisterInitializableFuncFactory<HcTabDirectoryViewModel, IHcDirectory>();
        builder.RegisterInitializableFuncFactory<HcTabFileViewModel, IHcFile>();
        builder.RegisterFuncFactory<HcBookmarkViewModel>();
        builder.RegisterFuncFactory<HcTabRootViewModel>();

        builder.RegisterSingleton<MainWindowViewModel>();
        builder.RegisterSingleton<HcFileService, IHcFileService>();
        builder.RegisterSingleton<SettingsService, ISettingsService>();
        builder.RegisterSingleton<HcRuntimePlatform, IHcRuntimePlatform>();
        builder.RegisterSingleton<UserInteractionHelper, IUserInteractionHelper>();
        builder.RegisterSingleton<HcCommandFactory, IHcCommandFactory>();
        builder.RegisterSingleton<HcErrorHandler, IHcErrorHandler>();
        builder.RegisterSingleton<SynchronizationHelper, ISynchronizationHelper>();
        builder.RegisterSingleton<BookmarksService, IBookmarksService>();
        builder.RegisterSingleton<SaveStateService, ISaveStateService>();

        builder.RegisterSingleton(_ => new HcStorage<List<HcBookmark>>("bookmarks.json", new List<HcBookmark>()));
        builder.RegisterSingleton(_ => new HcStorage<HcSaveState>("state.json", new HcSaveState(
            Tabs: new List<HcOpenedTab> { new(Guid.NewGuid(), string.Empty, 1) }
        )));
        builder.RegisterSingleton(_ => new HcStorage<HcSettings>("settings.json", new HcSettings(
            HomeDirectory: Environment.GetFolderPath(Environment.SpecialFolder.UserProfile
            ))));
    }
}
