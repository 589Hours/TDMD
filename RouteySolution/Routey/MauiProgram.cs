using Microsoft.Extensions.Logging;
using Routey.Domain.Clients;
using Routey.Domain.SQLiteDatabases;
using Routey.Infrastructure.WeatherApiClient;

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
            builder.Services.AddSingleton<IRouteDatabase>();
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
