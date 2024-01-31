using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class GetScreenOperationQueryHandler : IRequestHandler<GetScreenOperationQuery, ScreenOperationDto>
    {
        private readonly IScreenOperationRepository _screenOperationRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetScreenOperationQueryHandler> _logger;

        public GetScreenOperationQueryHandler(
         IScreenOperationRepository screenOperationRepository,
          IMapper mapper,
          ILogger<GetScreenOperationQueryHandler> logger)
        {
            _screenOperationRepository = screenOperationRepository;
            _mapper = mapper;
            _logger = logger;

        }

        public async Task<ScreenOperationDto> Handle(GetScreenOperationQuery request, CancellationToken cancellationToken)
        {
            var entity = await _screenOperationRepository.FindAsync(request.Id);
            if (entity != null)
                return _mapper.Map<ScreenOperationDto>(entity);
            else
            {
                var entityDto = new ScreenOperationDto
                {
                    StatusCode = 404,
                    Messages = new List<string> { "Not Found Role" }
                };
                _logger.LogWarning("Not Found Role", entityDto);
                return entityDto;
            }
        }
    }
}
