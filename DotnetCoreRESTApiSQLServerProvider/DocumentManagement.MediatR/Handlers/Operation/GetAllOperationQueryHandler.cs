using AutoMapper;
using DocumentManagement.Data.Dto;
using DocumentManagement.MediatR.Queries;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class GetAllOperationQueryHandler : IRequestHandler<GetAllOperationQuery, List<OperationDto>>
    {
        private readonly IOperationRepository _operationRepository;
        private readonly IMapper _mapper;

        public GetAllOperationQueryHandler(
            IOperationRepository operationRepository,
            IMapper mapper)
        {
            _operationRepository = operationRepository;
            _mapper = mapper;

        }
        public async Task<List<OperationDto>> Handle(GetAllOperationQuery request, CancellationToken cancellationToken)
        {
            var entities = await _operationRepository.All.ToListAsync();
            return _mapper.Map<List<OperationDto>>(entities);
        }
    }
}
