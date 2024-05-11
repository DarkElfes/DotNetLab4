using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services.IServices;
using Shared.DTOs.Request.Auth;
using Shared.DTOs.Response;

namespace Server.Controllers;

[Route("[controller]")]
public class AuthController(
    SignInManager<ApplicationUser> signInManager,
    UserManager<ApplicationUser> userManager,
    IJwtTokenGenerator jwtTokenGenerator
    ) : ControllerBase
{
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;


    [HttpPost]
    [Route("register")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
    {
        if (!ModelState.IsValid)
            return StatusCode(StatusCodes.Status400BadRequest, "Incorrect data");

        if (await _userManager.FindByEmailAsync(registerRequest.Email!) is ApplicationUser exist)
            return StatusCode(StatusCodes.Status400BadRequest, $"User with email {exist.Email} has been registered already");

        ApplicationUser user = new()
        {
            UserName = registerRequest.Name,
            Email = registerRequest.Email,
            NormalizedEmail = registerRequest.Email!.ToUpper()
        };

        var result = await _userManager.CreateAsync(user, registerRequest.Password!);

        if (!result.Succeeded)
            return StatusCode(StatusCodes.Status500InternalServerError, "Registration failed");

        return StatusCode(StatusCodes.Status201Created, new AuthResponse(_jwtTokenGenerator.GenerateToken(user)));
    }

    [HttpPost]
    [Route("login")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        if (!ModelState.IsValid)
            return StatusCode(StatusCodes.Status400BadRequest, "Incorrect data");

        ApplicationUser? user = await _userManager.FindByEmailAsync(loginRequest.Email!);

        if (user is null)
            return StatusCode(StatusCodes.Status400BadRequest, $"User with this email not exist");


        var result = await _signInManager.PasswordSignInAsync(user, loginRequest.Password!, isPersistent: false, lockoutOnFailure: false);

        if (!result.Succeeded)
            return StatusCode(StatusCodes.Status400BadRequest, "Invalid login attempt");

        return StatusCode(StatusCodes.Status200OK, new AuthResponse(_jwtTokenGenerator.GenerateToken(user)));
    }


}
