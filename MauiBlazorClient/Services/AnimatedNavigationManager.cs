using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace MauiBlazorClient.Services;

public class AnimatedNavigationManager : IAnimatedNavigationManager, IDisposable
{
    private readonly NavigationManager _navigation;

    public event Func<int>? OnPreNavigation;
    public event Action? OnAfterNavigation;


    public AnimatedNavigationManager(NavigationManager navigation)
    {
        _navigation = navigation;
        _navigation.LocationChanged += HandleLocationChanged;
    }

    public async Task NavigateToAsync(string uri, bool forceload = false)
    {
        int delay = OnPreNavigation?.Invoke() ?? 0;
        await Task.Delay(delay);

        _navigation.NavigateTo(uri, forceload);
    }
    public void Dispose()
        => _navigation.LocationChanged -= HandleLocationChanged;

    private void HandleLocationChanged(object? sender, LocationChangedEventArgs e)
        => OnAfterNavigation?.Invoke();
}