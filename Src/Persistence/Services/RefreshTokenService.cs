using Application.Abstractions.Services;
using Application.Options;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Persistence.Context;
using System.Security.Cryptography;

namespace Persistence.Services;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly BinaLiteDbContext _context;
    private readonly JwtOptions _options;

    public RefreshTokenService(
        BinaLiteDbContext context,
        IOptions<JwtOptions> options)
    {
        _context = context;
        _options = options.Value;
    }

    public async Task<string> CreateAsync(User user)
    {
        var randomBytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);

        var token = Convert.ToHexString(randomBytes);

        var refreshToken = new RefreshToken
        {
            Token = token,
            UserId = user.Id,
            ExpiresAtUtc = DateTime.UtcNow
                .AddMinutes(_options.RefreshExpirationMinutes)
        };

        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();

        return token;
    }


    public async Task<User?> ValidateAndConsumeAsync(string token)
    {
        var refreshToken = await _context.RefreshTokens
            .Include(x => x.User)
            .FirstOrDefaultAsync(x => x.Token == token);

        if (refreshToken is null)
            return null;

        if (refreshToken.ExpiresAtUtc <= DateTime.UtcNow)
            return null;

        var user = refreshToken.User;

        _context.RefreshTokens.Remove(refreshToken);
        await _context.SaveChangesAsync();

        return user;
    }
}
