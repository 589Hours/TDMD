using Microsoft.Extensions.Logging;
using Routey.Domain.Clients;
using Routey.Domain.SQLiteDatabases;
using Routey.Domain.SQLiteDatabases.Entities;
using Routey.Infrastructure.SQLiteDatabases;
using Routey.Infrastructure.WeatherApiClient;
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
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            //.UseMauiMaps(); Usage of the MAUI map: https://learn.microsoft.com/en-us/dotnet/maui/user-interface/controls/map?view=net-maui-9.0
            // Views & ViewModels:
            builder.Services.AddTransient<MapPage>();
            builder.Services.AddTransient<MapPageViewModel>();
            builder.Services.AddTransient<RoutesPage>();
            builder.Services.AddTransient<RoutesPageViewModel>();
            builder.Services.AddTransient<SettingsPage>();
            builder.Services.AddTransient<SettingsPageViewModel>();

            // SQLite Database & HttpClient:
            builder.Services.AddSingleton<IRouteDatabase, SQLRouteDatabase>();
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
