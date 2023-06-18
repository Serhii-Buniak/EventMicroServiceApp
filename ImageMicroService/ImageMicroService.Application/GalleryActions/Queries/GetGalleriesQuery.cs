using ImageMicroService.Application.Common.Extensions;
using ImageMicroService.Application.Common.Interfaces;
using ImageMicroService.Application.Common.Mappings;
using ImageMicroService.Application.Common.Models;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace ImageMicroService.Application.GalleryActions.Queries;

public class GetGalleriesQuery : IRequest<IEnumerable<GalleryDto>>
{
    public class GetGalleriesQueryHandler : IRequestHandler<GetGalleriesQuery, IEnumerable<GalleryDto>>
    {
        private readonly IApplicationDbContext _dataContext;
        private readonly IMapper _mapper;
        private readonly IDistributedCache _cache;

        public GetGalleriesQueryHandler(IApplicationDbContext applicationDbContext, IMapper mapper, IDistributedCache cache)
        {
            _dataContext = applicationDbContext;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<IEnumerable<GalleryDto>> Handle(GetGalleriesQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<GalleryDto>? galleryDtos = await _cache.GetGalleriesAsync();

            if (galleryDtos is not null)
            {
                return galleryDtos;
            }

            galleryDtos = await _dataContext.Galleries
              .Include(g => g.Images)
              .ProjectToListAsync<GalleryDto>(_mapper.ConfigurationProvider);

            await _cache.SetGalleriesAsync(galleryDtos);
            await Task.Delay(2000);

            return galleryDtos;
        }
    }
}