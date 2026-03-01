using Application.Abstractions.Services;
using Application.Abstracts.Services;
using Application.Dtos.Auth;
using Application.Options;
using Domain.Entities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Persistence.Services;

public class AuthService:IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IJwtTokenGenerator _jwtGenerator;
    private readonly IRefreshTokenService _refreshtokenservice;
    private readonly JwtOptions _jwtOptions;

    public AuthService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IJwtTokenGenerator jwtGenerator,
        IRefreshTokenService refreshtokenservice,
        IOptions<JwtOptions> jwtOptions)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtGenerator = jwtGenerator;
        _refreshtokenservice = refreshtokenservice;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<(bool Success, string? Error)> RegisterAsync(RegisterRequest request, CancellationToken ct = default)
    {
        var user = new User
        {
            UserName = request.Email,
            Email = request.Email,
            FullName = request.FullName
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            return (false, result.Errors.First().Description);

        return (true, null);
    }
    private async Task<TokenResponse> BuildTokenResponseAsync(User user, CancellationToken ct)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var accessToken = _jwtGenerator.GenerateToken(user);

        var refreshToken = await _refreshtokenservice.CreateAsync(user);

        var expiresAtUtc = DateTime.UtcNow.AddMinutes(_jwtOptions.ExpirationMinutes);

        return new TokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAtUtc = expiresAtUtc
        };
    }

    public async Task<TokenResponse?> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        var user = await _userManager.FindByEmailAsync(request.Login);

        if (user is null)
            return null;

        var passwordValid = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!passwordValid)
            return null;

        return await BuildTokenResponseAsync(user,ct);
    }


    public async Task<TokenResponse?> RefreshTokenAsync(string refreshToken,CancellationToken ct=default)
    {
        var user = await _refreshtokenservice.ValidateAndConsumeAsync(refreshToken);

        if (user is null)
            return null;

        return await BuildTokenResponseAsync(user,ct);
    }

}

