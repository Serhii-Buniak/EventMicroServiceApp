using CityMicroService.BLL.Publishers;
using System.Linq.Expressions;
using static CityMicroService.Test.BLL.Services.CityServiceTestHelper;
using static CityMicroService.Test.BLL.Services.CountryServiceTestHelper;

namespace CityMicroService.Test.BLL.Services;

public class CityServiceTest
{
    protected readonly Mock<IRepositoryWrapper> repoWrapper;
    protected readonly Mock<ICityRepository> cityRepo;
    protected readonly Mock<ICountryRepository> countryRepo;
    protected readonly Mock<IMapper> mapper;
    protected readonly Mock<ICacheService> cache;
    protected readonly Mock<ICityPublisher> publisher;

    protected CityService cityService;


    public CityServiceTest()
    {
        repoWrapper = new();
        cityRepo = new();
        countryRepo = new();
        mapper = new();
        cache = new();
        publisher = new();

        repoWrapper.Setup(prop => prop.CityRepository).Returns(cityRepo.Object);
        repoWrapper.Setup(prop => prop.CountryRepository).Returns(countryRepo.Object);

        cityService = new(
           repositoryWrapper: repoWrapper.Object,
           mapper: mapper.Object,
           cache: cache.Object,
           publisher: publisher.Object
        );
    }

    [Fact]
    public async Task GetAllAsync_WasCached_ReturnCityDtos()
    {
        cache.Setup(prop => prop.GetCities()).Returns(GetCities);
        mapper.Setup(prop => prop.Map<IEnumerable<CityDTO>>(It.IsAny<IEnumerable<City>>())).Returns(GetCitiesDTOs);

        IEnumerable<CityDTO> cityDTOs = await cityService.GetAllAsync();

        Assert.Equal(GetCitiesDTOs().Count(), cityDTOs.Count());
    }

