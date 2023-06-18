using ImageMicroService.Application.Common.Models;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;

namespace ImageMicroService.Application.Common.Extensions;

public static class BlobServiceClientExtensions
{
    public static async Task<BlobData> GetBlobAsync(this BlobServiceClient storage, string container, string file)
    {
        BlobContainerClient containerClient = storage.GetBlobContainerClient(container);
        BlobClient blobClient = containerClient.GetBlobClient(file);
        Response<BlobDownloadInfo> blob = await blobClient.DownloadAsync();
        return new BlobData(blob, file);
    }

    public static async Task UploadBlobAsync(this BlobServiceClient storage, string container, IFormFile file)
    {
        await UploadBlobAsync(storage, container, file, file.FileName);
    }

    public static async Task UploadBlobAsync(this BlobServiceClient storage, string container, IFormFile file, string fileName)
    {
        BlobContainerClient containerClient = storage.GetBlobContainerClient(container);
        await containerClient.UploadBlobAsync(fileName, file.OpenReadStream());
    }

    public static async Task DeleteBlobAsync(this BlobServiceClient storage, string container, string file)
    {
        BlobContainerClient containerClient = storage.GetBlobContainerClient(container);
        await containerClient.DeleteBlobAsync(file);
    }
}