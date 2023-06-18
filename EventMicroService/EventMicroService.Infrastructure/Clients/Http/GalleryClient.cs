using EventMicroService.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Text;
using Newtonsoft.Json;
using System.Text.Json;
using EventMicroService.Application.Common.Interfaces;
using EventMicroService.Application.Common.Dtos;
using System.Net.Http.Headers;
using MediatR;
using Microsoft.Net.Http.Headers;
using System.Net.Http;

namespace EventMicroService.Infrastructure.Clients.Http;

public class GalleryClient : IGalleryClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private IHttpContextAccessor _accessor;

    public GalleryClient(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor accessor)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _accessor = accessor;
    }

    public async Task<Gallery?> CreateGalleryAsync(CreateGalleryDto createGallery)
    {
        string? token = _accessor?.HttpContext?.Request.Headers[HeaderNames.Authorization].ToString()[7..];

        Console.WriteLine(token);

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token ?? "");

        const string appJson = "application/json";
        
        StringContent content = new(
            content: System.Text.Json.JsonSerializer.Serialize(createGallery),
            encoding: Encoding.UTF8,
            mediaType: appJson
        );

        HttpResponseMessage response = await _httpClient.PostAsync(_configuration["GalleryEndpoint"], content);

        Console.WriteLine(response.StatusCode);
        Console.WriteLine(response.StatusCode);
        Console.WriteLine(response.StatusCode);
        string apiResponse = response.Content.ReadAsStringAsync().Result;

        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("-->> HttpResponseMessage PostAsync OK");
        }
        else
        {
            Console.WriteLine("-->> HttpResponseMessage PostAsync NOT OK");
        }

        var gallery = JsonConvert.DeserializeObject<Gallery>(apiResponse);

        return gallery;
    }
}
