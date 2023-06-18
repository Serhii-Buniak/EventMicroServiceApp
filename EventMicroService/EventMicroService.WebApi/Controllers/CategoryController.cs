using EventMicroService.Application.CategoryCQRS.Commands;
using EventMicroService.Application.CategoryCQRS.Queries;
using EventMicroService.Application.Common.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static EventMicroService.Infrastructure.AuthorizationConfigs;

namespace EventMicroService.WebApi.Controllers;

public class CategoryController : ApiControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        return Ok(await Mediator.Send(new GetAllCategoryQuery()));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCategory(long id)
    {
        return Ok(await Mediator.Send(new GetCategoryQuery(id)));
    }

    [HttpPost]
    [Authorize(Roles = $"{Moderator}, {Administrator}")]
    public async Task<IActionResult> CreateCategory(CreateCategoryCommand createCategoryCommand)
    {
        CategoryReadDto category = await Mediator.Send(createCategoryCommand);

        return CreatedAtAction(nameof(GetCategory), new { category.Id }, category);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = $"{Moderator}, {Administrator}")]
    public async Task<IActionResult> UpdateCategory(long id, UpdateCategoryCommand updateCategoryCommand)
    {
        updateCategoryCommand.SetId(id);
        CategoryReadDto category = await Mediator.Send(updateCategoryCommand);
        return CreatedAtAction(nameof(GetCategory), new { category.Id }, category);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = $"{Moderator}, {Administrator}")]
    public async Task<IActionResult> DeleteCategory(long id)
    {
        await Mediator.Send(new DeleteCategoryCommand(id));
        return NoContent();
    }

}
