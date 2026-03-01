using Domain.Entities;

namespace Application.Abstracts.Repositories;

public interface IRefreshTokenRepository
{
    /// <summary>
    /// Token ilə tap və user daxil et
    /// </summary>
    Task<RefreshToken?> GetByTokenWithUserAsync(string token, CancellationToken ct = default);

    /// <summary>
    /// Yeni token əlavə et
    /// </summary>
    Task AddAsync(RefreshToken refreshToken, CancellationToken ct = default);

    /// <summary>
    /// Token sil (consume)
    /// </summary>
    Task DeleteByTokenAsync(string token, CancellationToken ct = default);
}
