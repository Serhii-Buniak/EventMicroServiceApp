using CityMicroService.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using static CityMicroService.Test.BLL.Services.CountryServiceTestHelper;

namespace CityMicroService.Test.WebApi.Controllers;

public class CountryControllerTest
{
    protected readonly Mock<ICountryService> service;

    protected readonly CountryController controller;

    public CountryControllerTest()
    {
        service = new();

        controller = new(service.Object);
    }

    [Fact]
    public async Task GetCities_Success_ReturnOkCountriesDTOs()
    {
        service.Setup(prop => prop.GetAllAsync()).ReturnsAsync(GetCountriesDTOs());

        IActionResult result = await controller.GetCities();
        var okObjectResult = result as OkObjectResult;

        Assert.IsType<OkObjectResult>(result);
        Assert.IsAssignableFrom<IEnumerable<CountryDTO>>(okObjectResult!.Value);
    }

    [Fact]
    public async Task GetCity_Success_ReturnOkCountryDTO()
    {
        service.Setup(prop => prop.GetByIdAsync(It.IsAny<long>())).ReturnsAsync(GetCountriesDTOs().First());

        IActionResult result = await controller.GetCity(It.IsAny<long>());
        var okObjectResult = result as OkObjectResult;

        Assert.IsType<OkObjectResult>(result);
        Assert.IsAssignableFrom<CountryDTO>(okObjectResult!.Value);
    }

    [Fact]
    public async Task GetCity_ArgumentException_ReturnNotFound()
    {
        service.Setup(prop => prop.GetByIdAsync(It.IsAny<long>())).Throws<ArgumentException>();

        IActionResult result = await controller.GetCity(It.IsAny<long>());

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task CreateCity_Success_ReturnCreatedAtActionCountryDTO()
    {
        service.Setup(prop => prop.CreateAsync(It.IsAny<CountryRequestDTO>())).ReturnsAsync(GetCountriesDTOs().First());

        IActionResult result = await controller.CreateCity(It.IsAny<CountryRequestDTO>());
        var createdAtActionResult = result as CreatedAtActionResult;

        Assert.IsType<CreatedAtActionResult>(result);
        Assert.IsAssignableFrom<CountryDTO>(createdAtActionResult!.Value);
    }

    [Fact]
    public async Task CreateCity_IsNotValid_ReturnBadRequest()
    {
        controller.ModelState.AddModelError(string.Empty, string.Empty);

        IActionResult result = await controller.CreateCity(It.IsAny<CountryRequestDTO>());

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task UpdateCity_Success_ReturnCreatedAtActionCountryDTO()
    {
        service.Setup(prop => prop.UpdateAsync(It.IsAny<long>(), It.IsAny<CountryRequestDTO>())).ReturnsAsync(GetCountriesDTOs().First());

        IActionResult result = await controller.UpdateCity(It.IsAny<long>(), It.IsAny<CountryRequestDTO>());
        var createdAtActionResult = result as CreatedAtActionResult;

        Assert.IsType<CreatedAtActionResult>(result);
        Assert.IsAssignableFrom<CountryDTO>(createdAtActionResult!.Value);
    }

    [Fact]
    public async Task UpdateCity_ArgumentException_ReturnNotFound()
    {
        service.Setup(prop => prop.UpdateAsync(It.IsAny<long>(), It.IsAny<CountryRequestDTO>())).Throws<ArgumentException>();

        IActionResult result = await controller.UpdateCity(It.IsAny<long>(), It.IsAny<CountryRequestDTO>());

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
