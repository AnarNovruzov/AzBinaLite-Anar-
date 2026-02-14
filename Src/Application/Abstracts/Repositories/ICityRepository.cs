using Domain.Entities;

namespace Application.Abstracts.Repositories
{
    public interface ICityRepository : IRepository<City, int>
    {

        Task<List<City>> GetAllAsync(CancellationToken ct);
        Task<bool> ExistsByNameAsync(string name);
    }
}
