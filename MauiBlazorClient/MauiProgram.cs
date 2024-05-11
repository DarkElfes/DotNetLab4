global using MauiBlazorClient.Services;
global using MauiBlazorClient.Services.IServices;
global using MauiBlazorClient.Enums;
global using MauiBlazorClient.Components;
using MauiBlazorClient.Services.Authentication;
using MauiBlazorClient.Services.ChatServices;
using MauiBlazorClient.Services.ChatServices.IChatServices;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace MauiBlazorClient;

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
            });

        using var stream = Assembly.GetExecutingAssembly()
           .GetManifestResourceStream("MauiBlazorClient.appsettings.json");

        var config = new ConfigurationBuilder()
                    .AddJsonStream(stream ?? throw new ArgumentNullException("Not found appsettings.json"))
                    .Build();

        builder.Configuration.AddConfiguration(config);

        builder.Services.AddMauiBlazorWebView();

        builder.Services.AddAuthorizationCore();
        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddScoped<AuthService>();
        builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();

        builder.Services.AddMemoryCache();

        builder.Services.AddHttpClient("ServerApi", options =>
        {
            options.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ServerApiUrl")
                ?? throw new ArgumentException("Not set server url"));
        });

        builder.Services.AddScoped<IAnimatedNavigationManager, AnimatedNavigationManager>();

        builder.Services.AddScoped<IDialogService, DialogService>();
        builder.Services.AddScoped<IPersonalChatService, PersonalChatService>();
        builder.Services.AddScoped<IGroupChatService, GroupChatService>();
        builder.Services.AddScoped<ChatManager>();



#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
