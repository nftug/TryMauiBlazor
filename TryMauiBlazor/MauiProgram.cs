using Microsoft.AspNetCore.Components.WebView.Maui;
using MudBlazor.Services;
using TryMauiBlazor.Services;
using MudBlazor;
using TryMauiBlazor.Services.UserPreferences;
using CommunityToolkit.Maui;

namespace TryMauiBlazor;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiCommunityToolkit()
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();

        // #if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        // #endif

        builder.Services.AddSingleton<LayoutService>();
        builder.Services.AddSingleton<NavigationService>();

        builder.Services.AddTransient<NoteRepositoryService>();
        builder.Services.AddSingleton<NoteStoreService>();

        builder.Services.AddMudServices(config =>
        {
            config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomCenter;
            config.SnackbarConfiguration.PreventDuplicates = false;
            config.SnackbarConfiguration.NewestOnTop = true;
            config.SnackbarConfiguration.ShowCloseIcon = true;
            config.SnackbarConfiguration.VisibleStateDuration = 5000;
            config.SnackbarConfiguration.HideTransitionDuration = 200;
            config.SnackbarConfiguration.ShowTransitionDuration = 200;
            config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
        });

        builder.Services.AddScoped<UserPreferencesService>();

        return builder.Build();
    }
}
