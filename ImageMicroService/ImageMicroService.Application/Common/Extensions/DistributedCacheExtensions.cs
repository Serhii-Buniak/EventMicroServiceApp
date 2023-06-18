using ImageMicroService.Application.Common.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace ImageMicroService.Application.Common.Extensions;

public static class DistributedCacheExtensions
{
    public const string GalleriesKey = "Galleries";

    public static async Task SetRecordAsync<T>(this IDistributedCache cache,
        string redordId,
        T data,
        TimeSpan? absoluteExpireTime = null,
        TimeSpan? unusedExpireTime = null)
    {
        DistributedCacheEntryOptions options = new()
        {
            AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? TimeSpan.FromSeconds(60),
            SlidingExpiration = unusedExpireTime
        };
        var jsonData = JsonSerializer.Serialize(data);
        await cache.SetStringAsync(redordId, jsonData, options);
    }

    public static async Task<T?> GetRedordAsync<T>(this IDistributedCache cache, string recordId)
    {
        var jsonData = await cache.GetStringAsync(recordId);

        if (jsonData is null)
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(jsonData);
    }

    public static async Task SetGalleriesAsync(this IDistributedCache cache, IEnumerable<GalleryDto> data)
    {
        await cache.SetRecordAsync(GalleriesKey, data);
    }

    public static async Task<IEnumerable<GalleryDto>?> GetGalleriesAsync(this IDistributedCache cache)
    {
        return await cache.GetRedordAsync<IEnumerable<GalleryDto>>(GalleriesKey);
    }
}