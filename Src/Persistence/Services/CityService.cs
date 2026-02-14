using Application.Abstracts.Repositories;
using Application.Abstracts.Services;
using Application.Dtos.City;
using Application.Validations.CityValidation;
using AutoMapper;
using Domain.Entities;
using FluentValidation;

namespace Persistence.Services;

public class CityService : ICityService
{
    private readonly ICityRepository _repository;
    private readonly IMapper _mapper;
    private readonly IValidator<CreateCityRequest> _createValidator;
    private readonly IValidator<UpdateCityRequest> _updateValidator;

    public CityService(
        ICityRepository repository,
        IMapper mapper,
        IValidator<CreateCityRequest> createValidator,
        IValidator<UpdateCityRequest> updateValidator)
    {
        _repository = repository;
        _mapper = mapper;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    // GET ALL
    public async Task<List<GetAllCtiyResponse>> GetAllAsync(CancellationToken ct)
    {
        var entities = (await _repository.GetAllAsync(ct)).ToList();
        return _mapper.Map<List<GetAllCtiyResponse>>(entities);
    }

    // GET BY ID
    public async Task<GetByIdCityResponse?> GetByIdAsync(int id, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync(id, ct);

        if (entity is null)
            return null;

        return _mapper.Map<GetByIdCityResponse>(entity);
    }

    // CREATE
    public async Task<bool> CreateCityAsync(CreateCityRequest request, CancellationToken ct)
    {
        await _createValidator.ValidateAndThrowAsync(request, cancellationToken: ct);

        var entity = _mapper.Map<City>(request);
        await _repository.AddAsync(entity, ct);

        var affected = await _repository.SaveChangesAsync(ct);
        return affected > 0;
    }

    // UPDATE
    public async Task<bool> UpdateCityAsync(int id, UpdateCityRequest request, CancellationToken ct)
    {
        await _updateValidator.ValidateAndThrowAsync(request, cancellationToken: ct);

        if (request.Id != 0 && request.Id != id)
            return false;

        var entity = await _repository.GetByIdAsync(id, ct);
        if (entity is null)
            return false;

        _mapper.Map(request, entity);

        var affected = await _repository.SaveChangesAsync(ct);
        return affected > 0;
    }

    // DELETE
    public async Task<bool> DeleteCityAsync(int id, CancellationToken ct)
    {
        var entity = await _repository.GetByIdAsync(id, ct);
        if (entity is null)
            return false;

        await _repository.DeleteAsync(entity, ct);

        var affected = await _repository.SaveChangesAsync(ct);
        return affected > 0;
    }

 
}
