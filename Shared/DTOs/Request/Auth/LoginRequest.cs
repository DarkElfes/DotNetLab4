using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Request.Auth;

public record LoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}