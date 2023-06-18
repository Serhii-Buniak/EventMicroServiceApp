using CityMicroService.BLL.DTOs;
using CityMicroService.BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static CityMicroService.BLL.AuthorizationConfigs;

namespace CityMicroService.WebApi.Controllers;

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
    public async Task<IActionResult> GetCities()
    {
        IEnumerable<CityDTO> categories = await _cityService.GetAllAsync();
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCity(long id)
    {
        try
        {
            CityDTO city = await _cityService.GetByIdAsync(id);
            return Ok(city);
        }
        catch (ArgumentException e)
        {
            return NotFound(new { error = e.Message });
        }
    }

    [HttpPost]
    [Authorize(Roles = Administrator)]
    public async Task<IActionResult> CreateCity(CityRequestDTO cityRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        CityDTO city = await _cityService.CreateAsync(cityRequest);
        return CreatedAtAction(nameof(GetCity), new {city.Id }, city);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = Administrator)]
    public async Task<IActionResult> UpdateCity(long id, CityRequestDTO cityRequest)
    {
        try
        {
            CityDTO city = await _cityService.UpdateAsync(id, cityRequest);
            return CreatedAtAction(nameof(GetCity), new { id }, city);
        }
        catch (ArgumentException e)
        {
            return NotFound(new { error = e.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Administrator)]
    public async Task<IActionResult> DeleteCity(long id)
    {
        try
        {
            await _cityService.DeleteAsync(id);
            return NoContent();
        }
        catch (ArgumentException e)
        {
            return NotFound(new { error = e.Message });
        }
    }
}
