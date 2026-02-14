using Application.Abstracts.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;

namespace Persistence.Repositories;

public class CityRepository:GenericRepository<City,int>,ICityRepository

{
    private readonly BinaLiteDbContext _context;
    public CityRepository(BinaLiteDbContext context ) : base(context)
    {
        _context = context;
    }
    public async Task<bool> ExistsByNameAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return false;

        var normalizedName = name.Trim().ToLower();

        return await _context.Cities
            .AsNoTracking()
            .AnyAsync(x => x.Name.ToLower() == normalizedName);
    }

}
