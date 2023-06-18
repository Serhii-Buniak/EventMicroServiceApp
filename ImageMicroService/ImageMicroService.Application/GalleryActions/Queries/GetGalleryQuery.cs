using ImageMicroService.Application.Common.Exceptions;
using ImageMicroService.Application.Common.Interfaces;
using ImageMicroService.Application.Common.Models;
using AutoMapper;
using ImageMicroService.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ImageMicroService.Application.GalleryActions.Queries;

public class GetGalleryQuery : IRequest<GalleryDto>
{
    public GetGalleryQuery(long id)
    {
        Id = id;
    }

    public long Id { get; set; }

    public class GetGalleryQueryHandler : IRequestHandler<GetGalleryQuery, GalleryDto>
    {
        private readonly IApplicationDbContext _dataContext;
        private readonly IMapper _mapper;

        public GetGalleryQueryHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _dataContext = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<GalleryDto> Handle(GetGalleryQuery request, CancellationToken cancellationToken)
        {
            Gallery? gallery = await _dataContext.Galleries
                .Include(g => g.Images)
                .FirstOrDefaultAsync(g => g.Id == request.Id, cancellationToken);

            if (gallery is null)
            {
                throw new NotFoundException(nameof(Gallery), request.Id);
            }

            return _mapper.Map<GalleryDto>(gallery);
        }
    }
}