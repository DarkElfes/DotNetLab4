using Microsoft.AspNetCore.Identity;
using Server.Models;
using System.Security.Claims;

namespace Server.Helpers;

public record UserHelper(
    UserManager<ApplicationUser> _userManager)
{
    public async Task<ApplicationUser> GetCurrentUserAsync(ClaimsPrincipal? user)
    {
        ArgumentNullException.ThrowIfNull(user);

        if (await _userManager.GetUserAsync(user) is ApplicationUser currentUser)
            return currentUser;

        throw new ArgumentException("Current user not found, try to relogin.");
    }
}
