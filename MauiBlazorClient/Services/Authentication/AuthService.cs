using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.JsonWebTokens;
using Shared.DTOs.Request.Auth;
using Shared.DTOs.Response;
using System.Net.Http.Json;
using System.Security.Claims;

namespace MauiBlazorClient.Services.Authentication;

public class AuthService
{
    private readonly AuthStateProvider _authStateProvider;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMemoryCache _memoryCache;

    public event Action? OnLoggedOut;


    public AuthService(
        AuthenticationStateProvider authStateProvider,
        IHttpClientFactory httpClinetFactory,
        IMemoryCache memoryCache)
    {
        (_authStateProvider, _httpClientFactory, _memoryCache) = ((AuthStateProvider)authStateProvider, httpClinetFactory, memoryCache);


    }

    public async Task<bool> IsLoggedAsync()
        => await _authStateProvider.GetTokenAsync() is JsonWebToken jwt;

    public async Task<UserDTO?> GetCurrentUserAsync()
    {
        if (await _authStateProvider.GetTokenAsync() is JsonWebToken jwt)
            return new(
                jwt.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.NameIdentifier))!.Value,
                jwt.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Name))!.Value,
                jwt.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Email))!.Value)
            { IsOnline = true };
        else
            return null;
    }

    public void Logout()
    {
        SecureStorage.Default.Remove("jwt_token");
        _memoryCache.Remove("cached_jwt_token");
        OnLoggedOut?.Invoke();
    }





    public async Task<(bool, string?)> LoginAsync(LoginRequest loginRequest)
        => await AuthResponseHandler(await GetApiClient().PostAsJsonAsync("auth/login", loginRequest));
    public async Task<(bool, string?)> RegisterAsync(RegisterRequest registerRequest)
        => await AuthResponseHandler(await GetApiClient().PostAsJsonAsync("auth/register", registerRequest));

    private HttpClient GetApiClient()
        => _httpClientFactory.CreateClient("ServerApi");
    private async Task<(bool, string?)> AuthResponseHandler(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            string error = await response.Content.ReadAsStringAsync();
            return (false, !string.IsNullOrWhiteSpace(error) ? error : "Iternal server error. Try later.");
        }

        if (await response.Content.ReadFromJsonAsync<AuthResponse>() is AuthResponse authResponse
                && !string.IsNullOrEmpty(authResponse.Token))
        {
            await SecureStorage.Default.SetAsync("jwt_token", authResponse.Token);
            _memoryCache.Set("cached_jwt_token", authResponse.Token);

            await _authStateProvider.GetAuthenticationStateAsync();
            return (true, null);
        }
        else
            throw new InvalidDataException();
    }
}
