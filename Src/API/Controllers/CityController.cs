using Application.Abstracts.Services;
using Application.Dtos.City;
using Application.Shared.Helpers.Responses;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CityController : ControllerBase
{
    private readonly ICityService _cityService;
    public CityController(ICityService cityService)
    {
        _cityService = cityService;
    }

    [HttpGet]
    public async Task<ActionResult<BaseResponse<List<GetAllCtiyResponse>>>> GetAllAsync(CancellationToken ct)
    {
        var cities = await _cityService.GetAllAsync(ct);
        return Ok(BaseResponse<List<GetAllCtiyResponse>>.Ok(cities));
    }
    
    [HttpPost]
    public async Task<ActionResult<BaseResponse>> CreateAsync([FromBody] CreateCityRequest request, CancellationToken ct)
    {
        var ok = await _cityService.CreateCityAsync(request, ct);

        if (!ok)
            return BadRequest(BaseResponse.Fail("Could not create City"));

        return StatusCode(StatusCodes.Status201Created, BaseResponse.Ok("Created successfully"));
    }
    [HttpPut("{id:int}")]
    public async Task<ActionResult<BaseResponse>> UpdateAsync(int id, [FromBody] UpdateCityRequest request, CancellationToken ct)
    {
        {
            try
            {
                var ok = await _cityService.UpdateCityAsync(id, request, ct);

                if (!ok)
                    return NotFound(BaseResponse.Fail("City not found"));

                return Ok(BaseResponse.Ok("Updated successfully"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(BaseResponse.Fail(ex.Message));
            }
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<BaseResponse>> DeleteAsync(int id, CancellationToken ct)
    {
        var ok = await _cityService.DeleteCityAsync(id, ct);

        if (!ok)
            return NotFound(BaseResponse.Fail("City not found"));

        return Ok(BaseResponse.Ok("Deleted successfully"));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<BaseResponse<GetByIdCityResponse>>> GetByIdAsync(int id, CancellationToken ct)
    {
        var city = await _cityService.GetByIdAsync(id, ct);

        if (city == null)
            return NotFound(BaseResponse<GetByIdCityResponse>.Fail("City not found"));

        return Ok(BaseResponse<GetByIdCityResponse>.Ok(city));
    }
}
