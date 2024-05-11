using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Request.Chat;

public class AddUserRequest
{
    [EmailAddress]
    public string? UserEmail { get; set; }
}
