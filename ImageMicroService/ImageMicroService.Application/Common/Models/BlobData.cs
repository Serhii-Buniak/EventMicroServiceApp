using Azure;
using Azure.Storage.Blobs.Models;

namespace ImageMicroService.Application.Common.Models;

public struct BlobData
{
    public BlobData(Stream content, string contentType, string name) : this()
    {
        Content = content;
        ContentType = contentType;
        Name = name;
    }

    public BlobData(Response<BlobDownloadInfo> response, string name) : this(response.Value.Content, response.Value.Details.ContentType, name)
    {

    }

    public Stream Content { get; set; }
    public string ContentType { get; set; }
    public string Name { get; set; }
}