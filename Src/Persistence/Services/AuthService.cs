using Application.Abstracts.Services;
using Application.Dtos.Auth;
using Domain.Entities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;

namespace Persistence.Services;

public class AuthService:IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IJwtTokenGenerator _jwtGenerator;

    public AuthService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IJwtTokenGenerator jwtGenerator)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtGenerator = jwtGenerator;
    }

    public Task<string?> LoginAsync(LoginRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<(bool Success, string? Error)> RegisterAsync(RegisterRequest request, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
