using FinancialChat.Api.Contracts.Auth;
using FinancialChat.Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FinancialChat.Api.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase {
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        IConfiguration configuration) {
        _userManager = userManager;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request) {
        var user = new ApplicationUser {
            UserName = request.UserName,
            Email = request.Email
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok();
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request) {
        var user = await _userManager.FindByNameAsync(request.UserName);

        if (user == null)
            return Unauthorized();

        var valid = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!valid)
            return Unauthorized();

        var token = GenerateJwt(user);

        return Ok(token);
    }

    private JwtTokenResult GenerateJwt(ApplicationUser user) 
    {
        var key = _configuration["Jwt:Key"]!;
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];

        const int expiresInSeconds = 60 * 60; // 1 hora
        var expiresAt = DateTime.UtcNow.AddSeconds(expiresInSeconds);

        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(key)
        );

        var credentials = new SigningCredentials(
            securityKey,
            SecurityAlgorithms.HmacSha256
        );

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName!)
        };


        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiresAt,
            signingCredentials: credentials
        );

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

        return new JwtTokenResult(
            AccessToken: accessToken,
            ExpiresIn: expiresInSeconds
        );
    }
}
