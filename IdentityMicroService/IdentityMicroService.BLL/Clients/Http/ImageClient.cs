using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using IdentityMicroService.BLL.DAL.Data;

namespace IdentityMicroService.BLL.Clients.Http;

public class ImageClient : IImageClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public ImageClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<Image?> CreateImage(IFormFile file)
    {
        using MultipartFormDataContent form = new();
        using StreamContent streamContent = new(file.OpenReadStream());
        using ByteArrayContent content = new(await streamContent.ReadAsByteArrayAsync());

        content.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

        form.Add(content, "file", file.FileName);

        using HttpResponseMessage response = await _httpClient.PostAsync(_configuration["ImageEndpoint"], form);

        string apiResponse = response.Content.ReadAsStringAsync().Result;

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("-->> HttpResponseMessage PostAsync OK");
        }
        else
        {
            Console.WriteLine("-->> HttpResponseMessage PostAsync NOT OK");
        }

        var image = JsonConvert.DeserializeObject<Image>(apiResponse);
        return image;
    }
}