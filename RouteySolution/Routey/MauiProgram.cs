using System.Globalization;
using CommunityToolkit.Maui;
using LocalizationResourceManager.Maui;
using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;
using Routey.Domain.SQLiteDatabases;
using Routey.Infrastructure.SQLiteDatabases;
using Routey.Resources.Localization;
using Routey.ViewModels;

namespace Routey
{
    /// <summary>
    /// This class has been documented with the help of GitHub Copilot!
    /// The main entry point for the application.
    /// </summary>
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                // Packages and app components:
                .UseMauiApp<App>()
                .UseLocalNotification()
                .UseLocalizationResourceManager(settings =>
                {
                    settings.RestoreLatestCulture(true);
                    settings.AddResource(AppResources.ResourceManager);
                    settings.InitialCulture(new CultureInfo("en-US"));
                })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
#if __ANDROID__ || __IOS___
            .UseMauiMaps()
#endif
            .UseMauiCommunityToolkit();

            // Views & ViewModels:
            builder.Services.AddTransient<MapPage>();
            builder.Services.AddTransient<MapPageViewModel>();          

            builder.Services.AddTransient<RoutesPage>();
            builder.Services.AddTransient<RoutesPageViewModel>();

            builder.Services.AddTransient<SettingsPage>();
            builder.Services.AddTransient<SettingsPageViewModel>();

            // SQLite Database & Map Location:
            builder.Services.AddSingleton<IRouteDatabase, SQLRouteDatabase>(o => 
            {
                return new SQLRouteDatabase(Path.Combine(FileSystem.Current.AppDataDirectory, "routesqlitedatabase.db3")); // Database file name
            });
            builder.Services.AddSingleton<IGeolocation>(o => Geolocation.Default);
#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}