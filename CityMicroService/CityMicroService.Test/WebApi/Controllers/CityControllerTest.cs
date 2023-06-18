namespace CityMicroService.Test.WebApi.Controllers;

using CityMicroService.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using static CityMicroService.Test.BLL.Services.CityServiceTestHelper;

public class CityControllerTest
{
    protected readonly Mock<ICityService> service;

    protected readonly CityController controller;

    public CityControllerTest()
    {
        service = new();

        controller = new(service.Object);
    }

    [Fact]
    public async Task GetCities_Success_ReturnOkCitiesDTOs()
    {
        service.Setup(prop => prop.GetAllAsync()).ReturnsAsync(GetCitiesDTOs());

        IActionResult result = await controller.GetCities();

        var okObjectResult = result as OkObjectResult;

        Assert.IsAssignableFrom<IEnumerable<CityDTO>>(okObjectResult!.Value);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetCity_Success_ReturnOkCityDTO()
    {
        service.Setup(prop => prop.GetByIdAsync(It.IsAny<long>())).ReturnsAsync(GetCitiesDTOs().First());

        IActionResult result = await controller.GetCity(It.IsAny<long>());

        var okObjectResult = result as OkObjectResult;

        Assert.IsAssignableFrom<CityDTO>(okObjectResult!.Value);
        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetCity_ArgumentException_ReturnNotFound()
    {
        service.Setup(prop => prop.GetByIdAsync(It.IsAny<long>())).Throws<ArgumentException>();

        IActionResult result = await controller.GetCity(It.IsAny<long>());

        var notFoundObjectResult = result as NotFoundObjectResult;

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task CreateCity_Success_ReturnCreatedAtActionCityDTO()
    {
        service.Setup(prop => prop.CreateAsync(It.IsAny<CityRequestDTO>())).ReturnsAsync(GetCitiesDTOs().First());

        IActionResult result = await controller.CreateCity(It.IsAny<CityRequestDTO>());
        var createdAtActionResult = result as CreatedAtActionResult;

        Assert.IsAssignableFrom<CityDTO>(createdAtActionResult!.Value);
        Assert.IsType<CreatedAtActionResult>(result);
    }

    [Fact]
    public async Task CreateCity_IsNotValid_ReturnBadRequest()
    {
        controller.ModelState.AddModelError(string.Empty, string.Empty);

        IActionResult result = await controller.CreateCity(It.IsAny<CityRequestDTO>());

        Assert.IsType<BadRequestObjectResult>(result);
    }    
    
    [Fact]
    public async Task UpdateCity_Success_ReturnCreatedAtActionCityDTO()
    {
        service.Setup(prop => prop.UpdateAsync(It.IsAny<long>(), It.IsAny<CityRequestDTO>())).ReturnsAsync(GetCitiesDTOs().First());

        IActionResult result = await controller.UpdateCity(It.IsAny<long>(), It.IsAny<CityRequestDTO>());
        var createdAtActionResult = result as CreatedAtActionResult;

        Assert.IsAssignableFrom<CityDTO>(createdAtActionResult!.Value);
        Assert.IsType<CreatedAtActionResult>(result);
    }    
    
    [Fact]
    public async Task UpdateCity_ArgumentException_ReturnNotFound()
    {
        service.Setup(prop => prop.UpdateAsync(It.IsAny<long>(), It.IsAny<CityRequestDTO>())).Throws<ArgumentException>();

        IActionResult result = await controller.UpdateCity(It.IsAny<long>(), It.IsAny<CityRequestDTO>());

        Assert.IsType<NotFoundObjectResult>(result);
    }    

    [Fact]
    public async Task DeleteCity_Success_ReturnNoContent()
    {
        service.Setup(prop => prop.DeleteAsync(It.IsAny<long>()));

        IActionResult result = await controller.DeleteCity(It.IsAny<long>());

        Assert.IsType<NoContentResult>(result);
    }
    
    [Fact]
    public async Task DeleteCity_ArgumentException_ReturnNotFound()
    {
        service.Setup(prop => prop.DeleteAsync(It.IsAny<long>())).Throws<ArgumentException>();

        IActionResult result = await controller.DeleteCity(It.IsAny<long>());

        Assert.IsType<NotFoundObjectResult>(result);
    }
}
