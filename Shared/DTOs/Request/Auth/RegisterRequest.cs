using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Request.Auth;

public record RegisterRequest : LoginRequest
{
    [StringLength(50, MinimumLength = 2)]
    [Required]
    public string? Name { get; set; }
}
