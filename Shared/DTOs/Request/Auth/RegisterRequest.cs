using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Request.Auth;

public record RegisterRequest : LoginRequest
{
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Name { get; set; } = null!;
}
