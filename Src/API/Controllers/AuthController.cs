using Application.Abstracts.Services;
using Application.Dtos.Auth;
using Application.Shared.Helpers.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    // ---------------- REGISTER ----------------
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest request,
        CancellationToken ct)
    {
        var (success, error) = await _authService.RegisterAsync(request, ct);

        if (!success)
            return BadRequest(BaseResponse.Fail(error!));

        return Ok(BaseResponse.Ok("User registered successfully."));
    }

    // ---------------- LOGIN ----------------
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        CancellationToken ct)
    {
        var token = await _authService.LoginAsync(request, ct);

        if (token is null)
            return Unauthorized(
                BaseResponse<TokenResponse>.Fail("Invalid login or password."));

        return Ok(BaseResponse<TokenResponse>.Ok(token));
    }

    // ---------------- REFRESH TOKEN ----------------
    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> Refresh(
        [FromBody] RefreshTokenRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
            return BadRequest(
                BaseResponse<TokenResponse>.Fail("Refresh token is required."));

        var tokenResponse =
            await _authService.RefreshTokenAsync(request.RefreshToken);

        if (tokenResponse is null)
            return Unauthorized(
                BaseResponse<TokenResponse>.Fail("Invalid or expired refresh token."));

        return Ok(BaseResponse<TokenResponse>.Ok(tokenResponse));
    }
}
