using ImageMicroService.Application.Common.Models;
using ImageMicroService.Application.ImageActions.Commands;
using ImageMicroService.Application.ImageActions.Queries;
using Microsoft.AspNetCore.Mvc;

namespace ImageMicroService.WebApi.Controllers;

public class ImageController : ApiControllerBase
{
    public ImageController()
    {

    }

    [HttpGet]
    public async Task<IActionResult> GetImages()
    {
        IEnumerable<ImageDto> images = await Mediator.Send(new GetImagesQuery());
        return Ok(images);
    }

    [HttpGet("Blob/{id}")]
    public async Task<IActionResult> GetImage(long id)
    {
        BlobData blob = await Mediator.Send(new GetImageBlobQuery(id));
        return File(blob.Content, blob.ContentType, blob.Name);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetImageBlob(long id)
    {
        ImageDto image = await Mediator.Send(new GetImageQuery(id));
        return Ok(image);
    }

    [HttpPost]
    public async Task<IActionResult> CreateImage([FromForm] CreateImageCommand command)
    {
        ImageDto image = await Mediator.Send(command);
        return Ok(image);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteImage(long id)
    {
        await Mediator.Send(new DeleteImageCommand(id));
        return NoContent();
    }

    [HttpPatch("[action]")]
    public async Task<IActionResult> ChangeGallery(ChangeImageGalleryCommand command)
    {
        await Mediator.Send(command);
        return NoContent();
    }
}