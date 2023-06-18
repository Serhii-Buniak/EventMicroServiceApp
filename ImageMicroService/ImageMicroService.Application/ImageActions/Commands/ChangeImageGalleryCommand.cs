using ImageMicroService.Application.Common.Exceptions;
using ImageMicroService.Application.Common.Interfaces;
using ImageMicroService.Application.Common.Models;
using AutoMapper;
using ImageMicroService.Domain.Entities;
using MediatR;

namespace ImageMicroService.Application.ImageActions.Commands;

public class ChangeImageGalleryCommand : IRequest<ImageDto>
{
    public long Id { get; set; }
    public long? GalleryId { get; set; }

    public class ChangeImageGalleryCommandHandler : IRequestHandler<ChangeImageGalleryCommand, ImageDto>
    {
        private readonly IApplicationDbContext _dataContext;
        private readonly IMapper _mapper;

        public ChangeImageGalleryCommandHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _dataContext = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<ImageDto> Handle(ChangeImageGalleryCommand request, CancellationToken cancellationToken)
        {
            Image? image = await _dataContext.Images.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);
            if (image is null)
            {
                throw new NotFoundException(nameof(Image), request.Id);
            }

            if (request.GalleryId.HasValue)
            {
                Gallery? gallery = await _dataContext.Galleries.FindAsync(new object[] { request.GalleryId.Value }, cancellationToken: cancellationToken);
                if (gallery is null)
                {
                    throw new NotFoundException(nameof(Gallery), request.GalleryId.Value);
                }
            }

            image.GalleryId = request.GalleryId;
            await _dataContext.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ImageDto>(image); ;
        }
    }
}