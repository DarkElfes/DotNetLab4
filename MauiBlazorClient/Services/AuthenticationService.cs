using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Shared.DTOs.Request.Auth;
using Shared.DTOs.Response;
using System.Net.Http.Json;

namespace MauiBlazorClient.Services;

public record AuthenticationService(
    IHttpClientFactory _httpClientFactory,
    IMemoryCache _memoryCached,
    AuthenticationStateProvider _authenticationStateProvider
    ) : IAuthenticationService
{
    public event Action? OnLoggedOut;


    public async Task<string?> LoginAsync(LoginRequest loginRequest)
        => await AuthResponseHandler(await GetApiClient().PostAsJsonAsync("auth/login", loginRequest));
    public async Task<string?> RegisterAsync(RegisterRequest registerRequest)
        => await AuthResponseHandler(await GetApiClient().PostAsJsonAsync("auth/register", registerRequest));
    public void Logout()
    {
        SecureStorage.Default.Remove("jwt_token");
        _memoryCached.Remove("cached_jwt_token");
        OnLoggedOut?.Invoke();
    }


    public async ValueTask<string?> GetTokenAsync()
    {
        string? token;

        if (_memoryCached.TryGetValue("cached_jwt_token", out token) && !string.IsNullOrWhiteSpace(token))
            return token;
        else
            token = await SecureStorage.GetAsync("jwt_token");

        if (!string.IsNullOrWhiteSpace(token))
            return token;
        else
            Logout();

        return null;
    }

    private HttpClient GetApiClient()
        => _httpClientFactory.CreateClient("ServerApi");
    private async Task<string?> AuthResponseHandler(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
            return await response.Content.ReadAsStringAsync();

        if (await response.Content.ReadFromJsonAsync<AuthResponse>() is AuthResponse authResponse
            && !string.IsNullOrEmpty(authResponse.Token))
        {
            await SecureStorage.Default.SetAsync("jwt_token", authResponse.Token);
            _memoryCached.Set("cached_jwt_token", authResponse.Token);
            await _authenticationStateProvider.GetAuthenticationStateAsync();
            return null;
        }
        else
            throw new InvalidDataException();
    }
}
