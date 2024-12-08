using CommunityToolkit.Maui;
using HueApp.Domain.Clients;
using HueApp.Infrastructure.PhilipsHueApi;
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
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Services:
            builder.Services.AddHttpClient<IPhilipsHueApiClient, PhilipsHueApiClient>(); // Client
            builder.Services.AddSingleton<ISecureStorage>(o => SecureStorage.Default); // Secure Storage

            // Pages:
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<MainPageViewModel>();

            builder.Services.AddTransient<LightPage>();
            builder.Services.AddTransient<LightPageViewModel>();

            builder.Services.AddTransient<LightDetailPage>();
            builder.Services.AddTransient<LightDetailPageViewModel>();

            // Debug:
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
