using AutoMapper;
using DocumentManagement.Common.UnitOfWork;
using DocumentManagement.Data;
using DocumentManagement.Data.Dto;
using DocumentManagement.Domain;
using DocumentManagement.MediatR.Commands;
using DocumentManagement.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DocumentManagement.MediatR.Handlers
{
    public class UpdateOperationCommandHandler : IRequestHandler<UpdateOperationCommand, OperationDto>
    {
        private readonly IOperationRepository _operationRepository;
        private readonly IUnitOfWork<DocumentContext> _uow;
        private readonly IMapper _mapper;
        public UpdateOperationCommandHandler(
           IOperationRepository operationRepository,
            IMapper mapper,
            IUnitOfWork<DocumentContext> uow
            )
        {
            _operationRepository = operationRepository;
            _mapper = mapper;
            _uow = uow;
        }
        public async Task<OperationDto> Handle(UpdateOperationCommand request, CancellationToken cancellationToken)
        {
            var entityExist = await _operationRepository.FindBy(c => c.Name == request.Name && c.Id != request.Id)
                .FirstOrDefaultAsync();
            if (entityExist != null)
            {
                var errorDto = new OperationDto
                {
                    StatusCode = 409,
                    Messages = new List<string> { "Operation Name Already Exist." }
                };
                return errorDto;
            }
            var entity = _mapper.Map<Operation>(request);
            entityExist = await _operationRepository.FindBy(v => v.Id == request.Id).FirstOrDefaultAsync();
            entity.CreatedBy = entityExist.CreatedBy;
            entity.CreatedDate = entityExist.CreatedDate;
            _operationRepository.Update(entity);
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
            entityDto.StatusCode = 204;
            return entityDto;
        }
    }
}
