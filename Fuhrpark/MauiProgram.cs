using Fuhrpark.Service;
using Fuhrpark.ViewModels;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using Fuhrpark.View;

namespace Fuhrpark;

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

        // Dependency Injection für Services und ViewModels registrieren
        builder.Services.AddSingleton<DatabaseService>();
        builder.Services.AddSingleton<FleetViewModel>();
        builder.Services.AddSingleton<HomePage>();

        builder.Services.AddTransientPopup<AddVehicleView, FleetViewModel>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}