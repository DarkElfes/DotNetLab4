using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace MauiBlazorClient.Services;

public class AnimatedNavigationManager: IAnimatedNavigationManager, IDisposable
{
    private readonly NavigationManager _navigation;

    public event Func<int>? OnPreNavigation;
    public event Action? OnAfterNavigation;


    public AnimatedNavigationManager(IServiceProvider serviceProvider)
    {
        _navigation = serviceProvider.GetRequiredService<NavigationManager>();
        _navigation.LocationChanged += HandleLocationChanged;
    }

    public async Task NavigateToAsync(string uri, bool forceload = false)
    {
        int delay = OnPreNavigation?.Invoke() ?? 0;
        await Task.Delay(delay);

        _navigation.NavigateTo(uri, forceload);
    }

    private void HandleLocationChanged(object? sender, LocationChangedEventArgs e)
        => OnAfterNavigation?.Invoke();

    public void Dispose()
        => _navigation.LocationChanged -= HandleLocationChanged;
}