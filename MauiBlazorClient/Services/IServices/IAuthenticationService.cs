using Shared.DTOs.Request.Auth;

namespace MauiBlazorClient.Services.IServices;

public interface IAuthenticationService
{
    event Action OnLoggedOut;

    Task<string?> LoginAsync(LoginRequest loginRequest);
    Task<string?> RegisterAsync(RegisterRequest registerRequest);
    void Logout();

    ValueTask<string?> GetTokenAsync();
}