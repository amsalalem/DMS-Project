using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class GetScreenQueryHandler : IRequestHandler<GetScreenQuery, ScreenDto>
    {
        private readonly IScreenRepository _screenRepository;
        private readonly IMapper _mapper;

        public GetScreenQueryHandler(
            IScreenRepository screenRepository,
            IMapper mapper)
        {
            _screenRepository = screenRepository;
            _mapper = mapper;

        }
        public async Task<ScreenDto> Handle(GetScreenQuery request, CancellationToken cancellationToken)
        {
            var entity = await _screenRepository.FindAsync(request.Id);
            if (entity != null)
                return _mapper.Map<ScreenDto>(entity);
            else
                return new ScreenDto
                {
                    StatusCode = 404,
                    Messages = new List<string> { "Not Found" }
                };
        }
    }
}
