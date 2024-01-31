using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class AddOperationCommandHandler : IRequestHandler<AddOperationCommand, OperationDto>
    {
        private readonly IOperationRepository _operationRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;
        private readonly IMapper _mapper;
        public AddOperationCommandHandler(
           IOperationRepository operationRepository,
            IMapper mapper,
            IUnitOfWork<DocumentContext> uow
            )
        {
            _operationRepository = operationRepository;
            _mapper = mapper;
            _uow = uow;
        }
        public async Task<OperationDto> Handle(AddOperationCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _operationRepository.FindBy(c => c.Name == request.Name).FirstOrDefaultAsync();
            if (entityExist != null)
            {
                var errorDto = new OperationDto
                {
                    StatusCode = 409,
                    Messages = new List<string> { "Operation already exist." }
                };
                return errorDto;
            }
            var entity = _mapper.Map<Operation>(request);
            entity.Id = Guid.NewGuid();
            _operationRepository.Add(entity);
            if (await _uow.SaveAsync() <= 0)
            {
                var errorDto = new OperationDto
                {
                    StatusCode = 500,
                    Messages = new List<string> { "An unexpected fault happened. Try again later." }
                };
                return errorDto;
            }
            var entityDto = _mapper.Map<OperationDto>(entity);
            return entityDto;
        }
    }
}
