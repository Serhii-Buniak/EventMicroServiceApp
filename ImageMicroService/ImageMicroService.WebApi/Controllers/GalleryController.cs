using ImageMicroService.Application.Common.Models;
using ImageMicroService.Application.GalleryActions.Commands;
using ImageMicroService.Application.GalleryActions.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static ImageMicroService.Infrastructure.AuthorizationConfigs;

namespace ImageMicroService.WebApi.Controllers;

public class GalleryController : ApiControllerBase
{
    public GalleryController()
    {

    }

    [HttpGet]
    public async Task<IActionResult> GetGalleries()
    {
        IEnumerable<GalleryDto> galleries = await Mediator.Send(new GetGalleriesQuery());
        return Ok(galleries);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetGallery(long id)
    {
        GalleryDto gallery = await Mediator.Send(new GetGalleryQuery(id));
        return Ok(gallery);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateGallery(CreateGalleryCommand command)
    {
        GalleryDto gallery = await Mediator.Send(command);
        return Ok(gallery);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = $"{Moderator}, {Administrator}")]
    public async Task<IActionResult> DeleteGallery(long id)
    {
        await Mediator.Send(new DeleteGalleryCommand(id));
        return NoContent();
    }
}