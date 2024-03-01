using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using Microsoft.Extensions.Caching.Memory;

namespace MauiBlazorClient.Services;
public class PersistentAuthenticationStateProvider(
    IMemoryCache memoryCache) : AuthenticationStateProvider
{
    private readonly IMemoryCache _memoryCache = memoryCache;

    private static readonly Task<AuthenticationState> defaultAuthenticationStateTask =
       Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));

    private Task<AuthenticationState> authenticationStateTask = defaultAuthenticationStateTask;
    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        string? token = string.Empty;

        if (!_memoryCache.TryGetValue("cached_jwt_token", out token) || string.IsNullOrEmpty(token))
            token = await SecureStorage.Default.GetAsync("jwt_token");

        try
        {
            JsonWebToken? jwt = new(token);

            authenticationStateTask = Task.FromResult(
                new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(
                    claims: jwt.Claims,
                    authenticationType: nameof(AuthenticationStateProvider)))));
        }
        catch
        {
            authenticationStateTask = defaultAuthenticationStateTask;
        }

        NotifyAuthenticationStateChanged(authenticationStateTask);

        return authenticationStateTask.Result;
    }
}
