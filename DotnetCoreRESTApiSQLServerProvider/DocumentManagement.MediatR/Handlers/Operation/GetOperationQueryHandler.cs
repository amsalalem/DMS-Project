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
    public class GetOperationQueryHandler : IRequestHandler<GetOperationQuery, OperationDto>
    {
        private readonly IOperationRepository _operationRepository;
        private readonly IMapper _mapper;

        public GetOperationQueryHandler(
           IOperationRepository operationRepository,
           IMapper mapper)
        {
            _operationRepository = operationRepository;
            _mapper = mapper;
        }

        public async Task<OperationDto> Handle(GetOperationQuery request, CancellationToken cancellationToken)
        {
            var entity = await _operationRepository.FindAsync(request.Id);
            if (entity != null)
                return _mapper.Map<OperationDto>(entity);
            else
                return new OperationDto
                {
                    StatusCode = 404,
                    Messages = new List<string> { "Operation is not found." }
                };
        }
    }
}
