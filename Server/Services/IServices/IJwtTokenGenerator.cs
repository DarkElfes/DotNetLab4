using Microsoft.AspNetCore.Identity;

namespace Server.Services.IServices;

public interface IJwtTokenGenerator
{
    string GenerateToken(IdentityUser user);
}
