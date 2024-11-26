using HueApp.Domain.Clients;
using HueApp.Infrastructure.HueApi;
using HueApp.ViewModels;
using Microsoft.Extensions.Logging;

namespace HueApp
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

            //TODO wel of geen base URL? Afhankelijk van ip-adres dus moeilijk misschien aan user overlaten?
            //TODO inloggen/account maken voor authorisatie?
            builder.Services.AddHttpClient<IPhilipsHueApiClient, PhilipsHueApiClient>();

            //TODO uncomment wanneer ViewModel is opgezet.
            //builder.Services.AddTransient<MainPage>();
            //builder.Services.AddTransient<MainPageViewModel>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
