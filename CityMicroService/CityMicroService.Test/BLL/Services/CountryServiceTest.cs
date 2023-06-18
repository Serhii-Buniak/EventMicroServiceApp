using CityMicroService.BLL.Publishers;
using System.Linq.Expressions;
using static CityMicroService.Test.BLL.Services.CountryServiceTestHelper;

namespace CityMicroService.Test.BLL.Services;

public class CountryServiceTest
{
    protected readonly Mock<IRepositoryWrapper> repoWrapper;
    protected readonly Mock<ICityRepository> cityRepo;
    protected readonly Mock<ICountryRepository> countryRepo;
    protected readonly Mock<IMapper> mapper;
    protected readonly Mock<ICacheService> cache;
    protected readonly Mock<ICountryPublisher> publisher;

    protected CountryService countryService;

    public CountryServiceTest()
    {
        repoWrapper = new();
        cityRepo = new();
        countryRepo = new();
        mapper = new();
        cache = new();
        publisher = new();

        repoWrapper.Setup(prop => prop.CityRepository).Returns(cityRepo.Object);
        repoWrapper.Setup(prop => prop.CountryRepository).Returns(countryRepo.Object);

        countryService = new(
           repositoryWrapper: repoWrapper.Object,
           mapper: mapper.Object,
           cache: cache.Object,
           publisher: publisher.Object
        );
    }

    [Fact]
    public async Task GetAllAsync_WasCached_ReturnCountriesDTOs()
    {
        cache.Setup(prop => prop.GetCountries()).Returns(GetCountries());
        mapper.Setup(prop => prop.Map<IEnumerable<CountryDTO>>(It.IsAny<IEnumerable<Country>>())).Returns(GetCountriesDTOs());

        IEnumerable<CountryDTO> countries = await countryService.GetAllAsync();

        Assert.Equal(countries.Count(), GetCountries().Count());
    }

    [Fact]
    public async Task GetAllAsync_NotCached_ReturnCountriesDTOs()
    {
        cache.Setup(prop => prop.GetCountries()).Returns(() => null);
        countryRepo.Setup(prop => prop.GetAllAsync(null)).ReturnsAsync(GetCountries());
        cache.Setup(prop => prop.SetCountries(It.IsAny<IEnumerable<Country>>(), It.IsAny<int>()));
        mapper.Setup(prop => prop.Map<IEnumerable<CountryDTO>>(It.IsAny<IEnumerable<Country>>())).Returns(GetCountriesDTOs());

        IEnumerable<CountryDTO> countries = await countryService.GetAllAsync();

        Assert.Equal(countries.Count(), GetCountries().Count());
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task GetByIdAsync_Success_ReturnCountryDTO(long id)
    {
        countryRepo.Setup(prop => prop.SingleOrDefaultAsync(It.IsAny<Expression<Func<Country, bool>>>())).ReturnsAsync(GetCountryById(id));
        mapper.Setup(prop => prop.Map<CountryDTO>(It.IsAny<Country>())).Returns(GetCountryDTOById(id)!);

        CountryDTO createdCountry = await countryService.GetByIdAsync(id);

        Assert.Equal(id, createdCountry.Id);
    }

    [Theory]
    [InlineData(4)]
    [InlineData(long.MaxValue)]
    public async Task GetByIdAsync_CountryNotFound_ThrowArgumentException(long id)
    {
        countryRepo.Setup(prop => prop.SingleOrDefaultAsync(It.IsAny<Expression<Func<Country, bool>>>())).ReturnsAsync(GetCountryById(id));
        mapper.Setup(prop => prop.Map<CountryDTO>(It.IsAny<Country>())).Returns(GetCountryDTOById(id)!);

        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            CountryDTO createdCountry = await countryService.GetByIdAsync(id);
        });
    }

    [Fact]
    public async Task CreateAsync_Success_ReturnCountryDTO()
    {
        Country country = GetCountries().First();
        CountryDTO countryDto = GetCountriesDTOs().First();

        mapper.Setup(prop => prop.Map<Country>(It.IsAny<CountryRequestDTO>())).Returns(country);
        countryRepo.Setup(prop => prop.CreateAsync(country));
        repoWrapper.Setup(prop => prop.SaveAsync());
        countryRepo.Setup(prop => prop.SingleOrDefaultAsync(It.IsAny<Expression<Func<Country, bool>>>())).ReturnsAsync(country);
        mapper.Setup(prop => prop.Map<CountryDTO>(country)).Returns(countryDto);
        publisher.Setup(prop => prop.CreateEvent(countryDto));

        CountryDTO createdCountry = await countryService.CreateAsync(It.IsAny<CountryRequestDTO>());

        Assert.Equal(country.Id, createdCountry.Id);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task DeleteAsync_Success_ReturnCountryDTO(long id)
    {
        countryRepo.Setup(prop => prop.SingleOrDefaultAsync(It.IsAny<Expression<Func<Country, bool>>>())).ReturnsAsync(GetCountryById(id));
        countryRepo.Setup(prop => prop.Delete(It.IsAny<Country>()));
        repoWrapper.Setup(prop => prop.SaveAsync());
        mapper.Setup(prop => prop.Map<CountryDTO>(It.IsAny<Country>())).Returns(GetCountryDTOById(id)!);
        publisher.Setup(prop => prop.DeleteEvent(It.IsAny<CountryDTO>()));

        CountryDTO deletedCountry = await countryService.DeleteAsync(id);

        Assert.Equal(id, deletedCountry.Id);
    }

    [Theory]
    [InlineData(4)]
    [InlineData(long.MaxValue)]
    public async Task DeleteAsync_CountryNotFound_ThrowArgumentException(long id)
    {
        countryRepo.Setup(prop => prop.SingleOrDefaultAsync(It.IsAny<Expression<Func<Country, bool>>>())).ReturnsAsync(GetCountryById(id));

        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            CountryDTO deletedCountry = await countryService.DeleteAsync(id);
        });
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task UpdateAsync_Success_ReturnCountryDTO(long id)
    {
        countryRepo.Setup(prop => prop.SingleOrDefaultAsync(It.IsAny<Expression<Func<Country, bool>>>())).ReturnsAsync(GetCountryById(id));
        mapper.Setup(prop => prop.Map(It.IsAny<CountryRequestDTO>(), It.IsAny<Country>())).Returns(GetCountryById(id)!);
        countryRepo.Setup(prop => prop.Update(It.IsAny<Country>()));
        repoWrapper.Setup(prop => prop.SaveAsync());
        mapper.Setup(prop => prop.Map<CountryDTO>(It.IsAny<Country>())).Returns(GetCountryDTOById(id)!);
        publisher.Setup(prop => prop.UpdateEvent(It.IsAny<CountryDTO>()));

        CountryDTO updatedCountry = await countryService.UpdateAsync(id, It.IsAny<CountryRequestDTO>());

        Assert.Equal(id, updatedCountry.Id);
    }    
    
    [Theory]
    [InlineData(4)]
    [InlineData(long.MaxValue)]
    public async Task UpdateAsync_CountryNotFound_ThrowArgumentException(long id)
    {
        countryRepo.Setup(prop => prop.SingleOrDefaultAsync(It.IsAny<Expression<Func<Country, bool>>>())).ReturnsAsync(GetCountryById(id));
       
        await Assert.ThrowsAsync<ArgumentException>(async () =>
        {
            CountryDTO updatedCountry = await countryService.UpdateAsync(id, It.IsAny<CountryRequestDTO>());
        });
    }
}