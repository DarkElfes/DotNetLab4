using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Request.Auth;

public record LoginRequest
{
    [EmailAddress]
    [Required]
    public string? Email { get; set; }

    [DataType(DataType.Password)]
    [Required]
    public string? Password { get; set; }
}