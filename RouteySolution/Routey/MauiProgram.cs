using System.Globalization;
using LocalizationResourceManager.Maui;
using Microsoft.Extensions.Logging;
using Routey.Domain.Clients;
using Routey.Domain.SQLiteDatabases;
using Routey.Infrastructure.SQLiteDatabases;
using Routey.Infrastructure.WeatherApiClient;
using Routey.Resources.Localization;
using Routey.ViewModels;

namespace Routey
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
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
                });
            //.UseMauiMaps(); Usage of the MAUI map: https://learn.microsoft.com/en-us/dotnet/maui/user-interface/controls/map?view=net-maui-9.0

            // Views & ViewModels:
            builder.Services.AddTransient<MapPage>();
            builder.Services.AddTransient<MapPageViewModel>(); //AddSingleton
            builder.Services.AddTransient<RoutesPage>();
            builder.Services.AddTransient<RoutesPageViewModel>(); //AddSingleton
            builder.Services.AddTransient<SettingsPage>(); // This page doesn't have a ViewModel because the language and modes are updated dynamically
            builder.Services.AddTransient<SettingsPageViewModel>();

            // SQLite Database & HttpClient:
            builder.Services.AddSingleton<IRouteDatabase>(new SQLRouteDatabase(Path.Combine(FileSystem.Current.AppDataDirectory, "routesqlitedatabase.db3")));
            builder.Services.AddHttpClient<IWeatherApiClient, WeatherApiClient>(client =>
            {
                // TODO add string to client BaseAddress
                client.BaseAddress = new Uri("");
            });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
