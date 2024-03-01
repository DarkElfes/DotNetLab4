namespace Server.Models;

public class JwtSettings
{
    public const string SectionName = "JwtSettings";
    public string Secret { get; init; } = null!;
    public int ExpiryMinutes { get; init; }
    public string Issuer { get; set; } = null!;
    public string Audience { get; set; } = null!;
}
