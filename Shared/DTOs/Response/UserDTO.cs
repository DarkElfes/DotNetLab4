namespace Shared.DTOs.Response;

public record UserDTO(
    string Id,
    string UserName, 
    string Email
    )
{
    public bool IsOnline { get; set; }
}