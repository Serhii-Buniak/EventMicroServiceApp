using ImageMicroService.Application.Common.Exceptions;
using ImageMicroService.Application.Common.Interfaces;
using AutoMapper;
using ImageMicroService.Domain.Entities;
using MediatR;
using ImageMicroService.Application.Common.Models;

namespace ImageMicroService.Application.ImageActions.Commands;

public class DeleteImageCommand : IRequest
{
    public DeleteImageCommand(long id)
    {
        Id = id;
    }

    public long Id { get; set; }

    public class DeleteImageCommandHandler : IRequestHandler<DeleteImageCommand>
    {
        private readonly IApplicationDbContext _dataContext;
        private readonly IImageBlobService _blobService;
        private readonly IMapper _mapper;
        private readonly IImagePublisher _publisher;

        public DeleteImageCommandHandler(IApplicationDbContext applicationDbContext, IImageBlobService imageBlobService, IMapper mapper, IImagePublisher publisher)
        {
            _dataContext = applicationDbContext;
            _blobService = imageBlobService;
            _mapper = mapper;
            _publisher = publisher;
        }

        public async Task<Unit> Handle(DeleteImageCommand request, CancellationToken cancellationToken)
        {
            Image? image = await _dataContext.Images.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);

            if (image is null)
            {
                throw new NotFoundException(nameof(Image), request.Id);
            }

            try
            {
                await _blobService.DeleteAsync(image.Id);
            }
            finally
            {
                _dataContext.Images.Remove(image);
                await _dataContext.SaveChangesAsync(cancellationToken);

                var imageDto = _mapper.Map<ImageDto>(image);

                _publisher.DeleteEvent(imageDto);
            }

            return Unit.Value;
        }
    }
}