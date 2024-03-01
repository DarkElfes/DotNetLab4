global using MauiBlazorClient.Services;
global using MauiBlazorClient.Services.IServices;
using MauiBlazorClient.Services.ChatServices.GroupChatService;
using MauiBlazorClient.Services.ChatServices.PersonalChatService;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace MauiBlazorClient
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
            builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

            builder.Services.AddMemoryCache();

            builder.Services.AddHttpClient("ServerApi", options =>
            {
                options.BaseAddress = new Uri(builder.Configuration.GetValue<string>("ServerApiUrl")
                    ?? throw new ArgumentException("Not set server url"));
            });

            builder.Services.AddScoped<IAnimatedNavigationManager, AnimatedNavigationManager>();
            builder.Services.AddScoped<IModalDialogService, ModalDialogService>();

            builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();
            builder.Services.AddSingleton<IPersonalChatService, PersonalChatService>();
            builder.Services.AddSingleton<IGroupChatService, GroupChatService>();
            builder.Services.AddSingleton<ChatManager>();



#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
