using InventoryManagement.Api.Contracts;
using InventoryManagement.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly IUserAuthRepository _userAuthRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthController(
        IUserAuthRepository userAuthRepository,
        IPasswordHasher passwordHasher,
        IJwtTokenGenerator jwtTokenGenerator)
    {
        _userAuthRepository = userAuthRepository;
        _passwordHasher = passwordHasher;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
        {
            return Unauthorized(new { message = "Credenciales invalidas." });
        }

        var user = await _userAuthRepository.GetByUsernameOrEmailAsync(request.Username, cancellationToken);
        if (user is null || !user.IsActive || !_passwordHasher.VerifyPassword(request.Password, user.PasswordHash))
        {
            return Unauthorized(new { message = "Credenciales invalidas." });
        }

        var roles = user.UserRoles
            .Select(userRole => userRole.Role?.Name)
            .Where(role => !string.IsNullOrWhiteSpace(role))
            .Cast<string>()
            .ToArray();

        var token = _jwtTokenGenerator.GenerateToken(user.Id, user.Username, user.FullName, roles);

        return Ok(new
        {
            accessToken = token,
            tokenType = "Bearer",
            username = user.Username,
            fullName = user.FullName,
            roles
        });
    }
}