    [Fact]
    public async Task GetAllAsync_NotCached_ReturnCityDtos()
    {
        cache.Setup(prop => prop.GetCities()).Returns(() => null);
        mapper.Setup(prop => prop.Map<IEnumerable<CityDTO>>(It.IsAny<IEnumerable<City>>())).Returns(GetCitiesDTOs);
        cityRepo.Setup(prop => prop.GetAllAsync(null)).ReturnsAsync(GetCities);

        IEnumerable<CityDTO> cityDTOs = await cityService.GetAllAsync();

        Assert.Equal(GetCitiesDTOs().Count(), cityDTOs.Count());
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task GetByIdAsync_Success_ReturnCityDto(long id)
    {
        cityRepo.Setup(prop => prop.SingleOrDefaultAsync(p => p.Id == id)).ReturnsAsync(GetCityById(id));
        mapper.Setup(prop => prop.Map<CityDTO>(It.IsAny<City>())).Returns(GetCityDTOById(id)!);

        CityDTO cityDTO = await cityService.GetByIdAsync(id);

        Assert.Equal(cityDTO.Id, id);
    }

    [Theory]
    [InlineData(4)]
    [InlineData(long.MaxValue)]
    public async Task GetByIdAsync_CityNotFound_ThrowArgumentException(long id)
    {
        cityRepo.Setup(prop => prop.SingleOrDefaultAsync(p => p.Id == id)).ReturnsAsync(GetCityById(id));

        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            await cityService.GetByIdAsync(id);
        });
    }

    [Fact]
    public async Task CreateAsync_Success_ReturnCityDto()
    {
        City city = GetCities().First();
        CityDTO cityDto = GetCitiesDTOs().First();

        mapper.Setup(prop => prop.Map<City>(It.IsAny<CityRequestDTO>())).Returns(city);
        cityRepo.Setup(prop => prop.CreateAsync(It.IsAny<City>()));
        repoWrapper.Setup(prop => prop.SaveAsync());
        cityRepo.Setup(prop => prop.SingleOrDefaultAsync(It.IsAny<Expression<Func<City, bool>>?>())).ReturnsAsync(city);
        mapper.Setup(prop => prop.Map<CityDTO>(It.IsAny<City>())).Returns(() => cityDto);
        publisher.Setup(prop => prop.CreateEvent(cityDto));

        CityDTO createdCity = await cityService.CreateAsync(It.IsAny<CityRequestDTO>());

        Assert.True(city.Id == createdCity.Id && createdCity.Id == cityDto.Id);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task DeleteAsync_Success_ReturnCityDto(long id)
    {
        cityRepo.Setup(prop => prop.SingleOrDefaultAsync(p => p.Id == id)).ReturnsAsync(GetCityById(id));
        cityRepo.Setup(prop => prop.Delete(It.IsAny<City>()));
        repoWrapper.Setup(prop => prop.SaveAsync());
        mapper.Setup(prop => prop.Map<CityDTO>(It.IsAny<City>())).Returns(GetCityDTOById(id)!);
        publisher.Setup(prop => prop.DeleteEvent(It.IsAny<CityDTO>()));

        CityDTO deletedCity = await cityService.DeleteAsync(id);

        Assert.Equal(deletedCity.Id, id);
    }    
    
    [Theory]
    [InlineData(4)]
    [InlineData(long.MaxValue)]
    public async Task DeleteAsync_CityNotFound_ThrowArgumentException(long id)
    {
        cityRepo.Setup(prop => prop.SingleOrDefaultAsync(p => p.Id == id)).ReturnsAsync(GetCityById(id));

        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            CityDTO deletedCity = await cityService.DeleteAsync(id);
        });
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 2)]
    public async Task UpdateAsync_Success_ReturnCityDto(long cityId, long countryId)
    {
        CityRequestDTO cityRequest = new() { CountryId = countryId };

        cityRepo.Setup(prop => prop.SingleOrDefaultAsync(It.IsAny<Expression<Func<City, bool>>?>())).ReturnsAsync(GetCityById(cityId));
        countryRepo.Setup(prop => prop.SingleOrDefaultAsync(It.IsAny<Expression<Func<Country, bool>>?>())).ReturnsAsync(GetCountryById(countryId));
        cityRepo.Setup(prop => prop.Update(It.IsAny<City>()));
        repoWrapper.Setup(prop => prop.SaveAsync());
        publisher.Setup(prop => prop.UpdateEvent(It.IsAny<CityDTO>()));
        mapper.Setup(prop => prop.Map<CityDTO>(It.IsAny<City>())).Returns(GetCityDTOById(cityId)!);
        mapper.Setup(prop => prop.Map(It.IsAny<CityRequestDTO>(), It.IsAny<City>())).Returns(GetCityById(cityId)!);

        CityDTO cityDto = await cityService.UpdateAsync(cityId, cityRequest);

        Assert.Equal(cityId, cityDto.Id);
    }

    [Theory]
    [InlineData(4, 1)]
    [InlineData(long.MaxValue, 2)]
    public async Task UpdateAsync_CityNotFound_ThrowArgumentException(long cityId, long countryId)
    {
        CityRequestDTO cityRequest = new() { CountryId = countryId };

        cityRepo.Setup(prop => prop.SingleOrDefaultAsync(It.IsAny<Expression<Func<City, bool>>?>())).ReturnsAsync(GetCityById(cityId));

        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            CityDTO cityDto = await cityService.UpdateAsync(cityId, cityRequest);
        });
    }   
    
    [Theory]
    [InlineData(1, 4)]
    [InlineData(2, long.MaxValue)]
    public async Task UpdateAsync_CountryNotFound_ThrowArgumentException(long cityId, long countryId)
    {
        CityRequestDTO cityRequest = new() { CountryId = countryId };

        cityRepo.Setup(prop => prop.SingleOrDefaultAsync(It.IsAny<Expression<Func<City, bool>>?>())).ReturnsAsync(GetCityById(cityId));
        countryRepo.Setup(prop => prop.SingleOrDefaultAsync(It.IsAny<Expression<Func<Country, bool>>?>())).ReturnsAsync(GetCountryById(countryId));
        mapper.Setup(prop => prop.Map(It.IsAny<CityRequestDTO>(), It.IsAny<City>())).Returns(GetCityById(cityId)!);

        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            CityDTO cityDto = await cityService.UpdateAsync(cityId, cityRequest);
        });
    }
}
