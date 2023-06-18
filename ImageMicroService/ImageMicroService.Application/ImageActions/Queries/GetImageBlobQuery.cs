using ImageMicroService.Application.Common.Interfaces;
using ImageMicroService.Application.Common.Models;
using MediatR;

namespace ImageMicroService.Application.ImageActions.Queries;

public class GetImageBlobQuery : IRequest<BlobData>
{
    public GetImageBlobQuery(long id)
    {
        Id = id;
    }

    public long Id { get; set; }

    public class GetImageBlobQueryHandler : IRequestHandler<GetImageBlobQuery, BlobData>
    {
        private readonly IImageBlobService _blobService;

        public GetImageBlobQueryHandler(IImageBlobService blobService)
        {
            _blobService = blobService;
        }

        public async Task<BlobData> Handle(GetImageBlobQuery request, CancellationToken cancellationToken)
        {
            return await _blobService.GetByIdAsync(request.Id);
        }
    }
}