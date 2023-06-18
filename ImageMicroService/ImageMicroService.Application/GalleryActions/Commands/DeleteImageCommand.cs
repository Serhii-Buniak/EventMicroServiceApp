using ImageMicroService.Application.Common.Exceptions;
using ImageMicroService.Application.Common.Interfaces;
using ImageMicroService.Domain.Entities;
using MediatR;

namespace ImageMicroService.Application.GalleryActions.Commands;

public class DeleteGalleryCommand : IRequest
{
    public DeleteGalleryCommand(long id)
    {
        Id = id;
    }

    public long Id { get; set; }

    public class DeleteGalleryCommandHandler : IRequestHandler<DeleteGalleryCommand>
    {
        private readonly IApplicationDbContext _dataContext;

        public DeleteGalleryCommandHandler(IApplicationDbContext applicationDbContext)
        {
            _dataContext = applicationDbContext;
        }

        public async Task<Unit> Handle(DeleteGalleryCommand request, CancellationToken cancellationToken)
        {
            Gallery? gallery = await _dataContext.Galleries.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);

            if (gallery is null)
            {
                throw new NotFoundException(nameof(Gallery), request.Id);
            }

            _dataContext.Galleries.Remove(gallery);
            await _dataContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}