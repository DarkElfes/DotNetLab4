namespace MauiBlazorClient.Services.IServices;

public interface IAnimatedNavigationManager
{
    event Func<int>? OnPreNavigation;
    event Action? OnAfterNavigation;

    Task NavigateToAsync(string uri, bool forceload = false);
}