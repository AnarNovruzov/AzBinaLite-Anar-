using Application.Dtos.City;

public interface ICityService
{
    Task<List<GetAllCtiyResponse>> GetAllAsync(CancellationToken ct);
    Task<GetByIdCityResponse?> GetByIdAsync(int id, CancellationToken ct);
    Task<bool> CreateCityAsync(CreateCityRequest request, CancellationToken ct);
    Task<bool> UpdateCityAsync(int id, UpdateCityRequest request, CancellationToken ct);
    Task<bool> DeleteCityAsync(int id, CancellationToken ct);
}