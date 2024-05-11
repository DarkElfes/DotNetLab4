using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.Request.Chat;

public class PersonalChatRequest : ChatRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
}
