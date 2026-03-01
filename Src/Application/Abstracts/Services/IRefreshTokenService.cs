using Domain.Entities;

namespace Application.Abstractions.Services;

public interface IRefreshTokenService
{
    Task<string> CreateAsync(User user);
    Task<User?> ValidateAndConsumeAsync(string token);
}
