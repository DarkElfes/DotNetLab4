using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace MauiBlazorClient.Services.Authentication;
public class AuthStateProvider(
    IMemoryCache memoryCache
    ) : AuthenticationStateProvider
{
    private readonly IMemoryCache _memoryCache = memoryCache;

    private static readonly Task<AuthenticationState> defaultAuthenticationStateTask =
       Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));

    private Task<AuthenticationState> authenticationStateTask = defaultAuthenticationStateTask;


    public async override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (await GetTokenAsync() is JsonWebToken jwt)
        {
            authenticationStateTask = Task.FromResult(
            new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(
                claims: jwt.Claims,
                authenticationType: nameof(AuthenticationStateProvider)))));
        }
        else
            authenticationStateTask = defaultAuthenticationStateTask;

        NotifyAuthenticationStateChanged(authenticationStateTask);

        return authenticationStateTask.Result;
    }

    public async Task<JsonWebToken?> GetTokenAsync()
    {
        if (!_memoryCache.TryGetValue("cached_jwt_token", out string? token) || string.IsNullOrWhiteSpace(token))
            token = await SecureStorage.Default.GetAsync("jwt_token");

        try
        {
            JsonWebToken jwt = new(token);

            return jwt.ValidTo > DateTime.UtcNow ? jwt : null;
        }
        catch
        {
            return null;
        }
    }


}


