using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class GetAllScreenOperationQueryHandler : IRequestHandler<GetAllScreenOperationQuery, List<ScreenOperationDto>>
    {
        private readonly IScreenOperationRepository _screenOperationRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAllScreenOperationQueryHandler> _logger;

        public GetAllScreenOperationQueryHandler(
          IScreenOperationRepository screenOperationRepository,
            IMapper mapper,
            ILogger<GetAllScreenOperationQueryHandler> logger)
        {
            _screenOperationRepository = screenOperationRepository;
            _mapper = mapper;
            _logger = logger;

        }

        public async Task<List<ScreenOperationDto>> Handle(GetAllScreenOperationQuery request, CancellationToken cancellationToken)
        {
            var entities = await _screenOperationRepository.All.ToListAsync();
            return _mapper.Map<List<ScreenOperationDto>>(entities);
        }
    }
}
