using ImageMicroService.Application.Common.Exceptions;
using ImageMicroService.Application.Common.Interfaces;
using ImageMicroService.Application.Common.Mappings;
using ImageMicroService.Application.Common.Models;
using AutoMapper;
using ImageMicroService.Domain.Entities;
using MediatR;

namespace ImageMicroService.Application.ImageActions.Queries;

public class GetImageQuery : IRequest<ImageDto>
{
    public GetImageQuery(long id)
    {
        Id = id;
    }

    public long Id { get; set; }

    public class GetImageQueryHandler : IRequestHandler<GetImageQuery, ImageDto>
    {
        private readonly IApplicationDbContext _dataContext;
        private readonly IMapper _mapper;

        public GetImageQueryHandler(IApplicationDbContext applicationDbContext, IMapper mapper)
        {
            _dataContext = applicationDbContext;
            _mapper = mapper;
        }

        public async Task<ImageDto> Handle(GetImageQuery request, CancellationToken cancellationToken)
        {
            Image? image = await _dataContext.Images.FindAsync(new object[] { request.Id }, cancellationToken: cancellationToken);

            if (image is null)
            {
                throw new NotFoundException(nameof(Image), request.Id);
            }

            return _mapper.Map<ImageDto>(image); ;
        }
    }
}